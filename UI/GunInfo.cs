using UnityEngine;
using UnityEngine.UI;

public class GunInfo : MonoBehaviour
{
    public Text gunName;
    public Text magazine;
    public Text reserve;

    private void Start()
    {
        Shooting.updateText += UpdateText;
    }

    private void UpdateText()
    {
        //Getting the active gun's data
        IData gunData = Shooting.GetData();

        gunName.text = gunData.Name;
        magazine.text = Mathf.Max(gunData.MagAmmo, 0).ToString();
        reserve.text = Mathf.Max(gunData.ReserveAmmo, 0).ToString();
    }
}
