using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Bullet Impacts/Bullet Impact Storage", fileName = "BulletImpactTypeStorageData")]
public class BulletImpactTypeStorageSO : ScriptableObject
{
    [SerializeField] private List<BulletImpactType> _bulletImpactTypes;
    [SerializeField] private GameObject _bulletImpactSoundPrefab;

    IReadOnlyList<string> _typeNames;
    public IReadOnlyList<string> TypeNames => _typeNames ?? (_typeNames = _bulletImpactTypes.Select(x => x.Type).ToList());

    private IDictionary<string, BulletImpactType> _bulletImpactNameToPrefab;

    public void Init()
    {
        _bulletImpactNameToPrefab = new Dictionary<string, BulletImpactType>();

        foreach(BulletImpactType type in _bulletImpactTypes)
        {
            _bulletImpactNameToPrefab.Add(type.Type, type);
        }
    }

    public GameObject GetBulletImpactSoundPrefab()
    {
        return _bulletImpactSoundPrefab;
    }

    public BulletImpactType GetBulletImpactTypeByName(string name)
    {
        BulletImpactType desiredBulletImpactType;
        bool succesfullyFound = _bulletImpactNameToPrefab.TryGetValue(name, out desiredBulletImpactType);

#if UNITY_EDITOR
        if (!succesfullyFound)
        {
            Debug.LogWarning($"[BulletImpactTypeStorageSO at GetBulletImpactPrefabByName]: The bullet impact with name {name} could not be found.");
        }
#endif

        return desiredBulletImpactType;
    }
}
