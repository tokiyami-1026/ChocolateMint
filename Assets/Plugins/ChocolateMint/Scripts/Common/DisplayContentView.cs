using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace ChocolateMint.Common
{
    public class DisplayContentView : MonoBehaviour
    {
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
