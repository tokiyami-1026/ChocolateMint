using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ChocolateMint.Service;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using ChocolateMint.Common;
using UnityEngine.Playables;
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace ChocolateMint.Scene
{
    /// <summary>
    /// シーン全般の管理を行うサービス
    /// </summary>
    public class GameSceneManagementService : ServiceBase, IServiceParameter<Type>
    {
        /// <summary>
        /// 遷移パラメータ
        /// </summary>
        private class TransitionParameter
        {
            /// <summary>
            /// シーン
            /// </summary>
            public DisplayContent gameScene;

            /// <summary>
            /// パラメータをシーンへ送る関数デリゲート
            /// </summary>
            /// <remarks>
            /// パラメータなしのシーンの場合はnull
            /// </remarks>
            public Action preInitializeWithSceneParameter;
        }

        /// <summary>
        /// GameSceneとSceneViewのセット
        /// </summary>
        private class GameSceneSet
        {
            /// <summary>
            /// GameScene
            /// </summary>
            public DisplayContent gameScene;

            /// <summary>
            /// GameSceneView
            /// </summary>
            public DisplayContentView gameSceneView;
        }

        /// <summary>
        /// 遷移待ちのパラメータキュー
        /// </summary>
        private Queue<TransitionParameter> transitionQueueParameters = new Queue<TransitionParameter>();

        /// <summary>
        /// 現在アクティブなシーン
        /// </summary>
        private GameSceneSet currentActiveScene;

        /// <summary>
        /// 遷移中かどうか
        /// </summary>
        private bool isTransitioning = false;

        /// <summary>
        /// ゲーム開始時のシーンタイプ
        /// </summary>
        private Type startGameSceneType;

        /// <summary>
        /// サービス登録時のパラメータを受け取る
        /// </summary>
        /// <param name="startGameSceneType">ゲーム開始時のシーンタイプ</param>
        public void PreStartup(Type startGameSceneType)
        {
            this.startGameSceneType = startGameSceneType;
        }

        /// <summary>
        /// 開始処理
        /// </summary>
        public override void Startup()
        {
            // ゲーム開始時のシーンのインスタンスを作る
            var startGameScene = (DisplayContent)Activator.CreateInstance(startGameSceneType);
            var activeUnityScene = SceneManager.GetActiveScene();
            
            // 現在アクティブなシーンとして設定
            currentActiveScene = new GameSceneSet()
            {
                gameScene = startGameScene,
                gameSceneView = ((IDisplayContentLoadingCallbackReceiverInternal<UnityScene, DisplayContentView>)startGameScene).OnLoadedInternal(activeUnityScene),
            };
        }

        /// <summary>
        /// 指定したシーンへの遷移をリクエストする
        /// </summary>
        /// <typeparam name="TGameScene">遷移するシーンタイプ</typeparam>
        public void RequestTransitionGameScene<TGameScene>() 
            where TGameScene : DisplayContent, IDisplayContentLoadingCallbackReceiverInternal<UnityScene, DisplayContentView>, new()
        {
            var parameter = new TransitionParameter()
            {
                gameScene = new TGameScene(),
            };
            transitionQueueParameters.Enqueue(parameter);
        }

        /// <summary>
        /// 指定したシーンへの遷移をリクエストする（シーンへ渡すパラメータも設定できる）
        /// </summary>
        /// <typeparam name="TGameScene">遷移するシーンタイプ</typeparam>
        /// <typeparam name="TSceneParameter">シーンに渡すパラメータタイプ</typeparam>
        /// <param name="sceneParameter">シーンに渡すパラメータ</param>
        public void RequestTransitionGameScene<TGameScene,TSceneParameter>(TSceneParameter sceneParameter) 
            where TGameScene : DisplayContent, IDisplayContentLoadingCallbackReceiverInternal<UnityScene, DisplayContentView>, IGameSceneParameter<TSceneParameter>,new()
        {
            var gameScene = new TGameScene();
            var parameter = new TransitionParameter()
            {
                gameScene = gameScene,
                preInitializeWithSceneParameter = () => gameScene.PreInitialize(sceneParameter),
            };
            transitionQueueParameters.Enqueue(parameter);
        }

        /// <summary>
        /// 指定したシーンに遷移する
        /// </summary>
        /// <param name="parameter">遷移パラメータ</param>
        private async UniTask TransitionGameSceneInternal(TransitionParameter parameter)
        {
            // シーン遷移開始
            isTransitioning = true;

            // 遷移前のシーンから抜けるときのExitアニメーションを再生する
            var animExit = ((IDisplayContentTransitionAnimation)currentActiveScene.gameSceneView)?.TransitionAnimExit;
            if (animExit != null)
            {
                await PlayTransitionAnim(animExit);
            }

            // 現在のシーンの終了処理を呼ぶ
            currentActiveScene.gameScene.Terminate();

            // 指定シーンへ遷移する
            var loadScene = parameter.gameScene;
            await SceneManager.LoadSceneAsync(loadScene.ContentAssetPath);
            var activeScene = SceneManager.GetActiveScene();

            // ロード完了
            parameter.preInitializeWithSceneParameter?.Invoke();
            var view = ((IDisplayContentLoadingCallbackReceiverInternal<UnityScene, DisplayContentView>)loadScene).OnLoadedInternal(activeScene);
            
            // 現在のアクティブなシーンを設定
            currentActiveScene = new GameSceneSet()
            {
                gameScene = loadScene,
                gameSceneView = view,
            };

            // 遷移してきたシーンのEnterアニメーションを再生
            var enterAnim = ((IDisplayContentTransitionAnimation)currentActiveScene.gameSceneView)?.TransitionAnimEnter;
            if (enterAnim != null)
            {
                await PlayTransitionAnim(enterAnim);
                enterAnim.gameObject.SetActive(false);
            }

            // シーン遷移終了
            isTransitioning = false;
        }

        /// <summary>
        /// 遷移アニメーション再生
        /// </summary>
        /// <param name="transitionAnim">遷移アニメーション</param>
        private UniTask PlayTransitionAnim(PlayableDirector transitionAnim)
        {
            transitionAnim.gameObject.SetActive(true);
            transitionAnim.Play();
            return UniTask.WaitWhile(() => transitionAnim.state == PlayState.Playing);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public override void Update()
        {
            // 現在のシーンの更新処理
            if (currentActiveScene?.gameScene != null)
            {
                currentActiveScene.gameScene.Update();
            }

            var isTransitionable =
                // 遷移待ちのキューがある
                transitionQueueParameters.Any() &&
                // 現在遷移中ではない
                !isTransitioning;

            if (isTransitionable)
            {
                // 遷移する
                var parameter = transitionQueueParameters.Dequeue();
                TransitionGameSceneInternal(parameter).Forget();
            }
        }
    }
}