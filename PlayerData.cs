using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int playerSaveSlot;
    public string playerLastVisitedStage;
    public string playerName;
    public string playerClass;
    public int playerLevel;
    public float currentExp;
    public int nextExp;
    public int playerPointsLeft;
    public int playerHealthPotions;
    public int playerManaPotions;
    public float playerMaxHealth;
    public int playerMaxMana;
    public float playerDamage;
    public float playerFireRate;
    public float playerCritChance;
    public float playerCritDamage;
    public float playerDefense;
    public float playerToughness;
    public float playerEvasion;
    public int playerGold;
    public string playerEquipedWeapon;
    public string playerEquipedArmor;
    public string playerEquipedTrinket1;
    public string playerEquipedTrinket2;
    public string[] playerMenuInventory = new string[16];

    public PlayerData(PlayerStatus playerStatus)
    {
        playerSaveSlot = playerStatus.playerSaveSlot;
        playerLastVisitedStage = playerStatus.playerLastVisitedStage;
        playerName = playerStatus.playerName;
        playerClass = playerStatus.playerClass;
        playerLevel = playerStatus.playerLevel;
        currentExp = playerStatus.currentExp;
        nextExp = playerStatus.nextExp;
        playerPointsLeft = playerStatus.playerPointsLeft;
        playerMaxHealth = playerStatus.maxHealth;
        playerDamage = playerStatus.damage;
        playerFireRate = playerStatus.fireRate;
        playerCritChance = playerStatus.playerCritChance;
        playerCritDamage = playerStatus.playerCritDamage;
        playerGold = playerStatus.playerGold;
        playerDefense = playerStatus.defense;
        playerToughness = playerStatus.toughness;
        playerEvasion = playerStatus.evasion;
        playerHealthPotions = playerStatus.healthPotions;
        playerManaPotions = playerStatus.manaPotions;



    }
}
