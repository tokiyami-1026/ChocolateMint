using UnityEngine;
using ChocolateMint.Service;
using ChocolateMint.Scene;
using ChocolateMint.Common;
using UnityScene = UnityEngine.SceneManagement.Scene;
using System;

namespace ChocolateMint
{
    /// <summary>
    /// ゲームメイン（エントリポイント）
    /// </summary>
    public class GameMain
    {
        private readonly ServiceManager ServiceManager = new ServiceManager();
        private CommonUpdater commonUpdater = default;

        /// <summary>
        /// 起動する
        /// </summary>
        public void Run<TStartGameScene>()
            where TStartGameScene : DisplayContent, IDisplayContentLoadingCallbackReceiverInternal<UnityScene, DisplayContentView>, new()
        {
            // 各種サービスを登録
            ServiceManager.RegisterService<GameSceneTransitionService,Type>(typeof(TStartGameScene));

            // CommonUpdater
            var commonUpdaterObject = new GameObject(typeof(CommonUpdater).Name);
            commonUpdater = commonUpdaterObject.AddComponent<CommonUpdater>();
            GameObject.DontDestroyOnLoad(commonUpdaterObject);

            commonUpdater.AddUpdateHandler(ServiceManager);
        }

        /// <summary>
        /// 終了する
        /// </summary>
        public void Shutdown()
        {

        }
    }
}
