using ChocolateMint.Common;
using System;
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace ChocolateMint.Scene
{
    /// <summary>
    /// MVCモデルを実装したゲームシーンクラス
    /// </summary>
    /// <typeparam name="TModel">Modelタイプ</typeparam>
    /// <typeparam name="TView">Viewタイプ</typeparam>
    public abstract class GameSceneController<TModel, TView> : DisplayContentController<TModel, TView>, IDisplayContentLoadingCallbackReceiverInternal<UnityScene, DisplayContentView>
        where TModel : DisplayContentModel,new()
        where TView : DisplayContentView
    {
        /// <summary>
        /// Unityシーンのロードが完了した
        /// </summary>
        /// <param name="loadedScene">Unityシーン</param>
        /// <returns>シーン内のView</returns>
        DisplayContentView IDisplayContentLoadingCallbackReceiverInternal<UnityScene, DisplayContentView>.OnLoadedInternal(UnityScene loadedScene)
        {
            Initialize();

            // シーン内のオブジェクトにアタッチされているViewを取得する
            var rootObjects = loadedScene.GetRootGameObjects();
            foreach (var root in rootObjects)
            {
                view = root.GetComponentInChildren<TView>(includeInactive:true);
                if (view != null)
                {
                    break;
                }
            }

            // Viewが見つかった
            if (view != null)
            {
                view.InitializeInternal(MessageBroker);
                view.Initialize();

                return view;
            }
            // Viewがなかった
            else
            {
                throw new Exception("シーン内のViewの取得に失敗しました。");
            }
        }
    }
}
