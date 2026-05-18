using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace View
{
    public sealed class CharacterAnimationPlayable : ICharacterAnimationDriver
    {
        private readonly PlayableGraph _graph;
        private readonly AnimationMixerPlayable _mixer;
        private readonly bool _isValid;

        public CharacterAnimationPlayable(Animator animator, AnimationClip idleClip, AnimationClip runClip)
        {
            if (animator == null || idleClip == null || runClip == null)
            {
                _graph = default;
                _mixer = default;
                _isValid = false;
                return;
            }

            _graph = PlayableGraph.Create("CharacterAnimationPlayable");
            var output = AnimationPlayableOutput.Create(_graph, "Animation", animator);
            _mixer = AnimationMixerPlayable.Create(_graph, 2, true);

            var idlePlayable = AnimationClipPlayable.Create(_graph, idleClip);
            idlePlayable.SetApplyFootIK(false);
            idlePlayable.SetApplyPlayableIK(false);

            var runPlayable = AnimationClipPlayable.Create(_graph, runClip);
            runPlayable.SetApplyFootIK(false);
            runPlayable.SetApplyPlayableIK(false);

            _graph.Connect(idlePlayable, 0, _mixer, 0);
            _graph.Connect(runPlayable, 0, _mixer, 1);

            _mixer.SetInputWeight(0, 1f);
            _mixer.SetInputWeight(1, 0f);

            output.SetSourcePlayable(_mixer);
            _graph.Play();
            _isValid = true;
        }

        public void SetMoving(bool isMoving)
        {
            if (!_isValid)
            {
                return;
            }

            _mixer.SetInputWeight(0, isMoving ? 0f : 1f);
            _mixer.SetInputWeight(1, isMoving ? 1f : 0f);
        }

        public void Update(float deltaTime)
        {
        }

        public void Dispose()
        {
            if (_isValid && _graph.IsValid())
            {
                _graph.Destroy();
            }
        }
    }
}
