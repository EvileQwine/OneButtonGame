using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
using UnityEngine.Rendering;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject powerUp;
    List<Tuple<Vector3, PowerUps, float, int>> collectedPowerups = new();

    [SerializeField] int currentCheckpoint = 0;
    CheckpointManager checkpointManager;
    EffectsScript effects;
    ScoreScript scoreScript;

    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] float burnOffSpeed = 0.2f;
    [SerializeField] int maxSpeed = 20;
    public bool leftMousedown = false;
    public float speed;
    public bool canMove = true;
    [SerializeField] int bounceStrength = 20;

    Vector2 forward;
    Vector2 moveDir;
    float angle;

    Rigidbody2D rb;

    public int DeathCount = 0;

    public bool damageable = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        checkpointManager = FindFirstObjectByType<CheckpointManager>();
        effects = FindFirstObjectByType<EffectsScript>();
        scoreScript = FindFirstObjectByType<ScoreScript>();
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
            if (!scoreScript.active) scoreScript.Counting(true);        
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
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            int i = checkpointManager.ReturnIndex(other.gameObject);
            if (currentCheckpoint != i)
            {
                checkpointManager.SetRotation(currentCheckpoint);
                currentCheckpoint = i;
                checkpointManager.UpdateRotation(i);
                effects.RemoveAttempts();
                collectedPowerups.Clear();
            }
        }
        else if (other.gameObject.CompareTag("Danger") && damageable)
        {
            Death();
        }
        else if (other.gameObject.CompareTag("Powerup"))
        {
            PowerUp(other.gameObject.GetComponent<PowerUpMemory>().powerup, 
                other.gameObject.GetComponent<PowerUpMemory>().powerMultiplier, 
                other.gameObject.GetComponent<PowerUpMemory>().powerTime);

            collectedPowerups.Add(Tuple.Create(
                other.gameObject.transform.position, 
                other.gameObject.GetComponent<PowerUpMemory>().powerup, other.gameObject.GetComponent<PowerUpMemory>().powerMultiplier,
                other.gameObject.GetComponent<PowerUpMemory>().powerTime));

            Destroy(other.gameObject);
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
        scoreScript.Counting(false);
        effects.Death(gameObject);
        transform.position = checkpointManager.ReturnCheckpoint(currentCheckpoint);
        speed = 0;
        rb.linearVelocity = Vector2.zero;
        DeathCount++;
        for (int i = 0; i < collectedPowerups.Count; i++)
        {
            GameObject thing = Instantiate(powerUp, collectedPowerups[i].Item1, Quaternion.identity);
            thing.GetComponent<PowerUpMemory>().powerup = collectedPowerups[i].Item2;
            thing.GetComponent<PowerUpMemory>().powerMultiplier = collectedPowerups[i].Item3;
            thing.GetComponent<PowerUpMemory>().powerTime = collectedPowerups[i].Item4;
        }
        collectedPowerups.Clear();
        ClearPowers();
    }
    IEnumerator DisableMovement(float f)
    {
        canMove = false;
        yield return new WaitForSeconds(f);
        rb.linearVelocity = Vector2.zero;
        speed = 0;
        canMove = true;
    }
    public void PowerUp(PowerUps powerup, float multiplier, int time)
    {
        switch(powerup)
        {
            case PowerUps.TimeDelay:
                scoreScript.DelayTime(multiplier, time);
                break;
            case PowerUps.Invincible:
                StartCoroutine(Invincible(time));
                break;
        }
    }
    public void ClearPowers()
    {
        scoreScript.delay = 1;
    }
    IEnumerator Invincible(float f)
    {
        damageable = false;
        yield return new WaitForSeconds(f);
        damageable = true;
    }
}
