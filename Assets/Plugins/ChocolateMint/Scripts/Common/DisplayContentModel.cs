using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChocolateMint.Common
{
    public abstract class DisplayContentModel
    {
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
