using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace ChocolateMint.Common
{
    public interface IDisplayContentTransitionAnimation
    {
        PlayableDirector TransitionAnimEnter { get; }

        PlayableDirector TransitionAnimExit { get; }
    }
}
