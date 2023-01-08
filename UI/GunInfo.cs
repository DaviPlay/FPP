using System;
using Gun_Stuff;
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
                magAmmo.text = weaponData.MagAmmo + "   /";
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

            if (weaponData._AmmoType == AmmoType.None)
            {
                magAmmo.gameObject.SetActive(false);
                ammo.gameObject.SetActive(false);
                
                gunName.text = weaponData.Name;
            }
            else
            {
                magAmmo.gameObject.SetActive(true);
                ammo.gameObject.SetActive(true);

                gunName.text = weaponData.Name;
                magAmmo.text =  weaponData.MagAmmo + "  /";
                ammo.text = weaponData.Ammo.ToString();
            }
        }
    }
}
