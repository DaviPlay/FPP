using System;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private int _selectedWeapon;

    public static Action SemiShootInput;
    public static Action AutoShootInput;
    public static Action MeleeAttackInput;
    public static Action ReloadInput;
    public static Action InspectInput;
    public static Action WeaponSwitchInput;

    public static Action UpdateText;

    private static IWeapon _weapon;
    private static IWeaponData _weaponData;

    private void Start()
    {
        foreach (Transform gunContainer in transform)
            foreach (Transform gunParent in gunContainer)
                foreach (Transform gun in gunParent)
                    if (gun.gameObject.activeSelf)
                    {
                        _weapon = gun.gameObject.GetComponent<Gun>() != null ? gun.gameObject.GetComponent<Gun>() : gun.gameObject.GetComponent<Melee>();
                        _weaponData = _weapon is Gun ? gun.gameObject.GetComponent<Gun>().GetData() : gun.gameObject.GetComponent<Melee>().GetData();
                    }

        UpdateText?.Invoke();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
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
                    UpdateText?.Invoke();
                }
            }
            else
                MeleeAttackInput?.Invoke();

        if (Input.GetButtonDown("Fire1"))
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

        if (Input.GetButtonDown("Reload"))
        {
            ReloadInput?.Invoke();
            UpdateText?.Invoke();
        }

        if (Input.GetButtonDown("Inspect"))
            InspectInput?.Invoke();

        SwitchWeapon();
    }

    private void SwitchWeapon()
    {
        if (MenuFunctions.IsGamePaused) return;
        int previousWeapon = _selectedWeapon;

        if (Input.GetAxis("Scroll") > 0)
        {
            if (_selectedWeapon >= transform.childCount - 1)
                _selectedWeapon = 0;
            else
                _selectedWeapon++;
        }
        if (Input.GetAxis("Scroll") < 0)
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
            foreach (Transform gunParent in gunContainer)
                foreach (Transform gun in gunParent)
                {
                    if (i == _selectedWeapon)
                    {
                        gun.gameObject.SetActive(true);
                        _weapon = gun.gameObject.GetComponent<Gun>() != null ? gun.gameObject.GetComponent<Gun>() : gun.gameObject.GetComponent<Melee>();
                        _weaponData = _weapon is Gun ? gun.gameObject.GetComponent<Gun>().GetData() : gun.gameObject.GetComponent<Melee>().GetData();
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
