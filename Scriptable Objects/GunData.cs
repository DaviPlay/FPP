using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject, IData
{
    [Header("Info")]
    public new string name;
    [Range(0, 100)]
    [Tooltip("In percentage")]
    public float weight;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;
    public bool isAuto;

    [Header("Reloading")]
    public int magAmmo;
    public int magSize;
    public int reserveAmmo;
    public int reserveSize;
    [Tooltip("Rounds Per Minute")]
    public float fireRate;
    [Tooltip("In Seconds")]
    public float reloadTime;
    [HideInInspector]
    public bool reloading;
    [HideInInspector] public bool inspecting;

    public string Name => name;
    public float Weight => weight;
    public float MagAmmo => magAmmo;
    public float ReserveAmmo => reserveAmmo;
}
