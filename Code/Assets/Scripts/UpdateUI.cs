using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerState;
    [SerializeField] private StateMachine _stateMachine;

    private void Start()
    {
        _playerState.text = "IDLE"; // On initialise le texte de l'interface avec l'état IDLE au lancement du jeu
    }

    private void Update()
    {
        _playerState.text = "" + _stateMachine.CurrentState; // On met à jour la valeur du score
    }
}