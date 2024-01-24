using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Bullet Hole/Bullet Hole Storage", fileName = "BulletHoleTypeStorageData")]
public class BulletHoleTypeStorageSO : ScriptableObject
{
    [SerializeField] private List<BulletImpactType> _bulletHoleTypes;

    IReadOnlyList<string> _typeNames;
    public IReadOnlyList<string> TypeNames => _typeNames ?? (_typeNames = _bulletHoleTypes.Select(x => x.Type).ToList());

    private IDictionary<string, GameObject> _bulletHoleNameToPrefab;

    public void Init()
    {
        _bulletHoleNameToPrefab = new Dictionary<string, GameObject>();

        foreach (BulletImpactType type in _bulletHoleTypes)
        {
            _bulletHoleNameToPrefab.Add(type.Type, type.BulletImpactPrefab);
        }
    }

    public GameObject GetBulletHolePrefabByName(string name)
    {
        GameObject desiredBulletHolePrefab;
        bool succesfullyFound = _bulletHoleNameToPrefab.TryGetValue(name, out desiredBulletHolePrefab);

#if UNITY_EDITOR
        if(!succesfullyFound)
        {
            Debug.LogWarning($"[BulletHoleTypeStorageSO at GetBulletHolePrefabByName]: The bullet hole with name {name} could not be found.");
        }
#endif

        return desiredBulletHolePrefab;
    }
}
