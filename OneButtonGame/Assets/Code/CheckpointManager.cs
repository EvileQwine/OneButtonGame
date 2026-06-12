using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _checkpoints = new();
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
                return i;
            }
        }
        return 0;
    }
}
