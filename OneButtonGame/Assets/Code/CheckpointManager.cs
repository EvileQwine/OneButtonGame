using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _checkpoints = new();
    [SerializeField] GameObject RoundGibs;
    int currentIndex = 0;
    GameObject[] myGibs = new GameObject[2];
    void Awake()
    {
        for (int i = 0; i < myGibs.Length; i++)
        {
            myGibs[i] = Instantiate(RoundGibs, _checkpoints[0].transform.position, Quaternion.identity);
            myGibs[i].GetComponent<SpriteRenderer>().color = _checkpoints[currentIndex].GetComponent<SpriteRenderer>().color;
        }
        SetGibs(_checkpoints[0].transform.position);
    }
    public Vector2 ReturnCheckpoint(int i)
    {
        return _checkpoints[i].transform.position;
    }
    public int ReturnIndex(GameObject checkpoint)
    {
        for (int i = 0; i < _checkpoints.Count; i++)
        {
            if (checkpoint == _checkpoints[i])
            {
                currentIndex = i;
                return i;
            }
        }
        return 0;
    }
    public void SetGibs(Vector3 pos)
    {
        myGibs[0].transform.position = new Vector3(pos.x, pos.y + 1, pos.z);
        myGibs[1].transform.position = new Vector3(pos.x, pos.y - 1, pos.z);
    }
    public void UpdateRotation(int i)
    {
        _checkpoints[currentIndex].transform.eulerAngles = Vector3.zero;
        SetGibs(_checkpoints[i].transform.position);
    }
    public void SetRotation(int i)
    {
        _checkpoints[i].transform.rotation = Quaternion.identity;
    }
    void Update()
    {
        _checkpoints[currentIndex].transform.eulerAngles += new Vector3(0, 0, 50 * Time.deltaTime);
        for (int i = 0; i < myGibs.Length; i++)
        {
            myGibs[i].transform.RotateAround(_checkpoints[currentIndex].transform.position, Vector3.forward, -70 * Time.deltaTime);
        }
    }
}
