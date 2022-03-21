namespace ChocolateMint.Common
{
    /// <summary>
    /// 表示物の最基底クラス
    /// </summary>
    public abstract class DisplayContent : IUpdateHandler
    {
        public abstract string ContentAssetPath { get; }

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// 更新
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// 終了処理
        /// </summary>
        public virtual void Terminate() { }
    }
}
