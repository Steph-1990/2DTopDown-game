using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rollSpeed;

    private Rigidbody2D _rigidBody2D;
    private Animator _animator;
    private Vector2 _movementDirection;
    private Vector2 _lastMovementDirectionBeforeRoll;
    private Vector2 _lastDirection;
    //private Vector2 __movementDirectionAtStart;
    private float _rollingDuration = 1f; // Durée de la roulade
    private float _lastRollTime;
    private float _horizontalInput;
    private float _verticalInput;
    bool _isRolling;


    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    /*private void Start()
    {
        __movementDirectionAtStart = Vector2.down;       
    }*/

    private void Update()
    {
        GetInput();
        GetLastDirection();

        _animator.SetFloat("VectorX", _lastMovementDirectionBeforeRoll.x);
        _animator.SetFloat("VectorY", _lastMovementDirectionBeforeRoll.y);
    }

    private void FixedUpdate()
    {
        if (_isRolling == true)  // Si le Player est en train de rouler...
        {
            ApplyRollMovement(); // ...On lui applique un mouvement de roulade dans le sens de sa dernière direction enregistrée
        }
        else
        {
            ApplyInput(); // Sinon on lui applique son mouvement habituel
        }
    }


    private void GetLastDirection() // On récupère la dernière direction
    {
        _lastMovementDirectionBeforeRoll = LastDirection();
    }

    private void GetInput() // On récupère les entrées clavier
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        _movementDirection = new Vector2(_horizontalInput, _verticalInput);
        _movementDirection.Normalize();
    } 

    private void ApplyRollMovement()
    {
        _rigidBody2D.velocity = _lastMovementDirectionBeforeRoll * _rollSpeed;
    }

    private void ApplyInput() // On applique les entrées clavier au Rigidbody pour déplacer le Player
    {
        _rigidBody2D.velocity = _movementDirection * _speed;
    } 

    private Vector2 LastDirection() // On stock dans une méthode la dernière direction du joueur avant une roulade
    {
        if (_movementDirection != Vector2.zero && !_isRolling) // Si Le Player est en mouvement mais qu'il n'est pas en train de faire uine roulade...
        {
            _lastDirection = _movementDirection; // On sauvegarde sa direction
            _lastDirection.Normalize();
        }
        return _lastDirection;
    }

    public void Roll() // Roulade du joueur
    {
        _isRolling = true;
        _lastRollTime = Time.time; // On sauvegarde l'heure de la dernière roulade
    }

    public bool RollIsEnd() // On check si la roulade est terminée
    {            
        if (Time.time > _lastRollTime + _rollingDuration)
        {
            _isRolling = false; 
        }

        return !_isRolling; 
    }

    public bool RollingKeyIsPressed() // Retourne True si appuie sur la touche left shift
    {
        return (Input.GetButtonDown("Roll")); 
    }

    public bool RollingKeyIsHeldDown() // Retourne True si la touche left shift est maintenu enfoncé
    {
        return (Input.GetButton("Roll")); 
    }

    public bool IsMoving() // Retourne True si le personnage est en mouvement, sinon retourne false
    {
        return _movementDirection == Vector2.zero ? false : true; 
    }

    public void SprintSpeed() // Le Player passe en vitesse de sprint
    {
        _speed = 15f;
    }

    public void PlayerNormalSpeed() // Le Player repasse en vitesse normal 
    {
        _speed = 8f;
    }
}








// Initialisation de la roulade au lancement du jeu

/*if (_lastMovementDirectionBeforeRoll == Vector2.zero)
{
    if (_isRolling == true)  // Si le Player est en train de rouler...
    {
        _rigidBody2D.velocity = __movementDirectionAtStart * _rollSpeed;
    }
    else
    {
        ApplyInput(); // Sinon on lui applique son mouvement habituel
    }
}
else
{
    if (_isRolling == true)  // Si le Player est en train de rouler...
    {
        ApplyRollMovement(); // ...On lui applique un mouvement de roulade dans le sens de sa dernière direction enregistrée
    }
    else
    {
        ApplyInput(); // Sinon on lui applique son mouvement habituel
    }
}*/