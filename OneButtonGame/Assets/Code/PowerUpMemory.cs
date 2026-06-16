using UnityEngine;

public enum PowerUps
{
    TimeDelay,
    Invincible,
    Speed,
    Laser,
}
public class PowerUpMemory : MonoBehaviour
{
    public PowerUps powerup;
    public float powerMultiplier;
    public int powerTime;
}
