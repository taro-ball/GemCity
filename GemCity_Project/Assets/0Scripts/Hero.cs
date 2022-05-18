using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Hero : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;
    private float fireCountdown = 0f;
    public float baseRange;
    public float baseDamage;
    public float baseFireRate;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public ParticleSystem impactEffect;
    public Transform towerPointer;
    public float angularSpeed = 10f;
    private string enemyTag = "Enemy";
    public BuildManager buildManager;
    [HideInInspector]
    public float effectiveFireRate;
    [HideInInspector]
    public float effectiveRange;
    [HideInInspector]
    public float effectiveDamage;


    public float startHealth = 500;
    private float health;
    //public int worth = 50;
    //public float speed = 1f;
    public GameObject deathEffect;

    private bool HeroStopped;
    private Camera cam;
    public NavMeshAgent agent;
    public Image healthBar;
    private Animator anim;
    public float animationDelaySeconds = 1f;
    private float baseSpeed;
    //private float slowTimer;
    public bool isDead = false;

    // Start is called before the first frame update

    bool agentStopped
    {
        get
        { return (agent.velocity.magnitude < 0.15f); }
    }

    void Start()
    {
        isDead = false;
        health = startHealth;
        healthBar.fillAmount = 1f;
        //Debug.Log("=====START: health/startHealth: " + health + "/" + startHealth);
        baseSpeed = agent.speed;
        cam = Camera.main;
        anim = GetComponent<Animator>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        HeroStopped = true;

    }

    // Update is called once per frame

    void Update()
    {
        //TakeDamage(2f);
        //Debug.Log("moving: " + anim.GetInteger("moving"));
        if (isDead)
        { return; }

        if (target == null)
        {
            anim.SetInteger("battle", 0);
            //return;
        }


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {



                //if (EventSystem.current.IsPointerOverGameObject())
                //{
                //    PointerEventData pointerData = new PointerEventData(EventSystem.current)
                //    {
                //        pointerId = -1,
                //    };

                //    pointerData.position = Input.mousePosition;

                //    List<RaycastResult> results = new List<RaycastResult>();
                //    EventSystem.current.RaycastAll(pointerData, results);

                //    if (results.Count > 0)
                //    {
                //        for (int i = 0; i < results.Count; ++i)
                //        {
                //            if (results[i].gameObject.CompareTag("node"))
                //            {
                //                isOverNode = true;
                //                Debug.Log("clixked on node! " + results[i].gameObject.tag);
                //            }
                //        }
                //    }
                //}

                //if (EventSystem.current.IsPointerOverGameObject() &&
                //    EventSystem.current.currentSelectedGameObject != null &&
                //    !isOverNode)
                //{
                //    //Debug.Log("clicked on tag: " + EventSystem.current.currentSelectedGameObject.tag);
                //    ////buildManager.DeselectNode();

                if (EventSystem.current.IsPointerOverGameObject() || hit.transform.gameObject.CompareTag("node"))
                { Debug.Log("on ui or node"); }
                else { HeroSendTo(hit.point); }

                //}
                //https://answers.unity.com/questions/1429689/specific-ui-element.html
                //hit.
                //buildManager.DeselectNode();
                //agent.SetDestination(hit.point);
            }
        }

        //Debug.Log("remainingDistance: " + agent.remainingDistance);
        if (agent.remainingDistance > 0 && agent.remainingDistance < 1f && !HeroStopped)
        {
            endPath();
            //return;
        }
        //dont shoot if no target, dead or moving
        if (target == null || isDead || !agentStopped)
        {
            //Debug.Log("agent.isStopped: " + agent.isStopped);

            return;
        }

        LockOnTarget();



        if (fireCountdown <= 0)
        {

            Shoot(effectiveDamage);
            fireCountdown = 1f / effectiveFireRate;

        }
        fireCountdown -= Time.deltaTime;


    }
    void LockOnTarget()
    {
        //follow target
        //Debug.Log("got target: " + target.name);
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        Vector3 rotation = Quaternion.Lerp(towerPointer.rotation, lookRotation, Time.deltaTime * angularSpeed).eulerAngles;
        towerPointer.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Attack()
    {
        if (isDead || agent.isStopped != true)
        { return; }

    }
    void Shoot(float _damage)
    {
        //Debug.Log("Hero shoot!");
        anim.SetInteger("battle", 1);
        //anim.SetInteger("moving", 0);
        anim.SetInteger("moving", 3);
        //anim.SetInteger("battle", 0);

        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.damage = effectiveDamage;
        if (bullet != null)
            bullet.Seek(target);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDinstance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        effectiveRange = baseRange;
        effectiveFireRate = baseFireRate;
        effectiveDamage = baseDamage;
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




    public void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }


    void Die()
    {

        //dont allow to die multiple times
        Debug.Log("hero died!");
        isDead = true;
        agent.isStopped = true;
        anim.SetInteger("moving", 13);
        //anim.Play("death_2");
        //Destroy(gameObject.GetComponent<NavMeshAgent>());
        //GameObject effectDeath = (GameObject)
        Instantiate(deathEffect, transform.position, transform.rotation);
        //gameObject.SetActive(false);
        Destroy(gameObject, 0.85f);
    }


    void endPath()
    {
        //Destroy(gameObject);
        anim.SetInteger("moving", 0);
        HeroStopped = true;
        //agent.isStopped = true;
        Debug.Log("Hero end path!");
    }
    void HeroSendTo(Vector3 destination)
    {
        HeroStopped = false;
        agent.SetDestination(destination);
        anim.SetInteger("moving", 2);
    }
}
