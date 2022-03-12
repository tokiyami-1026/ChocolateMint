using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace ChocolateMint.Common
{
    public abstract class DisplayContentController<TModel,TView> : DisplayContent
        where TModel : DisplayContentModel,new()
        where TView : DisplayContentView
    {
        protected readonly TModel Model = new TModel();
        protected TView view = default;

        protected readonly MessageBroker MessageBroker = new MessageBroker();
    }
}
