using System;
using System.Collections;
using Interfaces;
using Player;
using Scriptable_Objects.Gun_Objects;
using UI;
using UnityEngine;

namespace Gun_Stuff
{
    public class Gun : MonoBehaviour, IWeapon
    {
        [Header("References")]
        [SerializeField] private GunData data;
        private Transform _shooter;
        private LayerMask _enemyMask;
        private Transform _eyes;

        [Header("Blood")] private const float BloodDuration = 1;

        private Animator _anim;
        private float _timeSinceLastShot;

        private Coroutine _shoot;
        private Coroutine _reload;
        private Coroutine _inspect; 
    
        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int Sprinting = Animator.StringToHash("Sprinting");
        private static readonly int Shot = Animator.StringToHash("Shot");
        private static readonly int Reloading = Animator.StringToHash("Reloading");
        private static readonly int Inspected = Animator.StringToHash("Inspected");

        private bool CanShoot() => data.Reloading == false && data.MagAmmo > 0 && !MenuFunctions.IsGamePaused && 
                                   gameObject.activeSelf && _timeSinceLastShot > 1 / (data.FireRate / 60);
    
        private void Start()
        {
            _shooter = Shooting.GetShooter();
        
            _enemyMask = LayerMask.GetMask("Enemy");
            _eyes = Camera.main!.transform;

            data.Inspecting = false;
            data.Reloading = false;
            data.Ammo = data._AmmoType.GetMaxAmmo();
            data.MagAmmo = data.MagSize;

            if (data.IsAuto)
                Shooting.AutoShootInput += Shoot;
            else
                Shooting.SemiShootInput += Shoot;

            Shooting.ReloadInput += StartReload;
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
            _timeSinceLastShot += Time.deltaTime;

            if (_anim == null) return;
        
            _anim.SetBool(Walking, _shooter.GetComponent<PlayerMovement>().isWalking);
            _anim.SetBool(Sprinting, _shooter.GetComponent<PlayerMovement>().isSprinting);
        }

        private void Shoot()
        {
            if (!CanShoot()) return;
        
            if (Physics.Raycast(_eyes.position, _eyes.forward, out RaycastHit hit, data.MaxDistance, _enemyMask))
            {
                IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                damageable?.Damage(data.Damage);

                ParticleSystem ps = Instantiate(Shooting.BloodParticles, hit.point, Quaternion.identity, hit.transform);
                Destroy(ps, BloodDuration);
            }
            
            _timeSinceLastShot = 0;
            
            if (data._AmmoType == AmmoType.None) return;
            
            data.MagAmmo--;
            if (_anim != null)
                _shoot = StartCoroutine(OnGunShot());
        }

        private IEnumerator OnGunShot()
        {
            _anim.SetBool(Shot, true);

            yield return new WaitForEndOfFrame();

            _anim.ResetTrigger(Shot);
        }

        private void StartReload()
        {
            if (MenuFunctions.IsGamePaused || !gameObject.activeSelf || data._AmmoType == AmmoType.None) return;
            if (data.MagAmmo == data.MagSize) return;

            _reload = StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            data.Reloading = true;
        
            if (_anim != null)
                _anim.SetBool(Reloading, true);
            
            yield return new WaitForSeconds(data.ReloadTime);
        
            //Math
            if (data.Ammo == 0) yield break;
        
            if (data.MagAmmo + data.Ammo < data.MagSize)
            {
                data.MagAmmo += data.Ammo;
                data.Ammo = 0;
            }
            else
            {
                data.Ammo -= data.MagSize - data.MagAmmo;
                data.MagAmmo = data.MagSize;
            }
        
            Shooting.UpdateText?.Invoke();
        
            if (_anim != null)
                _anim.ResetTrigger(Reloading);
        
            data.Reloading = false;
        }

        private void StartInspect()
        {
            if (MenuFunctions.IsGamePaused || !gameObject.activeSelf) return;

            _inspect = StartCoroutine(Inspect());
        }

        private IEnumerator Inspect()
        {
            data.Inspecting = true;
        
            if (_anim != null)
                _anim.SetBool(Inspected, true);

            yield return new WaitForEndOfFrame();
        
            if (_anim != null)
                _anim.ResetTrigger(Inspected);
        
            data.Inspecting = false;
        }

        public IWeaponData GetData() => data;
    }
}
