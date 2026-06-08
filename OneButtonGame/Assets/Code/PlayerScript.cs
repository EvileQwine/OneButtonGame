using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] float burnOffSpeed = 1f;
    [SerializeField] int maxSpeed = 30;
    public bool leftMousedown;
    public float speed;

    Vector2 forward;
    Vector2 moveDir;
    Vector2 startPosition;
    float angle;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
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

        if (leftMousedown)
        {
            moveDir = forward;
            speed += moveSpeed;
            if (speed < maxSpeed)
            {
                speed += burnOffSpeed;
            }
            else
            {
                speed = maxSpeed;
            }
        }
        if (!leftMousedown)
        {
            if (speed > 0)
            {
                speed -= burnOffSpeed;
            }
            else
            {
                speed = 0;
            }
        }
        rb.position += speed * Time.deltaTime * moveDir;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Danger"))
        {
            transform.position = startPosition;
            speed = 0;
        }
    }
}
