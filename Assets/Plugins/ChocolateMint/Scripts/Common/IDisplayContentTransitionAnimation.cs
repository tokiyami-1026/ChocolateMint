using UnityEngine.Playables;

namespace ChocolateMint.Common
{
    /// <summary>
    /// DisplayContentの遷移を行うときのアニメーション
    /// </summary>
    public interface IDisplayContentTransitionAnimation
    {
        /// <summary>
        /// 別の表示物からこの表示物に遷移してきたときのアニメーション
        /// </summary>
        PlayableDirector TransitionAnimEnter { get; }

        /// <summary>
        /// この表示物から別の表示物に遷移するときのアニメーション
        /// </summary>
        PlayableDirector TransitionAnimExit { get; }
    }
}
