using UnityEngine;
using UniRx;
using UnityEngine.Playables;

namespace ChocolateMint.Common
{
    /// <summary>
    /// MVCモデルのView
    /// </summary>
    public abstract class DisplayContentView : MonoBehaviour, IDisplayContentTransitionAnimation
    {
        /// <summary>
        /// シーン遷移で入ってきたときのアニメーション
        /// </summary>
        [SerializeField]
        private PlayableDirector transitionAnimEnter = default;
        PlayableDirector IDisplayContentTransitionAnimation.TransitionAnimEnter => transitionAnimEnter;

        /// <summary>
        /// シーン遷移でこのシーンから抜けるときのアニメーション
        /// </summary>
        [SerializeField]
        private PlayableDirector transitionAnimExit = default;
        PlayableDirector IDisplayContentTransitionAnimation.TransitionAnimExit => transitionAnimExit;

        /// <summary>
        /// Viewで起きたイベントをControllerへ通知する
        /// </summary>
        protected IMessagePublisher messagePublisher = default;

        /// <summary>
        /// 内部初期化
        /// </summary>
        /// <param name="messagePublisher">メッセージ通知</param>
        internal void InitializeInternal(IMessagePublisher messagePublisher)
        {
            this.messagePublisher = messagePublisher;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// 終了処理
        /// </summary>
        public virtual void Terminate() { }
    }
}
