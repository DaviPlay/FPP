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

    private bool CanShoot() => !data.reloading && !MenuFunctions.isGamePaused && 
        gameObject.activeSelf && timeSinceLastShot > 1 / (data.fireRate / 60);

    private void Start()
    {
        eyes = Camera.main.transform;

        data.magAmmo = data.magSize;
        data.reserveAmmo = data.reserveSize;
        data.reloading = false;
        data.inspecting = false;

        if (data.isAuto)
            Shooting.autoShootInput += Shoot;
        else
            Shooting.semiShootInput += Shoot;

        Shooting.reloadInput += StartReload;
        Shooting.inspectInput += StartInspect;

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        anim.SetBool("Walking", player.GetComponent<PlayerMovement>().isWalking);
        anim.SetBool("Sprinting", player.GetComponent<PlayerMovement>().isSprinting);
    }

    public void Shoot()
    {
        if (data.magAmmo > 0 && CanShoot())
        {
            if (Physics.Raycast(eyes.position, eyes.forward, out hit, data.maxDistance, enemyMask))
            {
                IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                damageable?.Damage(data.damage);

                GameObject go = Instantiate(blood, hit.point, Quaternion.identity, hit.transform);
                Destroy(go, bloodDuration);
            }

            data.magAmmo--;
            timeSinceLastShot = 0;
            if (gameObject.activeSelf)
                shoot = StartCoroutine(OnGunShot());
        }
        else if (data.magAmmo <= 0 && MenuFunctions.AutoReload)
        {
            StartReload();
        }
    }

    private IEnumerator OnGunShot()
    {
        anim.SetBool("Shot", true);

        yield return new WaitForEndOfFrame();

        anim.ResetTrigger("Shot");
    }

    public void StartReload()
    {
        if (MenuFunctions.isGamePaused) return;
        if (!gameObject.activeSelf) return;
        if (data.magAmmo == data.magSize) return;
        if (data.reserveAmmo <= 0) return;
        if (data.reloading) return;
        
        reload = StartCoroutine(Reload());
        anim.SetBool("Reloading", true);
    }

    private IEnumerator Reload()
    {
        data.reloading = true;

        yield return new WaitForSeconds(data.reloadTime);

        if (data.reserveAmmo == 0) yield return null;

        if (data.magAmmo + data.reserveAmmo < data.magSize)
        {
            data.magAmmo += data.reserveAmmo;
            data.reserveAmmo = 0;
        }
        else
        {
            data.reserveAmmo -= (data.magSize - data.magAmmo);
            data.magAmmo = data.magSize;
        }

        if (data.reserveAmmo < 0) data.reserveAmmo = 0;
        if (data.magAmmo < 0) data.magAmmo = 0;

        anim.ResetTrigger("Reloading");
        data.reloading = false;
    }

    public void StartInspect()
    {
        if (MenuFunctions.isGamePaused) return;
        if (!gameObject.activeSelf) return;
        if (data.reloading) return;

        inspect = StartCoroutine(Inspect());
    }

    public IEnumerator Inspect()
    {
        data.inspecting = true;
        anim.SetBool("Inspected", true);

        yield return new WaitForEndOfFrame();

        anim.ResetTrigger("Inspected");
        data.inspecting = false;
    }

    public IData GetData()
    {
        return data;
    }
}
