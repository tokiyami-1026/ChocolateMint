using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ChocolateMint.Service;
using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UniRx;
using ChocolateMint.Common;
using UnityEngine.Playables;
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace ChocolateMint.Scene
{
    public class GameSceneTransitionService : ServiceBase, IServiceParameter<Type>
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
            /// <remarks>
            /// パラメータなしのシーンの場合はnull
            /// </remarks>
            /// </summary>
            public Action preInitializeWithSceneParameter;
        }

        /// <summary>
        /// GameSceneとSceneViewのセット
        /// </summary>
        private class GameSceneSet
        {
            public DisplayContent gameScene;

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
        /// サービス開始時のパラメータ取得
        /// </summary>
        /// <param name="parameter">パラメータ</param>
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
            currentActiveScene = new GameSceneSet()
            {
                gameScene = startGameScene,
                gameSceneView = ((IDisplayContentLoadingCallbackReceiverInternal<UnityScene, DisplayContentView>)startGameScene).OnLoadedInternal(activeUnityScene),
            };
        }

        /// <summary>
        /// 指定したシーンへ遷移する
        /// </summary>
        /// <typeparam name="TGameScene">遷移先のシーンタイプ</typeparam>
        public void TransitionGameScene<TGameScene>() 
            where TGameScene : DisplayContent, IDisplayContentLoadingCallbackReceiverInternal<UnityScene, DisplayContentView>, new()
        {
            var parameter = new TransitionParameter()
            {
                gameScene = new TGameScene(),
            };
            transitionQueueParameters.Enqueue(parameter);
        }

        /// <summary>
        /// 指定したシーンへ遷移する（パラメータ付き）
        /// </summary>
        /// <typeparam name="TGameScene">遷移先のシーンタイプ</typeparam>
        /// <typeparam name="TSceneParameter">遷移先のシーンに渡すパラメータタイプ</typeparam>
        /// <param name="sceneParameter">遷移先のシーンに渡すパラメータ</param>
        public void TransitionGameScene<TGameScene,TSceneParameter>(TSceneParameter sceneParameter) 
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
        /// 指定したシーンへ遷移する
        /// </summary>
        private async UniTask TransitionGameSceneInternal(TransitionParameter parameter)
        {
            // 遷移開始アニメーション
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
            currentActiveScene = new GameSceneSet()
            {
                gameScene = loadScene,
                gameSceneView = view,
            };

            // 遷移終了アニメーション
            var enterAnim = ((IDisplayContentTransitionAnimation)currentActiveScene.gameSceneView)?.TransitionAnimEnter;
            if (enterAnim != null)
            {
                await PlayTransitionAnim(enterAnim);
                enterAnim.gameObject.SetActive(false);
            }

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
            // 遷移待ちのキューがなければ無視
            if (!transitionQueueParameters.Any()) { return; }

            // 遷移中なら無視
            if (isTransitioning) { return; }
            
            isTransitioning = true;

            // 遷移する
            var parameter = transitionQueueParameters.Dequeue();
            TransitionGameSceneInternal(parameter).Forget();
        }
    }
}