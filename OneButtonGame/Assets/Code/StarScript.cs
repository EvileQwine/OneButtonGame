using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StarScript : MonoBehaviour
{
    [SerializeField] int spinAmount = -10;
    [SerializeField] int range = 30;
    [SerializeField] float waitTime = 0.3f;
    [SerializeField] GameObject RoundGib;
    List<GameObject> RoundGibList = new();
    GameObject Player;
    ScoreScript scoreScript;
    public bool inRange = false;
    bool canShoot = true;

    void Awake()
    {
        Player = FindFirstObjectByType<PlayerScript>().gameObject;
        scoreScript = FindFirstObjectByType<ScoreScript>();
    }
    void Update()
    {
        if (Vector2.Distance(Player.transform.position, transform.position) < range)
        {
            inRange = true;   
        }
        else
        {
            inRange = false;
        }
        gameObject.transform.eulerAngles += new Vector3(0, 0, spinAmount * Time.deltaTime);
        if (inRange && canShoot && scoreScript.active)
        {
            StartCoroutine(Shoot());
            GameObject gib = Instantiate(RoundGib, transform.position, Quaternion.identity);
            RoundGibList.Add(gib);
            gib.AddComponent<CircleCollider2D>();
            gib.AddComponent<Rigidbody2D>();
            gib.GetComponent<Rigidbody2D>().gravityScale = 0;
            gib.GetComponent<CircleCollider2D>().isTrigger = true;
            gib.GetComponent<SpriteRenderer>().color = GetComponentInChildren<SpriteRenderer>().color;
            gib.gameObject.tag = "Danger";
            gib.GetComponent<Rigidbody2D>().AddForce((Player.transform.position - transform.position).normalized * 20, ForceMode2D.Impulse);
            StartCoroutine(DeleteObject(gib));
        }
        if (!scoreScript.active)
        {
            foreach (GameObject thing in RoundGibList)
            {
                Destroy(thing);
            }
        }
    }
    IEnumerator Shoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(waitTime);
        canShoot = true;
    }
    IEnumerator DeleteObject(GameObject thing)
    {
        yield return new WaitForSeconds(3);
        Destroy(thing);
    }
}
