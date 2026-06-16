using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.Splines.ExtrusionShapes;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class EffectsScript : MonoBehaviour
{
    [SerializeField] GameObject RoughGibs;
    [SerializeField] GameObject RoundGibs;
    [SerializeField] float explosiveForce = 5;

    List<GameObject> attempts = new();
    public void Death(GameObject exploder)
    {
        Color eColor = exploder.GetComponent<SpriteRenderer>().color;
        Color color = new(eColor.r - 0.2f, eColor.g - 0.2f, eColor.b - 0.2f);
        GameObject gib = Instantiate(RoughGibs, exploder.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        gib.GetComponent<SpriteShapeRenderer>().color = color;
        attempts.Add(gib);
        GameObject[] pieces = new GameObject[10];
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i] = Instantiate(RoundGibs, exploder.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            pieces[i].GetComponent<SpriteRenderer>().color = color;
            Rigidbody2D rb = pieces[i].AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.AddForce(pieces[i].transform.right * explosiveForce, ForceMode2D.Impulse);
        }
        StartCoroutine(DestroyDeathParticles(pieces));
    }
    IEnumerator DestroyDeathParticles(GameObject[] pieces)
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < pieces.Length; i++)
        {
            Destroy(pieces[i]);
        }
    }
    public void RemoveAttempts()
    {
        foreach (GameObject attempt in attempts)
        {
            Destroy(attempt);
        }
    }
}
