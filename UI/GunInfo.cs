using UnityEngine;
using UnityEngine.UI;

public class GunInfo : MonoBehaviour
{
    public Text gunName;
    public Text maxAmmo;
    public Text ammo;

    private void Start()
    {
        Shooting.updateText += UpdateText;
    }

    private void UpdateText()
    {
        IData gunData = Shooting.GetData();

        gunName.text = gunData.Name;
        maxAmmo.text = gunData.AmmoType.GetMaxAmmo().ToString();
        ammo.text = gunData.Ammo.ToString();
    }
}
