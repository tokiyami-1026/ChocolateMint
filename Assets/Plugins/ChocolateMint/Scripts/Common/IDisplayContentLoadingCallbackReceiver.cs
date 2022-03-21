namespace ChocolateMint.Common
{
    /// <summary>
    /// DisplayContentに紐づいているアセットのロードが完了したときに呼ぶ
    /// </summary>
    /// <typeparam name="LoadResult">ロード完了時に受け取るパラメータのタイプ</typeparam>
    /// <typeparam name="TOnLoadedResult">OnLoadedInternalの処理が完了して呼び出し元に返すパラメータのタイプ</typeparam>
    public interface IDisplayContentLoadingCallbackReceiverInternal<LoadResult,TOnLoadedResult>
    {
        /// <summary>
        /// アセットのロードが完了した
        /// </summary>
        /// <param name="loadResult">ロード完了時に受け取るパラメータ</param>
        /// <returns>OnLoadedInternalの処理が完了して呼び出し元に返すパラメータ</returns>
        TOnLoadedResult OnLoadedInternal(LoadResult loadResult);
    }
}
