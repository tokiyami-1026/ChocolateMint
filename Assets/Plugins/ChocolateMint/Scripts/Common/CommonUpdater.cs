using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChocolateMint.Common
{
    public class CommonUpdater : MonoBehaviour
    {
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
            if (!UpdateHandlers.Exists(x => x == handler))
            {
                UpdateHandlers.Add(handler);
            }
        }
    }
}
