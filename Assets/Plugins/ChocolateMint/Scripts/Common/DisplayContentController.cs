using UniRx;

namespace ChocolateMint.Common
{
    /// <summary>
    /// ModelとViewを持ったMVCモデルのDisplayContent
    /// </summary>
    /// <typeparam name="TModel">Modelタイプ</typeparam>
    /// <typeparam name="TView">Viewタイプ</typeparam>
    public abstract class DisplayContentController<TModel,TView> : DisplayContent
        where TModel : DisplayContentModel,new()
        where TView : DisplayContentView
    {
        /// <summary>
        /// モデル
        /// </summary>
        protected readonly TModel Model = new TModel();

        /// <summary>
        /// ビュー
        /// </summary>
        protected TView view = default;

        /// <summary>
        /// クラス間のメッセージのやり取りを行う
        /// </summary>
        /// <remarks>
        /// 主にViewからのメッセージを受け取り、それに応じた処理をControllerで行う
        /// </remarks>
        protected readonly MessageBroker MessageBroker = new MessageBroker();
    }
}
