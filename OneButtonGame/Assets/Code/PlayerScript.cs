using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] int currentCheckpoint = 0;
    CheckpointManager checkpointManager;
    EffectsScript effects;

    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] float burnOffSpeed = 0.2f;
    [SerializeField] int maxSpeed = 20;
    public bool leftMousedown;
    public float speed;
    public bool canMove = true;
    [SerializeField] int bounceStrength = 20;

    Vector2 forward;
    Vector2 moveDir;
    float angle;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        checkpointManager = FindFirstObjectByType<CheckpointManager>();
        effects = FindFirstObjectByType<EffectsScript>();
        transform.position = checkpointManager.ReturnCheckpoint(currentCheckpoint);
    }
    void Update()
    {
        forward =
            (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane))
            - transform.position).normalized;
        angle = Mathf.Atan2(forward.x, forward.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

        if (Input.GetMouseButtonDown(0)) leftMousedown = true;
        if (Input.GetMouseButtonUp(0)) leftMousedown = false;

    }
    void FixedUpdate()
    {
        if (leftMousedown)
        {
            moveDir = forward;
            speed += moveSpeed;
            if (speed >= maxSpeed) speed = maxSpeed;
        }
        if (!leftMousedown)
        {
            if (speed > 0) speed -= burnOffSpeed;
            else speed = 0;
        }
        if (canMove)
        {
            rb.position += speed * Time.deltaTime * moveDir.normalized;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            currentCheckpoint = checkpointManager.ReturnIndex(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Danger"))
        {
            Death();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bouncy"))
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(bounceStrength * - speed * moveDir.normalized);
            StartCoroutine(DisableMovement(0.5f));
        }
    }
    public void Death()
    {
        effects.Death(gameObject);
        transform.position = checkpointManager.ReturnCheckpoint(currentCheckpoint);
        speed = 0;
        rb.linearVelocity = Vector2.zero;
    }
    IEnumerator DisableMovement(float f)
    {
        canMove = false;
        yield return new WaitForSeconds(f);
        rb.linearVelocity = Vector2.zero;
        canMove = true;
    }
}
