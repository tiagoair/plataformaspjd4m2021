using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float velocidade;

    [SerializeField] private PlayerInput playerInput;
    
    private Rigidbody2D _rigidbody2D;

    private GameInput _gameInput;

    private Vector2 _movimento;

    private void OnEnable()
    {
        playerInput.onActionTriggered += OnActionTriggered;
    }

    private void OnDisable()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gameInput = new GameInput();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody2D.AddForce(_movimento * velocidade);
        
    }
    
    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        if (String.Compare(obj.action.name, _gameInput.Gameplay.Move.name, StringComparison.Ordinal)==0)
        {
            _movimento = obj.ReadValue<Vector2>(); 
            Debug.Log(_movimento.ToString());
        }
    }
    
    
}
