using ChocolateMint.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace ChocolateMint.Scene
{
    public abstract class GameSceneController<TModel, TView> : DisplayContentController<TModel, TView>, IDisplayContentLoadingCallbackReceiverInternal<UnityScene, DisplayContentView>
        where TModel : DisplayContentModel,new()
        where TView : DisplayContentView
    {
        /// <summary>
        /// ロードが完了した
        /// </summary>
        /// <param name="loadResult">ロードが完了したUnityのシーン</param>
        DisplayContentView IDisplayContentLoadingCallbackReceiverInternal<UnityScene, DisplayContentView>.OnLoadedInternal(UnityScene loadedScene)
        {
            Initialize();
            Model.Initialize();

            var rootObjects = loadedScene.GetRootGameObjects();
            foreach (var root in rootObjects)
            {
                view = root.GetComponentInChildren<TView>(includeInactive:true);
                if (view != null)
                {
                    break;
                }
            }

            if (view != null)
            {
                view.InitializeInternal(MessageBroker);
                view.Initialize();

                return view;
            }
            else
            {
                throw new Exception("シーン内のViewの取得に失敗しました。");
            }
        }
    }
}
