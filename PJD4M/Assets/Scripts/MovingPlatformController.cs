using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public float moveSpeed;
    
    [SerializeField] private bool useTransform;

    [SerializeField] private Transform transformDestination;
    
    [SerializeField] private Vector3 platformDestination;

    private Vector3 _initialPosition;

    private bool _isReturning;

    private Vector2 worldDestination;
    
    private Vector2 _currentMoveDirection;
    
    private void Start()
    {
        _initialPosition = transform.position;
        
        worldDestination = Vector2.zero;

        if (useTransform)
        {
            worldDestination = transformDestination.position;
        }
        else
        {
            worldDestination = _initialPosition + platformDestination;
        }
        _currentMoveDirection = (worldDestination - (Vector2)_initialPosition).normalized;
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (!_isReturning)
        {
            if (Vector2.Distance(transform.position, worldDestination) < 1f)
            {
                _isReturning = true;
                _currentMoveDirection = ((Vector2)_initialPosition - worldDestination).normalized;
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, _initialPosition) < 1f)
            {
                _isReturning = false;
                _currentMoveDirection = (worldDestination - (Vector2)_initialPosition).normalized;
            }
        }

        transform.position += (Vector3) _currentMoveDirection * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (other.contacts.Any(contact => contact.normal == Vector2.down))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        if(useTransform)
            Debug.DrawLine(transform.position, transformDestination.position, Color.yellow);
        else
            Debug.DrawLine(transform.position, transform.position + platformDestination, Color.red);
    }
}
