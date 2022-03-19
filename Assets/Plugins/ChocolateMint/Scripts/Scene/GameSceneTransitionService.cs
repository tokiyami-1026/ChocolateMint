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
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace ChocolateMint.Scene
{
    public class GameSceneTransitionService : ServiceBase
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
            /// パラメータ付きの初期化関数デリゲート
            /// <remarks>
            /// パラメータなしのシーンの場合はnull
            /// </remarks>
            /// </summary>
            public Action initializeWithSceneParameter;
        }

        /// <summary>
        /// 遷移待ちのパラメータキュー
        /// </summary>
        private Queue<TransitionParameter> transitionQueueParameters = new Queue<TransitionParameter>();

        /// <summary>
        /// 遷移中かどうか
        /// </summary>
        private bool isTransitioning = false;

        /// <summary>
        /// 指定したシーンへ遷移する
        /// </summary>
        /// <typeparam name="TGameScene">遷移先のシーンタイプ</typeparam>
        public void TransitionGameScene<TGameScene>() 
            where TGameScene : DisplayContent, IDisplayContentLoadingCallbackReceiverInternal<UnityScene>, new()
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
            where TGameScene : DisplayContent, IDisplayContentLoadingCallbackReceiverInternal<UnityScene>, IGameSceneParameter<TSceneParameter>,new()
        {
            var gameScene = new TGameScene();
            var parameter = new TransitionParameter()
            {
                gameScene = gameScene,
                initializeWithSceneParameter = () => gameScene.PreInitialize(sceneParameter),
            };
            transitionQueueParameters.Enqueue(parameter);
        }

        /// <summary>
        /// 指定したシーンへ遷移する
        /// </summary>
        private async UniTask TransitionGameSceneInternal(TransitionParameter parameter)
        {
            // TODO : 遷移開始アニメーション

            // 指定シーンへ遷移する
            var loadScene = parameter.gameScene;
            await SceneManager.LoadSceneAsync(loadScene.ContentAssetPath);
            var activeScene = SceneManager.GetActiveScene();

            // ロード完了
            parameter.initializeWithSceneParameter?.Invoke();
            ((IDisplayContentLoadingCallbackReceiverInternal<UnityScene>)loadScene).OnLoadedInternal(activeScene);

            // TODO : 遷移終了アニメーション

            isTransitioning = false;
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