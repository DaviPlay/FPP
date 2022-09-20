using UnityEngine;
using UnityEngine.UI;

public class GunInfo : MonoBehaviour
{
    public Text gunName;
    public Text maxAmmo;
    public Text ammo;

    private void Start()
    {
        Shooting.UpdateText += UpdateText;
        IWeaponData weaponData = Shooting.GetData();

        gunName.text = weaponData.Name;
        maxAmmo.text = weaponData.Ammo.ToString();
        ammo.text = weaponData.MagAmmo.ToString();
    }

    private void UpdateText()
    {
        IWeaponData weaponData = Shooting.GetData();

        gunName.text = weaponData.Name;
        maxAmmo.text = weaponData.Ammo.ToString();
        ammo.text = weaponData.MagAmmo.ToString();
    }
}
