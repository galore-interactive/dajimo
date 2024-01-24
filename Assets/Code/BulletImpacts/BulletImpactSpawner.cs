using GONet;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletImpactPool
{
    BulletImpactType bulletImpactTypeSource;
    public BulletImpactType BulletImpactTypeSource => bulletImpactTypeSource;
    public string BulletImpactType { get; private set; }

    int iNext = 0;
    int poolSize;
    readonly (GameObject go_impact, GameObject go_sound, AudioPlayer audioPlayer, ParticleSystem particleSystem)[] instances;

    public BulletImpactPool(string bulletImpactType, BulletImpactType bulletHoleToSpawn, GameObject prefab_sound, int poolSize, Transform itemParent)
    {
        BulletImpactType = bulletImpactType;
        bulletImpactTypeSource = bulletHoleToSpawn;
        this.poolSize = poolSize;
        instances = new (GameObject, GameObject, AudioPlayer, ParticleSystem)[poolSize];
        GameObject prefab_impact = bulletHoleToSpawn.BulletImpactPrefab;
        for (int i = 0; i < poolSize; ++i)
        {
            (GameObject go_impact, GameObject go_sound, AudioPlayer audioPlayer, ParticleSystem particleSystem) instance = new();
            
            instance.go_impact = Object.Instantiate(prefab_impact, itemParent);
            instance.go_impact.SetActive(false);

            instance.go_sound = Object.Instantiate(prefab_sound, itemParent);
            instance.go_sound.SetActive(false);

            instance.audioPlayer = instance.go_sound.GetComponent<AudioPlayer>();
            instance.particleSystem = instance.go_impact.GetComponentInChildren<ParticleSystem>();

            instances[i] = instance;
        }
    }

    public void ActivateAt(Vector3 position, Quaternion rotation)
    {
        int index = iNext++ % poolSize; // TODO use the optimized technique of bit shifting and wrap around instead of modulus
        (GameObject go_impact, GameObject go_sound, AudioPlayer audioPlayer, ParticleSystem particleSystem) instance = instances[index];

        instance.go_impact.transform.SetPositionAndRotation(position, rotation);
        instance.go_impact.SetActive(true);

        instance.go_sound.transform.SetPositionAndRotation(position, rotation);
        instance.go_sound.SetActive(true);

        string sound = bulletImpactTypeSource.GetRandomImpactSound();
        instance.audioPlayer.PlayAudio(sound);

        instance.particleSystem.Play();
    }
}


/// <summary>
/// IMPORTANT: Only a client side concern!  Server will not spawn bullet impacts.
/// </summary>
[DisallowMultipleComponent]
public class BulletImpactSpawner : MonoBehaviour
{
    [SerializeField] private BulletImpactTypeStorageSO _bulletImpactTypes;
    [SerializeField] private Transform _parentHolderTransform;

    private readonly Dictionary<string, BulletImpactPool> _bulletImpactPoolsByType = new();

    private void Awake()
    {
        _bulletImpactTypes.Init();
        
        StartCoroutine(FillUpPoolASAP(250));
    }

    private IEnumerator FillUpPoolASAP(int itemsPerLayer)
    {
        while (!GONetMain.IsClientVsServerStatusKnown)
        {
            yield return null;
        }

        if (GONetMain.IsClient) // server does not care about bullet impacts
        {
            string[] layers = Enumerable.Range(0, 32).Select(index => LayerMask.LayerToName(index)).Where(l => !string.IsNullOrEmpty(l)).ToArray();
            foreach (string layer in layers)
            {
                ProcessImpactType(layer);
            }

            foreach (var typeName in _bulletImpactTypes.TypeNames)
            {
                ProcessImpactType(typeName);
            }
        }

        void ProcessImpactType(string holeType)
        {
            BulletImpactType bulletHoleToSpawn = GetBulletImpactTypeByName(holeType);
            if (bulletHoleToSpawn != null)
            {
                _bulletImpactPoolsByType[holeType] = new BulletImpactPool(holeType, bulletHoleToSpawn, _bulletImpactTypes.GetBulletImpactSoundPrefab(), 100, transform);
            }
        }
    }

    public void Client_Spawn(ShotHitData hitData)
    {
        if (!GONetMain.IsClient) return;

        if (_bulletImpactPoolsByType.TryGetValue(hitData.surfaceMaterialType, out BulletImpactPool pool))
        {
            pool.ActivateAt(hitData.position + (hitData.surfaceNormal * 0.001f), Quaternion.LookRotation(hitData.surfaceNormal));
        }
    }

    private BulletImpactType GetBulletImpactTypeByName(string name)
    {
        return _bulletImpactTypes.GetBulletImpactTypeByName(name);
    }
}
