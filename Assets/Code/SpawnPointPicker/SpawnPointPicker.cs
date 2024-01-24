using UnityEngine;

public class SpawnPointPicker : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;

    /// <summary>
    /// Returns a spawn position and orientation from a collection of possible spawn points
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void GetSpawnPoint(out Vector3 position, out Quaternion rotation)
    {
        int randomNumber = Random.Range(0, _spawnPoints.Length);

        Transform spawnerTransform = _spawnPoints[randomNumber];
        position = spawnerTransform.position;
        rotation = spawnerTransform.rotation;
    }
}
