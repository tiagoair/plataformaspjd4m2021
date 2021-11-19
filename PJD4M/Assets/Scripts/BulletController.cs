using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public bool IsFlipped
    {
        get => _isFlipped;
        set => _isFlipped = value;
    }

    public float bulletSpeed;
    
    private Rigidbody2D _rigidbody2D;
    
    private bool _isFlipped;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        GetComponent<SpriteRenderer>().flipX = _isFlipped;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_isFlipped) _rigidbody2D.velocity = Vector2.left * bulletSpeed * Time.fixedDeltaTime;
        else _rigidbody2D.velocity = Vector2.right * bulletSpeed * Time.fixedDeltaTime;
    }
}
