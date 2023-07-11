using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Bullet : MonoBehaviour
{
    private Rigidbody rb;
    public float timer;
    private float ticker = 0;
    private Vector3 bulletDirection;
    private Vector3 LastPos;
    private float speed;
    private Transform target;
    private float weaponType;
    private bool onlyOnce = true;
    [SerializeField]
    private LayerMask ignoreLayer;

    //Wwise 
    [Header("Wwise Events")]
    public AK.Wwise.Event ProjectileFlight;
    public AK.Wwise.Event ProjectileHitObstacle;


    void Start()
    {
        LastPos = transform.position;
        rb = this.GetComponent<Rigidbody>();
        ProjectileFlight.Post(gameObject);
    }
    void Update()
    {
        ticker += Time.deltaTime;
        transform.parent = null;
        if(onlyOnce)
        {
            this.transform.rotation = Quaternion.identity;
            DirMath();
            onlyOnce = false;
        }
        if(ticker > timer)
        {
            DestroyBullet();
            ticker=0;        
        }
        
    }


    void FixedUpdate()
    {
        Vector3 Bulletvelocity = (bulletDirection) * speed*30 * Time.fixedDeltaTime;
        rb.velocity = Bulletvelocity;
        if (Physics.Linecast(transform.position, LastPos,out RaycastHit _obj))
        {
            if(_obj.transform.tag == "Player")
            {
                _obj.transform.GetComponent<Player_HPSystem>().TakeDamage(1);
                DestroyBullet();
            }
            if(_obj.transform.tag == "obstacle")
            {
                DestroyBullet();
            }
        }
        LastPos = transform.position;

    }


    

    void DestroyBullet(){

        Destroy(gameObject);

    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            other.transform.parent.GetComponent<Player_HPSystem>().TakeDamage(1);
            DestroyBullet();
        }
        if(other.tag == "obstacle")
        {
            DestroyBullet();
            ProjectileHitObstacle.Post(gameObject);
        }
        
    }

    public void RecieveBulletParameter(Vector3 _shootDir, float _muzzleV,Transform _target,float _weaponType)
    {
        speed = _muzzleV;
        target = _target;
        weaponType = _weaponType;
        if(_weaponType == 0)
        {
            bulletDirection = _shootDir;
        }   
    }

    public void DirMath()
    {
        if(weaponType == 1)
        {
            Vector3 ShootVector = (target.position - this.transform.position).normalized;
            ShootVector.x = ShootVector.x+Random.Range(-100f,100f)/2000;
            ShootVector.y = ShootVector.y+Random.Range(-100f,100f)/2000;
            bulletDirection = ShootVector;
        }
        if(weaponType == 3)
        {
            Vector3 ShootVector = (target.position - this.transform.position).normalized;
            ShootVector.x = ShootVector.x+Random.Range(-300f,300f)/2000;
            ShootVector.y = ShootVector.y+Random.Range(-300f,300f)/2000;
            bulletDirection = ShootVector;
        }
    }
}

