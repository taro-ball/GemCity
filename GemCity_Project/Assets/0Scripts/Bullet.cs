using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target4Bullet;
    public float speed = 20f;
    public GameObject impactEffect;
    public float explosionRange = 0f;
    [HideInInspector]
    public float damage;
    public void Seek(Transform targetIncoming)
    {
        target4Bullet = targetIncoming;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target4Bullet == null)
        {
            //destroy if target disappeared (e.g. killed already)
            Destroy(gameObject);
            //but still play destruction effect
            GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            return;
        }
        Vector3 dir = target4Bullet.position - transform.position;
        float distanceThisFraame = speed * Time.deltaTime;
        if (dir.magnitude <= distanceThisFraame)
        {
            //if overshoot move to enemy
            transform.position = target4Bullet.position;
            Hittarget4Bullet();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFraame, Space.World);
        transform.LookAt(target4Bullet);
    }
    void Hittarget4Bullet()
    {
        //Debug.Log("hITTTTTT!");
        GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        //destroy effect after delay
        Destroy(effectInstance, 3f);

        if (explosionRange > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target4Bullet);
        }
        //destroy the bullet
        Destroy(gameObject);


    }
    void Explode()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange);
        //Debug.Log("Explode: "+ colliders.ToString()+"     Range: "+ explosionRange);
        foreach (Collider collid in colliders)
        {
            //Debug.Log("collid.tag: "+collid.tag + collid.name);
            //if (collid.tag == "Enemy")
            if (collid.CompareTag("Enemy"))
            {
                Damage(collid.transform);
            }
        }
    }
    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(damage);
        }

        //Destroy(enemy.gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
