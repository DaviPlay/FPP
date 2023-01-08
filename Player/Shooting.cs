using System;
using Interfaces;
using Scriptable_Objects.Gun_Objects;
using UI;
using UnityEngine;

namespace Player
{
    public class Shooting : MonoBehaviour
    {
        [SerializeField] private Transform shooter;
        private static Transform _sShooter;
        
        private int _selectedWeapon;
        private static IWeaponData _weaponData;
        [SerializeField] private ParticleSystem bloodEffect;
        public static ParticleSystem BloodParticles;

        public static Action SemiShootInput;
        public static Action AutoShootInput;
        public static Action MeleeAttackInput;
        public static Action ReloadInput;
        public static Action InspectInput;
        public static Action WeaponSwitchInput;
        public static Action UpdateText;

        private void Start()
        {
            BloodParticles = bloodEffect;
            
            _sShooter = shooter;
            
            foreach (Transform gun in transform)
                if (gun.gameObject.activeSelf)
                {
                    _weaponData = gun.GetComponent<IWeapon>().GetData();
                    break;
                }

            UpdateText?.Invoke();
        }

        private void Update()
        {
            if (Input.GetButton("Fire1"))
                if (_weaponData is GunData)
                {
                    switch (_weaponData.MagAmmo)
                    {
                        case > 0:
                            AutoShootInput?.Invoke();
                            UpdateText?.Invoke();
                            break;
                        case <= 0 when MenuFunctions.IsAutoReload && !_weaponData.Reloading:
                            ReloadInput?.Invoke();
                            break;
                    }
                }
                else
                    MeleeAttackInput?.Invoke();

            if (Input.GetButtonDown("Fire1"))
                if (_weaponData is GunData)
                {
                    switch (_weaponData.MagAmmo)
                    {
                        case > 0:
                            SemiShootInput?.Invoke();
                            UpdateText?.Invoke();
                            break;
                        case <= 0 when MenuFunctions.IsAutoReload && !_weaponData.Reloading:
                            ReloadInput?.Invoke();
                            break;
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
        
            var previousWeapon = _selectedWeapon;

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
            var i = 0;
            foreach (Transform gun in transform)
            {
                if (i == _selectedWeapon)
                {
                    gun.gameObject.SetActive(true);
                    _weaponData = gun.GetComponent<IWeapon>().GetData();
                    WeaponSwitchInput?.Invoke();
                }
                else
                    gun.gameObject.SetActive(false);

                i++;
            }

            UpdateText?.Invoke();
        }

        public static Transform GetShooter() => _sShooter;

        public static IWeaponData GetData() => _weaponData;
    }
}
