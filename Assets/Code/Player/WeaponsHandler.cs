using GONet;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsHandler
{
    public event Action<WeaponComponent> OnWeaponEquiped;
    public event Action OnShotPerformed;
    public event Action OnStartReload;
    public event Action OnEndReload;

    private readonly Transform _weaponsHolderTransform;
    private List<WeaponComponent> _weapons;
    private WeaponComponent _activeWeapon;

    public float ActiveWeaponTimeSinceLastShot => _activeWeapon.TimeSinceLastShot;
    public int ActiveWeaponClipAmmoLeft => _activeWeapon.ClipAmmoLeft;
    public int ActiveWeaponAmmoLeft => _activeWeapon.AmmoLeft;
    public float ActiveWeaponReloadTimeLeft => _activeWeapon.ReloadTimeLeft;

    public WeaponsHandler(Transform weaponsHolderTransform, Transform shotPointTransform, WeaponComponentsEnableConfiguration weaponEnableConfiguration, RaycastShooter raycastShooter, 
                          GONetParticipant gnp, WeaponComponent[] initialWeapons)
    {
        _weapons = new List<WeaponComponent>();

        _weaponsHolderTransform = weaponsHolderTransform;

        for (int i = 0; i < initialWeapons.Length; ++i)
        {
            _weapons.Add(GameObject.Instantiate(initialWeapons[i], _weaponsHolderTransform));
        }

        foreach(WeaponComponent weaponController in _weapons)
        {
            weaponController.Initialize(shotPointTransform, weaponEnableConfiguration, raycastShooter, gnp);
            weaponController.gameObject.SetActive(false);
        }
    }

    public void SetActiveWeaponTimeSinceLastShot(float newTime)
    {
        _activeWeapon.SetTimeSinceLastShot(newTime);
    }

    public void SetActiveWeaponClipAmmoLeft(int newValue)
    {
        _activeWeapon.SetClipAmmoLeft(newValue);
    }

    public void SetActiveWeaponAmmoLeft(int newValue)
    {
        _activeWeapon.SetAmmoLeft(newValue);
    }

    public void SetActiveWeaponReloadTimeLeft(float newValue)
    {
        _activeWeapon.SetReloadTimeLeft(newValue);
    }

    public void SetActiveWeaponIsBeingReloaded(bool newValue)
    {
        _activeWeapon.SetIsBeingReloaded(newValue);
    }

    private void InvokeStartReload()
    {
        OnStartReload?.Invoke();
    }

    private void InvokeEndReload()
    {
        OnEndReload?.Invoke();
    }

    public void Start()
    {
        if (_weapons.Count > 0)
        {
            EquipWeapon(_weapons[0]);
        }
    }

    public void EquipWeapon(WeaponComponent weaponToEquip)
    {
        SetActiveWeapon(weaponToEquip);
        OnWeaponEquiped?.Invoke(_activeWeapon);
    }

    private void SetActiveWeapon(WeaponComponent weaponToBeActive)
    {
        if(_activeWeapon != null)
        {
            DeactivateWeapon(_activeWeapon);
        }

        _activeWeapon = weaponToBeActive;

        ActivateWeapon(_activeWeapon);
    }

    private void ActivateWeapon(WeaponComponent weaponToActivate)
    {
        weaponToActivate.Enable();
        weaponToActivate.OnStartReload += InvokeStartReload;
        weaponToActivate.OnEndReload += InvokeEndReload;
        weaponToActivate.gameObject.SetActive(true);
    }

    private void DeactivateWeapon(WeaponComponent weaponToDeactivate)
    {
        weaponToDeactivate.Disable();
        weaponToDeactivate.OnStartReload -= InvokeStartReload;
        weaponToDeactivate.OnEndReload -= InvokeEndReload;
        weaponToDeactivate.gameObject.SetActive(false);
    }

    public void Shoot()
    {
        if (_activeWeapon.CanShot())
        {
            _activeWeapon.Shoot();
            OnShotPerformed?.Invoke();
        }
    }

    /// <summary>
    /// No check if can shoot and no <see cref="OnShotPerformed"/> fired.
    /// </summary>
    public void Server_Shoot()
    {
        _activeWeapon.Shoot();
    }

    public void Reload()
    {
        if (_activeWeapon.CanReload())
        {
            _activeWeapon.Reload();
        }
    }

    public void UpdateActiveWeapon(float elapsedTime)
    {
        _activeWeapon.UpdateWeapon(elapsedTime);
    }
}
