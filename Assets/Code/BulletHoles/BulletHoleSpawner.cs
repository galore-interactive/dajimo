using GONet;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletHolePool
{
    public string BulletHoleType { get; private set; }

    int iNext = 0;
    int poolSize;
    readonly GameObject[] instances;

    public BulletHolePool(string bulletHoleType, GameObject prefab, int poolSize, Transform itemParent)
    {
        BulletHoleType = bulletHoleType;
        this.poolSize = poolSize;
        instances = new GameObject[poolSize];
        for (int i = 0; i < poolSize; ++i)
        {
            GameObject instance = instances[i] = Object.Instantiate(prefab, itemParent);
            instance.SetActive(false);
        }
    }

    public void ActivateAt(Vector3 position, Quaternion rotation)
    {
        int index = iNext++ % poolSize; // TODO use the optimized technique of bit shifting and wrap around instead of modulus
        GameObject instance = instances[index];
        instance.transform.SetPositionAndRotation(position, rotation);
        instance.SetActive(true);
    }
}

/// <summary>
/// IMPORTANT: Only a client side concern!  Server will not spawn bullet holes.
/// </summary>
[DisallowMultipleComponent]
public class BulletHoleSpawner : MonoBehaviour
{
    [SerializeField] private BulletHoleTypeStorageSO _bulletHoleTypes;

    private readonly Dictionary<string, BulletHolePool> _bulletHolePoolsByType = new();

    private void Awake()
    {
        _bulletHoleTypes.Init();

        StartCoroutine(FillUpPoolASAP(250));
    }

    private IEnumerator FillUpPoolASAP(int itemsPerLayer)
    {
        while (!GONetMain.IsClientVsServerStatusKnown)
        {
            yield return null;
        }

        if (GONetMain.IsClient) // server does not care about bullet holes
        {
            string[] layers = Enumerable.Range(0, 32).Select(index => LayerMask.LayerToName(index)).Where(l => !string.IsNullOrEmpty(l)).ToArray();
            foreach (string layer in layers)
            {
                ProcessHoleType(layer);
            }

            foreach (var typeName in _bulletHoleTypes.TypeNames)
            {
                ProcessHoleType(typeName);
            }
        }

        void ProcessHoleType(string holeType)
        {
            GameObject bulletHoleToSpawn = GetBulletHolePrefabByName(holeType);
            if (bulletHoleToSpawn != null)
            {
                _bulletHolePoolsByType[holeType] = new BulletHolePool(holeType, bulletHoleToSpawn, 100, transform);
            }
        }
    }

    public void Client_Spawn(ShotHitData hitData)
    {
        if (!GONetMain.IsClient) return;

        if (_bulletHolePoolsByType.TryGetValue(hitData.surfaceMaterialType, out BulletHolePool pool))
        {
            pool.ActivateAt(hitData.position + (hitData.surfaceNormal * 0.001f), Quaternion.LookRotation(hitData.surfaceNormal));
        }
    }

    private GameObject GetBulletHolePrefabByName(string name)
    {
        return _bulletHoleTypes.GetBulletHolePrefabByName(name);
    }
}

