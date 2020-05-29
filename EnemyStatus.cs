using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float expValue;
    public float damage;
    public bool isFromSpawner;
    public bool dealContactDamage;
    public float timeBetweenHits;
    PlayerStatus player;
    [SerializeField] private GameObject numberPopup;
    float timeLastHit;
    float itemDropRNG;
    public LootTable lootTable;
    bool hasDropped;
    public GameObject chest;
    public GameObject particle;
    public bool canBlind;
    bool expContabilizada = false;


    // Start is called before the first frame update
    public void TakeDamage(float damage)
    {
        float critChance = Mathf.RoundToInt(Random.Range(0.0f, 100.0f));
        bool isCrit = false;
        if (player != null)
        {
            if (player.GetComponent<PlayerStatus>() != null)
            {
                if (critChance <= player.GetComponent<PlayerStatus>().playerCritChance)
                {
                    isCrit = true;
                }
                if (isCrit)
                {
                    damage = Mathf.RoundToInt(damage * (1 + (player.GetComponent<PlayerStatus>().playerCritDamage / 100)));
                }
            }
        }

        var damagePopupReference = Instantiate(numberPopup, gameObject.transform.position, Quaternion.identity);
        var damagePopupScript = damagePopupReference.GetComponent<NumberPopup>();
        if (isCrit)
        {
            damagePopupScript.Setup(Mathf.RoundToInt(damage), "critical");
        }
        else
        {
            damagePopupScript.Setup(Mathf.RoundToInt(damage), "damage");
        }
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        int itemThatWillDrop = Mathf.RoundToInt(Random.Range(0.0f, lootTable.lootableItems.Count - 1));
        itemDropRNG = Random.Range(0.0f, 100.0f);
        if (itemDropRNG <= lootTable.lootableItems[itemThatWillDrop].dropChance && !hasDropped)
        {
            var dropChest = Instantiate(chest, gameObject.transform.position, Quaternion.identity);
            dropChest.GetComponent<ChestManager>().containedItem = lootTable.lootableItems[itemThatWillDrop].item;
            hasDropped = !hasDropped;
        }

        if (isFromSpawner)
        {
            transform.root.GetComponentInChildren<EnemySpawnerSimple>().spawnedEnemies.Remove(gameObject);
        }

        var expPopupReference = Instantiate(numberPopup, player.transform.position + (Vector3.up / 2), Quaternion.identity);
        var expPopupScript = expPopupReference.GetComponent<NumberPopup>();
        expPopupScript.Setup(expValue, "exp");

        if (player.name == "Player" && !expContabilizada)
        {
            player.currentExp += expValue;
            Debug.Log("Died");
            expContabilizada = true;
        }

        Instantiate(particle, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        timeLastHit += Time.deltaTime;
        if (collider.name == "Player" && dealContactDamage)
        {
            player = collider.GetComponent<PlayerStatus>();
            if (player.name != null && timeLastHit >= timeBetweenHits)
            {
                player.TakeDamage(damage);
                timeLastHit = 0;
            }
        }
        if (collider.name == "EnemyRange")
        {
            player = collider.transform.parent.GetComponent<PlayerStatus>();
        }
    }
}
