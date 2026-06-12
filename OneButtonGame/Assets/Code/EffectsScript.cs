using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class EffectsScript : MonoBehaviour
{
    [SerializeField] GameObject RoughGibs;
    public void Death(GameObject exploder)
    {
        Color color = exploder.GetComponent<SpriteRenderer>().color;
        GameObject gib = Instantiate(RoughGibs, exploder.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        gib.GetComponent<SpriteShapeRenderer>().color = color;
        gib.GetComponent<Rigidbody2D>().AddForce(transform.forward * 50, ForceMode2D.Impulse);
    }

}
