using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _checkpoints = new();
    [SerializeField] GameObject RoundGibs;
    int currentIndex = 0;
    GameObject roundGib;
    Transform gibTran;
    void Awake()
    {
        roundGib = Instantiate(RoundGibs, _checkpoints[0].transform.position, Quaternion.identity);
        roundGib.GetComponent<SpriteRenderer>().color = _checkpoints[currentIndex].GetComponent<SpriteRenderer>().color;
        gibTran = roundGib.GetComponent<Transform>();
        roundGib.transform.position = new Vector3(gibTran.position.x, gibTran.position.y + 1, 0);
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
                _checkpoints[currentIndex].transform.eulerAngles = Vector3.zero;
                gibTran.position = _checkpoints[i].transform.position;
                gibTran.position = new Vector3(gibTran.position.x, gibTran.position.y + 1, 0);
                currentIndex = i;
                return i;
            }
        }
        return 0;
    }
    void Update()
    {
        _checkpoints[currentIndex].transform.eulerAngles += new Vector3(0, 0, 50 * Time.deltaTime);
        gibTran.RotateAround(_checkpoints[currentIndex].transform.position, Vector3.forward, -70 * Time.deltaTime); 
    }
}
