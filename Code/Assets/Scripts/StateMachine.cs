using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum MovementState
{
    IDLE,
    RUNNING,
    ROLLING,
    SPRINTING,
}

public class StateMachine : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _playerState;
    [SerializeField] Animator _animator;
    [SerializeField] AudioSource _footSteps;
    [SerializeField] AudioSource _rollSound;


    /// L'état courant
    private MovementState _currentState;

    public MovementState CurrentState  //Propriété pour pouvoir mettre à jour cette variable dans l'UI
    {
        get
        {
            return _currentState;
        }
    }

    /// Les composants
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerController.PlayerNormalSpeed(); // On initialise la vitesse du Player au lancement du jeu
        OnStateEnter(MovementState.IDLE);      
    }

    private void Update()
    {
        OnStateUpdate(_currentState);
    }

    private void OnStateEnter(MovementState state)
    {
        switch (state)
        {
            case MovementState.IDLE:
                OnStateEnterIdle();
                break;

            case MovementState.RUNNING:
                OnStateEnterRun();
                break;

            case MovementState.ROLLING:
                OnStateEnterRoll();
                break;

            case MovementState.SPRINTING:
                OnStateEnterSprinting();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void OnStateUpdate(MovementState state)
    {
        switch (state)
        {
            case MovementState.IDLE:
                OnStateUpdateIdle();
                break;

            case MovementState.RUNNING:
                OnStateUpdateRun();
                break;

            case MovementState.ROLLING:
                OnStateUpdateRoll();
                break;

            case MovementState.SPRINTING:
                OnStateUpdateSprinting();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void OnStateExit(MovementState state)
    {
        switch (state)
        {
            case MovementState.IDLE:
                OnStateExitIdle();
                break;

            case MovementState.RUNNING:
                OnStateExitRun();
                break;

            case MovementState.ROLLING:
                OnStateExitRoll();
                break;

            case MovementState.SPRINTING:
                OnStateExitSprinting();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void TransitionToState(MovementState toState)
    {
        OnStateExit(_currentState);
        _currentState = toState;
        OnStateEnter(toState);
    }

    private void OnStateEnterIdle()
    {
        _animator.SetTrigger("Idle");
    }

    private void OnStateUpdateIdle()
    {
        if (_playerController.IsMoving() && !_playerController.RollingKeyIsHeldDown()) // Si le Player bouge et que la touche roulade n'est pas maintenue enfoncée, on passe en état RUNNING
        {
            TransitionToState(MovementState.RUNNING);         
        }
        else if (!_playerController.IsMoving() && _playerController.RollingKeyIsPressed()) // Si le Player est immobile et que la touche roulade vient d'être enfoncée, on passe en état ROLLING
        {
            TransitionToState(MovementState.ROLLING); 
        }
        else if (_playerController.IsMoving() && _playerController.RollingKeyIsHeldDown()) // Si le Player bouge et que la touche roulade est maintenue enfoncée, on passe en état SPRINTING
        {
                TransitionToState(MovementState.SPRINTING);    
        }          
    }

    private void OnStateExitIdle()
    {

    }

    private void OnStateEnterRun()
    {
        _animator.SetTrigger("Run");
        _footSteps.Play();
    }

    private void OnStateUpdateRun()
    {
        if (!_playerController.IsMoving()) // Si le Player est immobile, on passe en état IDLE
        {
            TransitionToState(MovementState.IDLE); 
        }
        else if (_playerController.RollingKeyIsPressed()) // Si on appuie sur la touche roulade, on passe en état roulade
        {
            TransitionToState(MovementState.ROLLING);
        }
    }

    private void OnStateExitRun()
    {
        _footSteps.Stop();
    }

    private void OnStateEnterRoll()
    {
        _animator.SetTrigger("Roll");
        _playerController.Roll();
        _rollSound.Play();
    }

    private void OnStateUpdateRoll()
    {
        if (_playerController.RollIsEnd())
        {
            if (!_playerController.IsMoving()) // Si le Player est immobile on revient à l'état IDLE
            {
                TransitionToState(MovementState.IDLE);
            }
            else if (_playerController.IsMoving() && !_playerController.RollingKeyIsHeldDown()) // Si le Player bouge et que la touche de roulade n'est pas enfoncée, on passe à l'état RUNNING
            {
                TransitionToState(MovementState.RUNNING);
            }
            else if (_playerController.IsMoving() && _playerController.RollingKeyIsHeldDown()) // Si le Player bouge et que la touche roulade est enfoncée, on passse à l'état SPRINTING
            {
                TransitionToState(MovementState.SPRINTING);
            }
        }        
    }

    private void OnStateExitRoll()
    {
        
    }

    private void OnStateEnterSprinting()
    {
        _animator.SetTrigger("Sprint");
        _playerController.SprintSpeed(); // On passe en mode sprint       
        _footSteps.pitch = 1.5f;
        _footSteps.Play();
    }

    private void OnStateUpdateSprinting()
    {
        if (!_playerController.IsMoving()) // Si le joueur est immobile, on passe en état IDLE 
        {
            TransitionToState(MovementState.IDLE);
        }
        else if (_playerController.IsMoving() && !_playerController.RollingKeyIsHeldDown()) // Si le joueur bouge et que la touche roulade n'est pas enfoncée on passe en état RUNNING
        {
            TransitionToState(MovementState.RUNNING);
        }
    }

    private void OnStateExitSprinting()
    {
        _playerController.PlayerNormalSpeed(); // Avant de quitter l'état sprint, on revient à la vitesse normal
        _footSteps.pitch = 1f;
        _footSteps.Stop(); ;
    }
}
