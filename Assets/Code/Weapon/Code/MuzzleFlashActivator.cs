using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[DisallowMultipleComponent]
public class MuzzleFlashActivator : MonoBehaviour
{
    [SerializeField] private Light _muzzleFlashLight;
    [SerializeField] private List<ParticleSystem> _sparks = new List<ParticleSystem>();

    private IEnumerator _lifeTimeCoroutine;
    private bool _isLifeTimeCoroutineRunning;

    private void Awake()
    {
        InitSparks();
        DisableMuzzleFlash();
        _isLifeTimeCoroutineRunning = false;
    }

    private void InitSparks()
    {
        foreach(ParticleSystem spark in _sparks)
        {
            MainModule sparkMainModule = spark.main;
            sparkMainModule.playOnAwake = false;
            sparkMainModule.loop = false;
        }
    }

    public void Activate(float time)
    {
        _lifeTimeCoroutine = ActivateMuzzleFlash(time);
        StartCoroutine(_lifeTimeCoroutine);
    }

    private IEnumerator ActivateMuzzleFlash(float time)
    {
        _isLifeTimeCoroutineRunning = true;
        EnableMuzzleFlash();
        yield return new WaitForSeconds(time);
        DisableMuzzleFlash();
        _isLifeTimeCoroutineRunning = false;
    }

    private void EnableMuzzleFlash()
    {
        _muzzleFlashLight.enabled = true;

        foreach(ParticleSystem spark in _sparks)
        {
            spark.Play();
        }
    }

    private void DisableMuzzleFlash()
    {
        _muzzleFlashLight.enabled = false;

        foreach (ParticleSystem spark in _sparks)
        {
            spark.Stop();
        }
    }

    public void Reset()
    {
        if(_isLifeTimeCoroutineRunning)
        {
            StopCoroutine(_lifeTimeCoroutine);
        }

        DisableMuzzleFlash();
    }
}
