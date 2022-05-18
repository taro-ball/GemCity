using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startHealth;
    private float health;
    public int worth = 50;
    //public float speed = 1f;
    public GameObject deathEffect;

    //public Camera cam;
    public NavMeshAgent agent;
    public Image healthBar;
    private Transform destination1;
    private Animator anim;
    public float animationDelaySeconds=1f;
    private float baseSpeed;
    private float slowTimer;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = 1f;
        //Debug.Log("=====START: health/startHealth: " + health + "/" + startHealth);
        baseSpeed = agent.speed;
        //cam = Camera.main;
            //GameObject.FindGameObjectWithTag("Camera").GetComponent<Camera>();
        destination1 = GameObject.Find("destination").transform;
        anim = GetComponent<Animator>();
        EnemySendTo(destination1.position);
    }

    // Update is called once per frame
    void Update()
    {
        //agent.speed += 1;
        //if (input.getmousebuttondown(0))
        //{
        //    ray ray = cam.screenpointtoray(input.mouseposition);
        //    raycasthit hit;
        //    if (physics.raycast(ray, out hit))
        //    {
        //        agent.setdestination(hit.point);
        //    }
        //}

        //Debug.Log("remainingDistance: " + agent.remainingDistance);
        if (agent.remainingDistance > 0 && agent.remainingDistance < 1f)
        {
            // Play anims
            //anim.SetInteger("moving", 0);
            //Debug.Log("Arrivved!");
            endPath();
            //gameObject.SetActive(false);
            return;
        }
        slowTimer -= Time.deltaTime;
        if (slowTimer > 0)
        {
            agent.speed = baseSpeed * 0.5f;
        }
        else { agent.speed = baseSpeed; }

    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health/startHealth;
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Slow(float amount)
    {
        //agent.speed = baseSpeed * (1f - amount);
        slowTimer = 1f;
    }
    void Die()
    {

        //dont allow to die multiple times
        PlayerStats.enemiesTotal += 1;
        isDead = true;
        PlayerStats.Money += worth;
        PlayerStats.moneyTotal += worth;
        if (!agent.isStopped)
        {
            //Debug.Log("die animation - " + gameObject.GetInstanceID());
            //anim.SetInteger("moving", 0);
            //remember to set HasExitTime!
            anim.SetInteger("moving", 13);
        }
        if (agent != null)
        {
            agent.isStopped = true;
        }


        //dont let corpses slow down others
        Destroy(gameObject.GetComponent<CapsuleCollider>());

        //Destroy(gameObject.GetComponent<NavMeshAgent>());
        //GameObject effectDeath = (GameObject)
        Instantiate(deathEffect, transform.position, transform.rotation);
        WaveSpawner.enemiesAlive--;
        //TODO!!!!
        //delete death effect! (self destruct?)
        //Destroy(deathEffect,2f);
        Destroy(gameObject, animationDelaySeconds);
    }

    void endPath()
    {
        PlayerStats.Lives--;
        WaveSpawner.enemiesAlive--;
        Destroy(gameObject);
    }
    void EnemySendTo(Vector3 destination)
    {
        agent.SetDestination(destination);
        anim.SetInteger("moving", 1);
    }
}
