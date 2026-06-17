using UnityEngine;

public enum PowerUps
{
    TimeDelay,
    Invincible,
    Acceleration,
    BurnSpeed,
    Laser,
}
public class PowerUpMemory : MonoBehaviour
{
    public PowerUps powerup;
    public float powerMultiplier;
    public int powerTime;
}
