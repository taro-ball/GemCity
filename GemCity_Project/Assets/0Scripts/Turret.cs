using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;
    private float fireCountdown = 0f;

    [Header("General")]
    public float baseRange;
    public float baseDamage;
    public float baseFireRate;

    [Header("Bullets (default)")]
    public GameObject bulletPrefab;

    [Header("Laser")]
    public bool useLaser = false;
    public LineRenderer lineRend;
    public ParticleSystem impactEffect;
    private float damageOverTime;
    public float slowFactor = .5f;

    [Header("Tower Setup")]
    public Transform towerPointer;
    public float angularSpeed = 10f;
    public string enemyTag = "Enemy";
    public Vector3 rotateVector = new Vector3(0f, 77f, 0f);

    [HideInInspector]
    public float effectiveFireRate;
    [HideInInspector]
    public float effectiveRange;
    [HideInInspector]
    public float effectiveDamage;
    //[HideInInspector]
    private AudioSource _audioSource;

    public Node parentNode;

    public Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        //effectiveRange = baseRange * GameManager.globalRange;
        //effectiveFireRate = baseFireRate * GameManager.globalRate;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDinstance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        effectiveRange = baseRange * GameManager.globalRange * parentNode.nodeRange;
        effectiveFireRate = baseFireRate * GameManager.globalRate * parentNode.nodeFireRate;
        effectiveDamage = baseDamage * GameManager.globalDamage * parentNode.nodeDamage;
        foreach (GameObject enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript.isDead)
            {
                //if dead do not traget
                target = null;
                return;
            }

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDinstance)
            {
                shortestDinstance = distanceToEnemy;
                nearestEnemy = enemy;
            }

            if (nearestEnemy != null && shortestDinstance <= effectiveRange)
            {
                target = nearestEnemy.transform;
                targetEnemy = nearestEnemy.GetComponent<Enemy>();
            }
            else
            {
                //Debug.Log("Lost target!");
                target = null;
            }

        }
    }

    void Update()
    {
        if (target == null)
        {
            //erase Laser on target loss
            if (useLaser)
            {
                if (lineRend.enabled)
                {
                    lineRend.enabled = false;
                    impactEffect.Stop();
                    _audioSource.Stop();
                }
            }
            //Debug.Log("No target! Returning!");
            //towerPointer.rotation = Quaternion.Euler(0f, towerPointer.rotation.y+Time.deltaTime*0.01f, 0f);

            towerPointer.Rotate(rotateVector * Time.deltaTime);
            return;
        }

        LockOnTarget();

        if (useLaser)
        {
            Laser();
        }
        else
        {
            if (fireCountdown <= 0)
            {

                Shoot(effectiveDamage);
                fireCountdown = 1f / effectiveFireRate;

            }
            fireCountdown -= Time.deltaTime;
        }

    }
    void Laser()
    {
        damageOverTime = effectiveDamage * effectiveFireRate;
        Debug.Log("damageOverTime: "+ damageOverTime);
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);

        targetEnemy.Slow(slowFactor);
        if (!lineRend.enabled)
        {
            lineRend.enabled = true;
            impactEffect.Play();
            _audioSource.Play();
        }
        //Debug.Log("laser!!!!!");
        lineRend.SetPosition(0, firePoint.position);
        lineRend.SetPosition(1, target.position);
        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized * 1.5f;


    }
    void LockOnTarget()
    {
        //follow target
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        Vector3 rotation = Quaternion.Lerp(towerPointer.rotation, lookRotation, Time.deltaTime * angularSpeed).eulerAngles;
        towerPointer.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
    void Shoot(float _damage)
    {
        //Debug.Log("Shoot initiated!");
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.damage = effectiveDamage;
        if (bullet != null)
            bullet.Seek(target);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, effectiveRange);
    }
}
