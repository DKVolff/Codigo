using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class WeaponStatus : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public int itemTier;
    public int timesUpgraded;
    public int itemPoints;
    public float damage;
    public float rateOfFire;
    public float criticalRate;
    public float criticalDamage;
    public string specialEffect;
    public string flavourText;
    public int valueInGold;

    public static string GetWeaponId(string data)
    {
        string[] weapon = data.Split('|');
        return weapon[1];
    }
    public static GameObject DataToWeapon(string data, GameObject item)
    {
        string[] weapon = data.Split('|');
        for (var i = 0; i < weapon.Length; i++)
        {
            if (i == 2)
            {

                item.GetComponent<WeaponStatus>().itemTier = System.Convert.ToInt32(weapon[i]);
            }
            if (i == 3)
            {
                item.GetComponent<WeaponStatus>().timesUpgraded = System.Convert.ToInt32(weapon[i]);
            }
            if (i == 4)
            {

                item.GetComponent<WeaponStatus>().damage = System.Convert.ToInt32(weapon[i]);
            }
            if (i == 5)
            {
                try
                {

                    string[] fireRate;
                    fireRate = weapon[i].Split('.');
                    if (fireRate.Length <= 1)
                    {
                        fireRate = weapon[i].Split(',');
                    }
                    if (fireRate.Length > 1)
                    {
                        while (System.Convert.ToDecimal(fireRate[1]) >= 11)
                        {
                            fireRate[1] = System.Convert.ToString(System.Convert.ToDecimal(fireRate[1]) / 10);
                        }
                    }

                    if (fireRate.Length > 1)
                    {
                        item.GetComponent<WeaponStatus>().rateOfFire = (float)System.Convert.ToDecimal(fireRate[0]) + (float)(System.Convert.ToDecimal(fireRate[1]) / 10);
                    }
                    else
                    {
                        item.GetComponent<WeaponStatus>().rateOfFire = System.Convert.ToInt32(weapon[i]);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                    item.GetComponent<WeaponStatus>().rateOfFire = 0;
                }

            }
            if (i == 6)
            {

                item.GetComponent<WeaponStatus>().criticalRate = System.Convert.ToInt32(weapon[i]);
            }
            if (i == 7)
            {

                item.GetComponent<WeaponStatus>().criticalDamage = System.Convert.ToInt32(weapon[i]);
            }
        }
        return item;
    }
    public static string WeaponToData(WeaponStatus arma)
    {
        return "w" + "|" + arma.itemID + "|"
                           + arma.itemTier + "|"
                           + arma.timesUpgraded + "|"
                           + arma.damage + "|"
                           + arma.rateOfFire + "|"
                           + arma.criticalRate + "|"
                           + arma.criticalDamage;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
