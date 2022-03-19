using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChocolateMint.Common
{
    public interface IDisplayContentLoadingCallbackReceiverInternal<LoadResult>
    {
        /// <summary>
        /// ロード完了
        /// </summary>
        void OnLoadedInternal(LoadResult loadResult);
    }
}
