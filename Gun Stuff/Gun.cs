using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{
    [Header("References")]
    [SerializeField] GunData data;
    [SerializeField] Transform player;
    [SerializeField] LayerMask enemyMask;
    private Transform eyes;

    [Header("Blood")]
    public GameObject blood;
    private const float bloodDuration = 1;

    private Animator anim;
    private float timeSinceLastShot;
    private RaycastHit hit;

    Coroutine shoot;
    Coroutine reload;
    Coroutine inspect;

    private bool CanShoot() => !MenuFunctions.isGamePaused && 
        gameObject.activeSelf && timeSinceLastShot > 1 / (data.fireRate / 60);

    private void Start()
    {
        eyes = Camera.main.transform;

        data.inspecting = false;
        data.ammo = data.ammoType.GetMaxAmmo();

        if (data.isAuto)
            Shooting.autoShootInput += Shoot;
        else
            Shooting.semiShootInput += Shoot;

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
        timeSinceLastShot += Time.deltaTime;

        if (anim != null)
        {
            anim.SetBool("Walking", player.GetComponent<PlayerMovement>().isWalking);
            anim.SetBool("Sprinting", player.GetComponent<PlayerMovement>().isSprinting);
        }
    }

    public void Shoot()
    {
        if (data.ammo > 0 && CanShoot())
        {
            if (Physics.Raycast(eyes.position, eyes.forward, out hit, data.maxDistance, enemyMask))
            {
                IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                damageable?.Damage(data.damage);

                GameObject go = Instantiate(blood, hit.point, Quaternion.identity, hit.transform);
                Destroy(go, bloodDuration);
            }

            data.ammo--;
            timeSinceLastShot = 0;
            shoot = StartCoroutine(OnGunShot());
        }
    }

    private IEnumerator OnGunShot()
    {
        if (anim != null)
        {
            anim.SetBool("Shot", true);

            yield return new WaitForEndOfFrame();

            anim.ResetTrigger("Shot");
        }
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
        if (anim != null)
        {
            anim.SetBool("Inspected", true);

            yield return new WaitForEndOfFrame();

            anim.ResetTrigger("Inspected");
            data.inspecting = false;
        }
    }

    public IData GetData()
    {
        return data;
    }
}
