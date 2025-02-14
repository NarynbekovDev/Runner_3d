using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private PathCreator _pathCreator;

    public PathCreator GetPathCreator => _pathCreator;
    private void Awake()
    {
        _pathCreator = transform.GetComponentInChildren<PathCreator>();
    }
}
