using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class BulletImpactType
{
    [SerializeField] private string _type;
    [SerializeField] private GameObject _bulletImpactPrefab;
    [SerializeField] private List<string> _bulletImpactSounds;

    public string Type => _type;
    public GameObject BulletImpactPrefab => _bulletImpactPrefab;

    public string GetRandomImpactSound()
    {
        Assert.IsTrue(_bulletImpactSounds.Count > 0, $"[BulletImpactType at GetRandomImpactSound]: There is no sound in the bullet impact of type {_type}");
        int randomIndex = UnityEngine.Random.Range(0, _bulletImpactSounds.Count);
        return _bulletImpactSounds[randomIndex];
    }
}