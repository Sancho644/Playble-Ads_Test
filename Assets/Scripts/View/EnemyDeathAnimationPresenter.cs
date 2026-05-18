using Gameplay;
using UnityEngine;

namespace View
{
    public sealed class EnemyDeathAnimationPresenter
    {
        private readonly RemovalRequestComponent _removalRequest;
        private readonly VisibilityComponent _visibility;
        private readonly Animator _animator;
        private readonly string _idleStateName;
        private readonly string _deathStateName;
        private readonly string _deathTriggerParameter;
        private readonly string _deathBoolParameter;
        private readonly float _fallbackDuration;

        private bool _deathStarted;
        private bool _visibilityApplied;
        private float _elapsed;
        private readonly int _deathTriggerHash;
        private readonly int _deathBoolHash;
        private readonly bool _useDeathTrigger;
        private readonly bool _useDeathBool;

        public EnemyDeathAnimationPresenter(
            RemovalRequestComponent removalRequest,
            VisibilityComponent visibility,
            Animator animator,
            string idleStateName,
            string deathStateName,
            string deathTriggerParameter,
            string deathBoolParameter,
            float fallbackDuration)
        {
            _removalRequest = removalRequest;
            _visibility = visibility;
            _animator = animator;
            _idleStateName = idleStateName;
            _deathStateName = deathStateName;
            _deathTriggerParameter = deathTriggerParameter;
            _deathBoolParameter = deathBoolParameter;
            _fallbackDuration = fallbackDuration;
            _useDeathTrigger = !string.IsNullOrWhiteSpace(deathTriggerParameter);
            _useDeathBool = !string.IsNullOrWhiteSpace(deathBoolParameter);
            _deathTriggerHash = _useDeathTrigger ? Animator.StringToHash(deathTriggerParameter) : 0;
            _deathBoolHash = _useDeathBool ? Animator.StringToHash(deathBoolParameter) : 0;
        }

        public void PlayIdle()
        {
            if (_animator == null || string.IsNullOrWhiteSpace(_idleStateName))
            {
                return;
            }

            _animator.Rebind();
            _animator.Update(0f);
            _animator.Play(_idleStateName, 0, 0f);
            _animator.Update(0f);
        }

        public void Refresh(float deltaTime)
        {
            if (!_deathStarted)
            {
                MaintainIdleLoop();
            }

            if (_removalRequest == null || !_removalRequest.IsRequested || _visibilityApplied)
            {
                return;
            }

            if (!_deathStarted)
            {
                StartDeath();
            }

            _elapsed += deltaTime;

            if (!IsDeathFinished())
            {
                return;
            }

            _visibility?.Hide();
            _visibilityApplied = true;
        }

        private void StartDeath()
        {
            _deathStarted = true;

            if (_animator == null)
            {
                return;
            }

            if (_useDeathBool)
            {
                _animator.SetBool(_deathBoolHash, true);
            }

            if (_useDeathTrigger)
            {
                _animator.SetTrigger(_deathTriggerHash);
            }

            if (!string.IsNullOrWhiteSpace(_deathStateName))
            {
                _animator.CrossFade(_deathStateName, 0.05f, 0, 0f);
            }
        }

        private bool IsDeathFinished()
        {
            if (_animator == null)
            {
                return _elapsed >= _fallbackDuration;
            }

            if (string.IsNullOrWhiteSpace(_deathStateName))
            {
                return _elapsed >= _fallbackDuration;
            }

            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            var isDeathState = stateInfo.IsName(_deathStateName) || stateInfo.IsName($"Base Layer.{_deathStateName}");
            if (!isDeathState)
            {
                return _elapsed >= _fallbackDuration;
            }

            return !_animator.IsInTransition(0) && stateInfo.normalizedTime >= 1f;
        }

        private void MaintainIdleLoop()
        {
            if (_animator == null || string.IsNullOrWhiteSpace(_idleStateName) || _animator.IsInTransition(0))
            {
                return;
            }

            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            var isIdleState = stateInfo.IsName(_idleStateName) || stateInfo.IsName($"Base Layer.{_idleStateName}");
            if (!isIdleState || stateInfo.loop || stateInfo.normalizedTime < 0.999f)
            {
                return;
            }

            _animator.Play(_idleStateName, 0, 0f);
        }
    }
}
