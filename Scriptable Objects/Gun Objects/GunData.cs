using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject, IWeaponData
{
    [Header("Info")]
    [SerializeField] private new string name;
    [Tooltip("In percentage"), Range(0, 100)]
    [SerializeField] private float weight;

    [Header("Shooting")]
    [SerializeField] private float damage;
    [SerializeField] private float maxDistance;
    [SerializeField] private bool isAuto;
    [SerializeField] private uint magSize;
    [SerializeField] private AmmoType ammoType;
    [Tooltip("Rounds Per Minute")] 
    [SerializeField] private float fireRate;
    [Tooltip("In Seconds")] 
    [SerializeField] private float reloadTime;
    
    public string Name
    {
        get => name;
        set => name = value;
    }

    public float Weight
    {
        get => weight;
        set => weight = value;
    }

    public float Damage
    {
        get => damage;
        set => damage = value;
    }

    public float MaxDistance
    {
        get => maxDistance;
        set => maxDistance = value;
    }

    public bool IsAuto
    {
        get => isAuto;
        set => isAuto = value;
    }

    public bool Reloading { get; set; }
    public uint MagSize
    {
        get => magSize;
        set => magSize = value;
    }

    public uint MagAmmo { get; set; }
    public uint Ammo { get; set; }
    public AmmoType AmmoType => ammoType;

    public float FireRate
    {
        get => fireRate;
        set => fireRate = value;
    }

    public float ReloadTime
    {
        get => reloadTime;
        set => reloadTime = value;
    }

    public bool Inspecting { get; set; }
}
