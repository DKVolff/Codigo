using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damage;
    public float projectileLife;
    int projectileHealth = 1;
    public GameObject origin;
    float projectileOut;
    private GameObject particle;
    public bool isBlinding;

    void Start()
    {
        CheckForSpecialEffects();
    }
    void Update()
    {
        projectileOut += Time.deltaTime;
        if (projectileOut >= projectileLife)
        {
            Destroy(gameObject);
        }
        if (projectileHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void CheckForSpecialEffects()
    {
        if (origin.GetComponent<PlayerStatus>() != null)
        {
            foreach (var effect in origin.GetComponent<PlayerStatus>().specialEffectsList)
            {
                if (effect == "CHAIN")
                {
                    damage = damage * 0.8f;
                    projectileHealth++;
                }

                if (effect == "MULTI")
                {
                    damage = damage * 0.6f;
                }
            }
        }

        if (origin.GetComponent<EnemyStatus>() != null)
        {
            if (origin.GetComponent<EnemyStatus>().canBlind)
            {
                isBlinding = true;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerStatus player = hitInfo.GetComponent<PlayerStatus>();
        EnemyStatus enemy = hitInfo.GetComponent<EnemyStatus>();
        if (enemy != null)
        {
            Vector3 shotDir;
            Instantiate(enemy.GetComponent<EnemyStatus>().particle, transform.position, Quaternion.identity);
            enemy.TakeDamage(damage);
            projectileOut = 0;
            if (Physics2D.OverlapCircleAll(gameObject.transform.position, 3f).Length > 0)
            {
                foreach (Collider2D objectInRange in Physics2D.OverlapCircleAll(gameObject.transform.position, 3f))
                {
                    if (objectInRange.gameObject.tag == "Enemy")
                    {

                        if (objectInRange.gameObject.transform.position != hitInfo.gameObject.transform.position)
                        {
                            shotDir = objectInRange.gameObject.transform.position - gameObject.transform.position;

                            float angle = Mathf.Atan2(shotDir.y, shotDir.x) * Mathf.Rad2Deg - 135f;
                            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
                            gameObject.GetComponent<Rigidbody2D>().AddForce(shotDir.normalized * 10, ForceMode2D.Impulse);

                            break;
                        }
                    }
                }
            }
            projectileHealth--;
        }
        if (player != null)
        {
            Instantiate(player.GetComponent<PlayerStatus>().particle, transform.position, Quaternion.identity);
            player.TakeDamage(damage);
            if (isBlinding)
            {
                player.GetComponent<PlayerStatus>().isBlinded = true;
                player.GetComponent<PlayerStatus>().blindDuration = 1;
            }
            projectileOut = 0;
            projectileHealth--;
        }

        if (player == null && enemy == null)
        {

            projectileHealth = 0;
        }
    }

}
