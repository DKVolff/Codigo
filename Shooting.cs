using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public Camera camLink;
    public float projectileOffset;
    Vector2 lookDir;
    Vector2 mousePos;
    public SpriteRenderer sr;
    public PlayerStatus playerStatus;
    float lastShot = 0;
    public float bulletForce = 10f;
    public Joystick joystick;
    float totalFireRate;
    float totalDamage;
    bool combatValuesUpdated = false;
    int shoot = 0;
    float fireAngle;
    // Update is called once per frame

    void Start()
    {

    }

    void UpdateCombatValues()
    {
        totalDamage = 0;
        totalFireRate = 0;
        totalDamage += playerStatus.damage;
        totalFireRate += playerStatus.fireRate;
        //if (playerStatus.equipedWeapon != null)
        //{
        //    totalFireRate += playerStatus.equipedWeapon.GetComponent<WeaponStatus>().rateOfFire;
        //     totalDamage += playerStatus.equipedWeapon.GetComponent<WeaponStatus>().damage;
        //}
        CheckForSpecialEffects();
    }

    void CheckForSpecialEffects()
    {

        foreach (var effect in playerStatus.specialEffectsList)
        {
        }
    }

    void Update()
    {
        if (!combatValuesUpdated)
        {
            UpdateCombatValues();
            combatValuesUpdated = !combatValuesUpdated;
        }
        lastShot += Time.deltaTime;
        if (lookDir != Vector2.zero)
        {
            if (lastShot >= 1 / totalFireRate)
            {
                Shoot();
            }
        }
        // mousePos = camLink.ScreenToWorldPoint(Input.mousePosition);

        if (lookDir.x < 0)
        {
            sr.flipX = true;
        }
        else if (lookDir.x > 0)
        {
            sr.flipX = false;
        }
    }

    void FixedUpdate()
    {
        //lookDir = mousePos - rb.position;
        lookDir = new Vector2(joystick.Horizontal, joystick.Vertical);
    }

    void Shoot()
    {
        shoot++;
        Debug.Log(shoot + "projeteis disparados");
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        float projectileRotation = 0;
        fireAngle = angle;

        foreach (GameObject playerFirePoint in playerStatus.firePoints)
        {
            playerFirePoint.transform.rotation = Quaternion.Euler(0, 0, fireAngle);
            fireAngle += 15f;
            GameObject projectile = Instantiate(projectilePrefab, playerFirePoint.transform.position, Quaternion.Euler(0, 0, angle + projectileOffset));
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.rotation = angle + projectileOffset + projectileRotation;
            projectileRotation += 15f;
            projectile.GetComponent<BulletController>().damage = totalDamage;
            projectile.GetComponent<BulletController>().origin = gameObject;
            rb.AddForce(playerFirePoint.transform.up * bulletForce, ForceMode2D.Impulse);
        }


        lastShot = 0;
    }
}
