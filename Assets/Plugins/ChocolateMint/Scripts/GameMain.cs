using UnityEngine;
using ChocolateMint.Service;
using ChocolateMint.Scene;
using ChocolateMint.Common;
using System;
using UnityScene = UnityEngine.SceneManagement.Scene;

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
        /// ライブラリを起動する
        /// </summary>
        /// <typeparam name="TStartGameScene">ゲーム開始時のシーンタイプ</typeparam>
        public void Run<TStartGameScene>()
            where TStartGameScene : DisplayContent, IDisplayContentLoadingCallbackReceiverInternal<UnityScene, DisplayContentView>, new()
        {
            // 各種サービスを登録
            ServiceManager.RegisterService<GameSceneManagementService,Type>(typeof(TStartGameScene));

            // IUpdateHandlerのUpdate関数を登録
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
