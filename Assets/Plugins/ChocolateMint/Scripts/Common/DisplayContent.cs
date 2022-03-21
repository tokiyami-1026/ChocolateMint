namespace ChocolateMint.Common
{
    /// <summary>
    /// 表示物の最基底クラス
    /// </summary>
    public abstract class DisplayContent : IUpdateHandler
    {
        /// <summary>
        /// 表示物アセットのパス
        /// </summary>
        /// <remarks>
        /// Path : Asset/...
        /// </remarks>
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
        /// 終了
        /// </summary>
        public virtual void Terminate() { }
    }
}
