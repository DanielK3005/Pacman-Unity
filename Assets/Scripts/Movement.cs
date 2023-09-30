using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 8.0f;
    public float speedMultiplier = 1.0f;
    [SerializeField] private Vector2 intialDirection;
    [SerializeField] private LayerMask wallsLayer;
    public Rigidbody2D rbody { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startPosition { get; private set; }

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        if(nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rbody.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;
        rbody.MovePosition(position + translation);
    }

    public void ResetState()
    {
        speedMultiplier = 1.0f;
        direction = intialDirection;
        nextDirection = Vector2.zero;
        transform.position = startPosition;
        rbody.isKinematic = false;
        enabled = true;

    }

    public void SetDirection(Vector2 dir, bool forced = false)
    {
        if(forced || !Occupied(dir))
        {
            direction = dir;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = dir;
        }

    }

    public bool Occupied(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0.0f, dir, 1.5f, wallsLayer);
        return hit.collider != null;
    }
}
