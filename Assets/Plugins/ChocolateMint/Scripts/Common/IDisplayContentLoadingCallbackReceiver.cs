using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChocolateMint.Common
{
    public interface IDisplayContentLoadingCallbackReceiverInternal<LoadResult,TOnLoadedResult>
    {
        /// <summary>
        /// ロード完了
        /// </summary>
        TOnLoadedResult OnLoadedInternal(LoadResult loadResult);
    }
}
