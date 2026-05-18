using System.Collections.Generic;
using Core.States;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public GameState CurrentState { get; private set; }

        private Dictionary<GameState, AbstractGameState> _playerControllers;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _playerControllers = new Dictionary<GameState, AbstractGameState>()
            {
                [GameState.WaitingInput] = new WaitingInputState(),
                [GameState.MovingToTarget] = new MovingToTargetState(),
                [GameState.Interacting] = new MovingToTargetState(),
            };
            
            ChangeState(GameState.WaitingInput);
        }

        public void ChangeState(GameState newState)
        {
            ExitState(CurrentState);

            CurrentState = newState;

            EnterState(CurrentState);
        }

        private void EnterState(GameState currentState)
        {
            if (_playerControllers.TryGetValue(currentState, out AbstractGameState controller))
            {
                CurrentState = currentState;
                controller.EnterState();
            }
            else
            {
                Debug.LogWarning($"No controller found for {currentState}");
            }
        }

        private void ExitState(GameState currentState)
        {
            if (currentState == CurrentState)
            {
                if (_playerControllers.TryGetValue(currentState, out AbstractGameState controller))
                {
                    controller.ExitState();
                    CurrentState = GameState.None;
                }
                else
                {
                    Debug.LogWarning($"No controller found for {currentState}");
                }
            }
            else
            {
                Debug.LogWarning($"Can't exit current state {currentState}");
            }
        }
    }
}