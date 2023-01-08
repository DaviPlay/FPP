using System;
using Interfaces;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GunInfo : MonoBehaviour
    {
        public TMP_Text gunName;
        public TMP_Text magAmmo;
        public TMP_Text ammo;
        
        private void Start()
        {
            Shooting.UpdateText += UpdateInfo;

            try
            {
                IWeaponData weaponData = Shooting.GetData();

                gunName.text = weaponData.Name;
                magAmmo.text = weaponData.MagAmmo.ToString();
                ammo.text = weaponData.Ammo.ToString();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void UpdateInfo()
        {
            IWeaponData weaponData = Shooting.GetData();

            gunName.text = weaponData.Name;
            magAmmo.text = weaponData.MagAmmo.ToString();
            ammo.text = weaponData.Ammo.ToString();
        }
    }
}
