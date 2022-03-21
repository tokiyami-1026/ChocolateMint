using System.Collections.Generic;
using UnityEngine;

namespace ChocolateMint.Common
{
    /// <summary>
    /// IUpdateHandlerが実装されているクラスのUpdate関数を一括で処理する
    /// </summary>
    public class CommonUpdater : MonoBehaviour
    {
        /// <summary>
        /// 一括で処理するIUpdateHandlerのリスト
        /// </summary>
        private readonly List<IUpdateHandler> UpdateHandlers = new List<IUpdateHandler>();

        /// <summary>
        /// 更新
        /// </summary>
        private void Update()
        {
            foreach (var handler in UpdateHandlers)
            {
                handler.Update();
            }
        }

        /// <summary>
        /// 更新処理の追加
        /// </summary>
        public void AddUpdateHandler(IUpdateHandler handler)
        {
            // 同一のものがなければ追加する
            if (!UpdateHandlers.Exists(x => x == handler))
            {
                UpdateHandlers.Add(handler);
            }
        }
    }
}
