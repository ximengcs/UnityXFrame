using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using XFrame.Modules.Diagnotics;

public class TestCreate : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject Prefab2;

    void Start()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        GameObject.Instantiate(Prefab);
        sw.Stop();
        UnityEngine.Debug.LogWarning(sw.ElapsedMilliseconds);
    }

    void Update()
    {
        
    }
}
