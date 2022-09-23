using System;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private int _selectedWeapon;
    private static IWeaponData _weaponData;
    
    public static Action SemiShootInput;
    public static Action AutoShootInput;
    public static Action MeleeAttackInput;
    public static Action ReloadInput;
    public static Action InspectInput;
    public static Action WeaponSwitchInput;
    public static Action UpdateText;

    private void Start()
    {
        foreach (Transform gunContainer in transform)
            foreach (Transform gun in gunContainer)
                if (gun.gameObject.activeSelf)
                    _weaponData = gun.gameObject.GetComponent<IWeapon>().GetData();

        UpdateText?.Invoke();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
            if (_weaponData is GunData)
            {
                if (_weaponData.MagAmmo > 0)
                {
                    AutoShootInput?.Invoke();
                    UpdateText?.Invoke();
                }
                else if (MenuFunctions.IsAutoReload)
                {
                    ReloadInput?.Invoke();
                }
            }
            else
                MeleeAttackInput?.Invoke();

        if (Input.GetButtonDown("Fire1"))
            if (_weaponData is GunData)
            {
                if (_weaponData.MagAmmo > 0)
                {
                    SemiShootInput?.Invoke();
                    UpdateText?.Invoke();
                }
                else if (MenuFunctions.IsAutoReload)
                {
                    ReloadInput?.Invoke();
                }
            }
            else
                MeleeAttackInput?.Invoke();

        if (Input.GetButtonDown("Reload"))
            ReloadInput?.Invoke();

        if (Input.GetButtonDown("Inspect"))
            InspectInput?.Invoke();

        SwitchWeapon();
    }

    private void SwitchWeapon()
    {
        if (MenuFunctions.IsGamePaused) return;
        
        int previousWeapon = _selectedWeapon;

        //Down
        if (Input.GetAxis("Scroll") < 0)
        {
            if (_selectedWeapon >= transform.childCount - 1)
                _selectedWeapon = 0;
            else
                _selectedWeapon++;
        }
        //Up
        if (Input.GetAxis("Scroll") > 0)
        {
            if (_selectedWeapon <= 0)
                _selectedWeapon = transform.childCount - 1;
            else
                _selectedWeapon--;
        }

        if (previousWeapon != _selectedWeapon)
            SelectWeapon();
    }

    private void SelectWeapon()
    {
        int i = 0;
        foreach (Transform gunContainer in transform)
            foreach (Transform gun in gunContainer)
            {
                if (i == _selectedWeapon)
                {
                    gun.gameObject.SetActive(true);
                    _weaponData = gun.gameObject.GetComponent<IWeapon>().GetData();
                    WeaponSwitchInput?.Invoke();
                }
                else
                    gun.gameObject.SetActive(false);

                i++;
            }

        UpdateText?.Invoke();
    }

    public static IWeaponData GetData()
    {
        return _weaponData;
    }
}
