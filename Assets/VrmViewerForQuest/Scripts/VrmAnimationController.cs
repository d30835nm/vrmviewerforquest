using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace VrmViewer
{
    [RequireComponent(typeof(Animator))]
    public class VrmAnimationController : MonoBehaviour
    {
        PlayableGraph playableGraph;
        AnimationPlayableOutput animationPlayableOutput;

        void Awake()
        {
            playableGraph = PlayableGraph.Create();
            animationPlayableOutput = AnimationPlayableOutput.Create(playableGraph, "output", GetComponent<Animator>());
        }

        public void Play(AnimationClip animationClip)
        {
            var animationClipPlayable = AnimationClipPlayable.Create(playableGraph, animationClip);
            var prePlayable = animationPlayableOutput.GetSourcePlayable();
            if (prePlayable.IsValid())
            {
                prePlayable.Destroy();
            }
            animationPlayableOutput.SetSourcePlayable(animationClipPlayable);
            playableGraph.Play();
        }

        void OnDestroy()
        {
            playableGraph.Destroy();
        }
    }
}