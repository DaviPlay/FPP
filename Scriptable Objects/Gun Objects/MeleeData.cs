using UnityEngine;

[CreateAssetMenu(fileName = "Melee", menuName = "Weapon/Melee")]
public class MeleeData : ScriptableObject, IWeaponData
{
    [Header("Info")]
    [SerializeField] private new string name;
    [Range(0, 100)]
    [Tooltip("In percentage")]
    [SerializeField] private float weight;

    [Header("Attacking")]
    [SerializeField] private float damage;
    [SerializeField] private float maxDistance;
    [Tooltip("Attacks Per Minute")]
    [SerializeField] private float fireRate;
    [Tooltip("How many milliseconds after the key is pressed for the attack to happen\n\nSync with animation")]
    [SerializeField] private float attackDelay;

    public string Name => name;
    public float Weight => weight;
    public float Damage => damage;
    public float MaxDistance => maxDistance;
    public float FireRate => fireRate;
    public float AttackDelay => attackDelay;
    public bool Reloading => false;
    public uint MagSize => 0;
    public uint MagAmmo { get => 0; set {} }
    public uint Ammo { get => 0; set {} }
    public AmmoType AmmoType => AmmoType.None;
    
    public bool Inspecting { get; set; }
    public bool Attacking { get; set; }
}
