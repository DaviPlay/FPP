using System;
using System.Collections;
using UnityEngine;

public class Melee : MonoBehaviour, IWeapon
{
    [Header("References")]
    [SerializeField] MeleeData data;
    [SerializeField] Transform player;
    [SerializeField] LayerMask enemyMask;
    private Transform eyes;

    [Header("Blood")]
    public GameObject blood;
    private const float bloodDuration = 1;

    private Animator anim;
    private float timeSinceLastAttack;

    Coroutine attack;
    Coroutine inspect;

    private bool CanAttack() => !data.attacking && timeSinceLastAttack > 1 / (data.fireRate / 60);

    private void Start()
    {
        eyes = Camera.main.transform;

        data.attacking = false;
        data.inspecting = false;

        Shooting.semiShootInput += Attack;
        Shooting.inspectInput += StartInspect;
        
        try
        {
            anim = GetComponent<Animator>();
        }
        catch (Exception)
        {
        }
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        anim.SetBool("Walking", player.GetComponent<PlayerMovement>().isWalking);
        anim.SetBool("Sprinting", player.GetComponent<PlayerMovement>().isSprinting);
    }

    public void Attack()
    {
        if (MenuFunctions.isGamePaused) return;
        if (!gameObject.activeSelf) return;

        if (CanAttack())
        {
            timeSinceLastAttack = 0;
            if (gameObject.activeSelf)
                attack = StartCoroutine(OnAttack());
        }
    }

    private IEnumerator OnAttack()
    {
        anim.SetBool("Attacked", true);

        yield return new WaitForSeconds(data.attackDelay / 1000);

        if (Physics.Raycast(eyes.position, eyes.forward, out RaycastHit hit, data.maxDistance, enemyMask))
        {
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            damageable?.Damage(data.damage);

            GameObject go = Instantiate(blood, hit.point, Quaternion.identity, hit.transform);
            Destroy(go, bloodDuration);
        }

        anim.ResetTrigger("Attacked");
    }

    public void StartInspect()
    {
        if (MenuFunctions.isGamePaused) return;
        if (!gameObject.activeSelf) return;

        inspect = StartCoroutine(Inspect());
    }

    public IEnumerator Inspect()
    {
        data.inspecting = true;
        anim.SetBool("Inspected", true);
        anim.SetInteger("Inspect Index", UnityEngine.Random.Range(0, 5));

        yield return new WaitForEndOfFrame();

        anim.ResetTrigger("Inspected");
        data.inspecting = false;
    }

    public IData GetData()
    {
        return data;
    }
}
