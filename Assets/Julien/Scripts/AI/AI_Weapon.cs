using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Weapon : MonoBehaviour
{
    public enum Weapons{ AR, Sniper, Shotgun }
    public Weapons Weapon;
    [HideInInspector]public float MagSize;
    [HideInInspector]public float StartMagSize;

    //WeaponStats
    //AR
    public GameObject bullet;
    private AudioSource audioSource;
    public GameObject MuzzleFlashEffect;
    [SerializeField]private Transform Gunpoint;
    private string enemyTag = "player";
    private float startShootTimerAR = 0.1f;
    private float shootTimerAR;
    private float muzzleVAR = 50;
    private float magSizeAR = 20;
    
    private float magSizeSniper = 20;
    public AudioClip ARSound;

    //Shotgun
    private float startShootTimerShotgun = 1f;
    private float shootTimerShotgun;
    private float muzzleVShotgun = 50;
    private float magSizeShotgun = 3;
    private float bulletAmountShotgun = 10;
    public AudioClip ShotgunSound;
    
    void Start()
    {
        if(Weapons.AR == Weapon)
        {
            MagSize = magSizeAR;
        }
        if(Weapons.Sniper == Weapon)
        {
            MagSize = magSizeSniper;
        }
        if(Weapons.Shotgun == Weapon)
        {
            MagSize = magSizeShotgun;
        }
        StartMagSize = MagSize;
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot(Transform _target)
    {
        if(Weapons.AR == Weapon)
        {
            ShootAR(_target);
        }
        if(Weapons.Sniper == Weapon)
        {
            Debug.Log("Sniper fire");
        }
        if(Weapons.Shotgun == Weapon)
        {
            ShootShotgun(_target);
        }
        shootTimerAR -= Time.deltaTime;
        shootTimerShotgun -= Time.deltaTime;
    }

    private void ShootAR(Transform _target)
    {
        if(shootTimerAR < 0)
        {   
            audioSource.PlayOneShot(ARSound);
            var Bullet = Instantiate(bullet) as GameObject;
            Bullet.transform.SetParent(Gunpoint);
            Bullet.transform.localPosition = new Vector3(0,0,0);
            Bullet.transform.localRotation = Quaternion.identity;
            
            var muzzleFlashEffectIstance = Instantiate(MuzzleFlashEffect) as GameObject;
            muzzleFlashEffectIstance.transform.SetParent(Gunpoint);
            muzzleFlashEffectIstance.transform.localPosition = new Vector3(0,0,0);
            muzzleFlashEffectIstance.transform.localRotation = Quaternion.identity;
            Destroy(muzzleFlashEffectIstance, 4);

            Bullet.GetComponent<AI_Bullet>().RecieveBulletParameter(Vector3.zero,muzzleVAR,_target,1);
            shootTimerAR = startShootTimerAR;
            MagSize -= 1;
        }
    }
    private void ShootShotgun(Transform _target)
    {
        if(shootTimerShotgun < 0)
        {
            audioSource.PlayOneShot(ShotgunSound);

            var muzzleFlashEffectIstance = Instantiate(MuzzleFlashEffect) as GameObject;
            muzzleFlashEffectIstance.transform.SetParent(Gunpoint);
            muzzleFlashEffectIstance.transform.localPosition = new Vector3(0,0,0);
            muzzleFlashEffectIstance.transform.localRotation = Quaternion.identity;
            Destroy(muzzleFlashEffectIstance, 4);

            for(int i = 0; i < bulletAmountShotgun; i++)
            {
                var Bullet = Instantiate(bullet) as GameObject;
                Bullet.transform.SetParent(Gunpoint);
                Bullet.transform.localPosition = new Vector3(0,0,0);
                Bullet.transform.localRotation = Quaternion.identity;
                
                Bullet.GetComponent<AI_Bullet>().RecieveBulletParameter(Vector3.zero,muzzleVShotgun,_target,3);
            }
            shootTimerShotgun = startShootTimerShotgun;
            MagSize -= 1;
        }
    }
}
