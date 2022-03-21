using ChocolateMint.Common;

namespace ChocolateMint.Service
{
    /// <summary>
    /// サービスのベースクラス
    /// </summary>
    public abstract class ServiceBase : IUpdateHandler
    {
        /// <summary>
        /// サービスのセットアップ
        /// </summary>
        public virtual void Startup() { }

        /// <summary>
        /// 更新
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// サービスの終了処理
        /// </summary>
        public virtual void Shutdown() { }
    }
}
