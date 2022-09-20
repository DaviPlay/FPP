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
    
    public string Name => name;
    public float Weight => weight;
    public float Damage => damage;
    public float MaxDistance => maxDistance;
    public bool IsAuto => isAuto;
    public bool Reloading { get; set; }
    public uint MagSize => magSize;
    public uint MagAmmo { get; set; }
    public uint Ammo { get; set; }
    public AmmoType AmmoType => ammoType;
    public float FireRate => fireRate;
    public float ReloadTime => reloadTime;
    public bool Inspecting { get; set; }
}
