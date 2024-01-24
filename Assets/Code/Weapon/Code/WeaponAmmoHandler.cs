using System;

public class WeaponAmmoHandler
{
    public event Action OnReload;

    private readonly int _maxAmmo;
    private int _ammoLeft;
    private readonly int _clipSize;
    private int _clipAmmoLeft;

    public int AmmoLeft => _ammoLeft;
    public int ClipAmmoLeft => _clipAmmoLeft;

    public void SetAmmoLeft(int newValue)
    {
        _ammoLeft = newValue;
    }

    public void SetClipAmmoLeft(int newValue)
    {
        _clipAmmoLeft = newValue;
    }

    public WeaponAmmoHandler(int maxAmmo, int clipSize, int initialAmmo)
    {
        _maxAmmo = maxAmmo;
        _clipSize = clipSize;

        _ammoLeft = initialAmmo;
        _clipAmmoLeft = 0;

        TryReloadWithoutInvokingEvent();
    }

    public bool HasAmmoInClip()
    {
        return _clipAmmoLeft > 0;
    }

    private void TryReloadWithoutInvokingEvent()
    {
        if (!CanReload())
        {
            return;
        }

        Reload();
    }

    private void Reload()
    {
        int bulletsToReload = CalculateBulletsToReload();
        UpdateAmmo(bulletsToReload);
    }

    public bool IsClipFull()
    {
        return _clipAmmoLeft == _clipSize;
    }

    public bool IsClipEmpty()
    {
        return _clipAmmoLeft == 0;
    }

    public void TryReload()
    {
        if(!CanReload())
        {
            return;
        }

        Reload();
        
        OnReload?.Invoke();
    }

    public bool CanReload()
    {
        return _ammoLeft > 0;
    }

    private int CalculateBulletsToReload()
    {
        int bulletsNeededToFillUpClip = CalculateBulletsNeededToFillClip();

        if (bulletsNeededToFillUpClip <= _ammoLeft)
        {
            return bulletsNeededToFillUpClip;
        }
        else
        {
            return _ammoLeft;
        }
    }

    private int CalculateBulletsNeededToFillClip()
    {
        return _clipSize - _clipAmmoLeft;
    }

    private void UpdateAmmo(int bulletsToReload)
    {
        _ammoLeft -= bulletsToReload;
        _clipAmmoLeft += bulletsToReload;
    }

    public void SubstractBullet()
    {
        if(_clipAmmoLeft == 0)
        {
            return;
        }

        _clipAmmoLeft--;
    }
}
