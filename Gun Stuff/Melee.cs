using System;
using System.Collections;
using UnityEngine;

public class Melee : MonoBehaviour, IWeapon
{
    [Header("References")]
    [SerializeField] private MeleeData data;
    private Transform _player;
    private LayerMask _enemyMask;
    private Transform _eyes;

    [Header("Blood")]
    public GameObject blood;
    [SerializeField] private float bloodDuration = 1;

    private Animator _anim;
    private float _timeSinceLastAttack;

    private Coroutine _attack;
    private Coroutine _inspect;
    
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Sprinting = Animator.StringToHash("Sprinting");
    private static readonly int Attacked = Animator.StringToHash("Attacked");
    private static readonly int Inspected = Animator.StringToHash("Inspected");
    private static readonly int InspectIndex = Animator.StringToHash("Inspect Index");

    private bool CanAttack() => !data.Attacking && _timeSinceLastAttack > 1 / (data.FireRate / 60);

    private void Start()
    {
        //Lol again
        _player = transform.parent.parent.parent.parent.GetChild(0);
        
        _enemyMask = LayerMask.GetMask("Enemy");
        _eyes = Camera.main!.transform;

        data.Attacking = false;
        data.Inspecting = false;

        Shooting.MeleeAttackInput += Attack;
        Shooting.InspectInput += StartInspect;
        
        try
        {
            _anim = GetComponent<Animator>();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void Update()
    {
        _timeSinceLastAttack += Time.deltaTime;

        _anim.SetBool(Walking, _player.GetComponent<PlayerMovement>().isWalking);
        _anim.SetBool(Sprinting, _player.GetComponent<PlayerMovement>().isSprinting);
    }

    private void Attack()
    {
        if (MenuFunctions.IsGamePaused || !gameObject.activeSelf) return;
        if (!CanAttack()) return;
        
        _timeSinceLastAttack = 0;
        _attack = StartCoroutine(OnAttack());
    }

    private IEnumerator OnAttack()
    {
        _anim.SetBool(Attacked, true);

        yield return new WaitForSeconds(data.AttackDelay / 1000);

        if (Physics.Raycast(_eyes.position, _eyes.forward, out RaycastHit hit, data.MaxDistance, _enemyMask))
        {
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            damageable?.Damage(data.Damage);

            GameObject go = Instantiate(blood, hit.point, Quaternion.identity, hit.transform);
            Destroy(go, bloodDuration);
        }

        _anim.ResetTrigger(Attacked);
    }

    private void StartInspect()
    {
        if (MenuFunctions.IsGamePaused) return;
        if (!gameObject.activeSelf) return;

        _inspect = StartCoroutine(Inspect());
    }

    private IEnumerator Inspect()
    {
        data.Inspecting = true;
        _anim.SetBool(Inspected, true);
        _anim.SetInteger(InspectIndex, UnityEngine.Random.Range(0, 5));

        yield return new WaitForEndOfFrame();

        _anim.ResetTrigger(Inspected);
        data.Inspecting = false;
    }

    public IWeaponData GetData()
    {
        return data;
    }
}
