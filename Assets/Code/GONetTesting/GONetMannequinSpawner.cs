using GONet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GONetMannequinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject mannequin;
    public Transform wayPoint1;
    public Transform wayPoint2;
    MannequinTargetController spawnedMannequin;

    public void SpawnMannequin()
    {
        GameObject mannequinSpawned = UnityEngine.Object.Instantiate(mannequin);
        //spawnedMannequin = mannequin.gameObject.GetComponent<MannequinTargetController>();
        //spawnedMannequin._waypoint1 = wayPoint1;
        //spawnedMannequin._waypoint2 = wayPoint2;
    }

    private void FixedUpdate()
    {
        if(spawnedMannequin == null)
        {
            return;
        }
        //spawnedMannequin.Simulate(Time.fixedDeltaTime, Time.time);
    }
}
