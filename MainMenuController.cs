using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Screens")]
    public GameObject startScreen;
    public GameObject fileSelect;
    public GameObject classSelect;
    public GameObject confirmClass;
    public GameObject nameWrite;
    public GameObject mainMenu;
    public GameObject explorationSelect;
    public GameObject loadOut;
    public GameObject inventoryMenu;
    public GameObject shopMenu;
    public GameObject statusScreen;
    public GameObject topBar;
    [Header("Inventory")]
    public GameObject sellConfirmation;
    public GameObject upgradeConfirmation;
    public Text upgradeCost;
    public Text upgradeRequirement;
    [Header("Misc")]
    public string selectedStage;
    public Text goldText;
    //public Text energyText;
    //public Text rubiesText;
    public GameObject playerObject;
    public Text inputText;
    public GameObject deleteModeText;
    public Text buttonText;
    bool deleteMode;
    PlayerData playerData;

    public List<string> itemAttributeValues;
    public List<string> itemAttributeNames;
    public GameObject inventoryWindow;
    public GameObject inventoryMenuSlots;
    private Transform selectedItem;
    public GameObject statusWindowEquipment;
    public GameObject statusWindowStatus;
    public GameObject weaponSlot;
    public GameObject armorSlot;
    public GameObject trinket1Slot;
    public GameObject trinket2Slot;
    public GameObject equipButton;
    public GameObject unequipButton;
    public bool refillPotionsWithGold;
    public bool refillPotionsWithRubies;
    [Header("Shop Buttons")]
    public GameObject shopConfirmation;
    public Button itemBuy;
    public Button weaponBuy;
    public Button weaponAd;
    public Button armorBuy;
    public Button armorAd;
    public Button AcessoryBuy;
    public Button AcessoryAd;

    [Header("Shop Stage Text")]
    public GameObject stageItemText;
    public GameObject stageWeaponText;
    public GameObject stageArmorText;
    public GameObject stageAcessoryText;
    [Header("Shop Prices")]
    public Text itemPrice;
    public Text weaponPrice;
    public Text armorPrice;
    public Text acessoryPrice;

    [Header("Shop Items")]
    public List<GameObject> possibleItems = new List<GameObject>();
    public GameObject itemBought;
    public GameObject itemBoughtWindow;
    void Awake()
    {

    }
    void Start()
    {
        playerObject.GetComponent<PlayerStatus>().currentHealth = playerObject.GetComponent<PlayerStatus>().maxHealth;
        try
        {
            if (SaveSystem.selectedSlot != 0)
            {
                if (SaveSystem.selectedSlot == 1)
                {
                    playerData = SaveSystem.LoadPlayer1();
                    DisableAll();
                    GoToMainMenu(playerData);
                }
            }
        }
        catch
        {

        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateEverything()
    {
        goldText.text = System.Convert.ToString(playerObject.GetComponent<PlayerStatus>().playerGold);
        SaveSystem.SavePlayer(playerObject.GetComponent<PlayerStatus>());
        UpdateStatusWindow(statusWindowEquipment);
        UpdateStatusWindow(statusWindowStatus);
    }
    public void GoToFileSelect()
    {
        //SaveSystem.DeletePlayer(1);
        playerObject.GetComponent<PlayerStatus>().ClearTrashData();
        deleteMode = false;
        deleteModeText.SetActive(false);
        DisableAll();
        topBar.SetActive(false);
        PlayerData data1 = SaveSystem.LoadPlayer1();
        //file1
        if (data1 == null)
        {
            foreach (Transform slot in fileSelect.transform)
            {
                if (slot.name == "Slot1")
                {
                    foreach (Transform child in slot.transform)
                    {
                        if (child.name == "Text")
                        {
                            child.gameObject.SetActive(true);
                            child.GetComponent<Text>().text = "EMPTY";
                        }
                        if (child.name == "File1Info")
                        {
                            child.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        else
        {
            foreach (Transform slot in fileSelect.transform)
            {
                if (slot.name == "Slot1")
                {
                    foreach (Transform child in slot.transform)
                    {
                        if (child.name == "Text")
                        {
                            child.gameObject.SetActive(false);
                        }
                        if (child.name == "File1Info")
                        {
                            var textList = child.GetComponentsInChildren<Text>();
                            foreach (Text value in textList)
                            {
                                if (value.name == "ClassLevel")
                                {
                                    value.text = "Level " + Convert.ToString(data1.playerLevel) + " - " + Convert.ToString(data1.playerClass);
                                }
                                if (value.name == "PlayerName")
                                {
                                    value.text = data1.playerName;
                                }
                                if (value.transform.parent.name == "HeartIcon")
                                {
                                    value.text = Convert.ToString(data1.playerMaxHealth);
                                }
                                if (value.transform.parent.name == "DamageIcon")
                                {
                                    value.text = Convert.ToString(data1.playerDamage);
                                }
                                if (value.transform.parent.name == "AttackSpeedIcon")
                                {
                                    value.text = data1.playerFireRate.ToString("F1");
                                }
                                if (value.transform.parent.name == "CriticalChanceIcon")
                                {
                                    value.text = Convert.ToString(data1.playerCritChance) + "%";
                                }
                                if (value.transform.parent.name == "CriticalDamageIcon")
                                {
                                    value.text = Convert.ToString(data1.playerCritDamage) + "%";
                                }
                            }
                            child.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
        fileSelect.SetActive(true);
    }

    public void DeleteSave()
    {
        SaveSystem.DeletePlayer(1);
    }

    public void LoadOrCreate1()
    {
        DisableAll();
        SaveSystem.selectedSlot = 1;
        playerData = SaveSystem.LoadPlayer1();
        if (playerData == null)
        {
            playerObject.GetComponent<PlayerStatus>().playerSaveSlot = 1;
            GoToCreateClass();
        }
        else
        {
            GoToMainMenu(playerData);
        }
    }

    void LoadPlayerObject(PlayerData data)
    {
        PlayerStatus nowPlaying = playerObject.GetComponent<PlayerStatus>();
        Debug.Log("Loading Player Data");
        nowPlaying.playerSaveSlot = data.playerSaveSlot;
        nowPlaying.playerLastVisitedStage = data.playerLastVisitedStage;
        nowPlaying.playerName = data.playerName;
        nowPlaying.playerClass = data.playerClass;
        nowPlaying.playerLevel = data.playerLevel;
        nowPlaying.currentExp = data.currentExp;
        nowPlaying.nextExp = data.nextExp;
        nowPlaying.playerPointsLeft = data.playerPointsLeft;
        nowPlaying.maxHealth = data.playerMaxHealth;
        nowPlaying.damage = data.playerDamage;
        nowPlaying.fireRate = data.playerFireRate;
        nowPlaying.playerCritChance = data.playerCritChance;
        nowPlaying.playerCritDamage = data.playerCritDamage;
        nowPlaying.playerGold = data.playerGold;
        nowPlaying.defense = data.playerDefense;
        nowPlaying.toughness = data.playerToughness;
        nowPlaying.evasion = data.playerEvasion;
        nowPlaying.healthPotions = data.playerHealthPotions;
        nowPlaying.manaPotions = data.playerManaPotions;

        if (data.playerEquipedWeapon != null)
        {
            var id = WeaponStatus.GetWeaponId(data.playerEquipedWeapon);
            foreach (GameObject weapon in gameObject.GetComponent<LoadResources>().items)
            {
                if (weapon.GetComponent<WeaponStatus>() != null)
                {

                    if (Convert.ToInt32(id) == weapon.GetComponent<WeaponStatus>().itemID)
                    {
                        nowPlaying.equipedWeapon = Instantiate(weapon, new Vector2(2000f, 2000f), Quaternion.identity);
                        WeaponStatus.DataToWeapon(data.playerEquipedWeapon, nowPlaying.equipedWeapon);
                        nowPlaying.equipedWeapon.transform.parent = weaponSlot.transform;
                        nowPlaying.equipedWeapon.transform.position = weaponSlot.transform.position;
                    }
                }
            }
        }

        if (data.playerEquipedArmor != null)
        {
            var id = ArmorStatus.GetArmorId(data.playerEquipedArmor);
            foreach (GameObject armor in gameObject.GetComponent<LoadResources>().items)
            {
                if (armor.GetComponent<ArmorStatus>() != null)
                {

                    if (Convert.ToInt32(id) == armor.GetComponent<ArmorStatus>().itemID)
                    {
                        nowPlaying.equipedArmor = Instantiate(armor, new Vector2(2000f, 2000f), Quaternion.identity);
                        ArmorStatus.DataToArmor(data.playerEquipedArmor, nowPlaying.equipedArmor);
                        nowPlaying.equipedArmor.transform.parent = armorSlot.transform;
                        nowPlaying.equipedArmor.transform.position = armorSlot.transform.position;
                    }
                }
            }
        }

        if (data.playerEquipedTrinket1 != null)
        {
            var id = TrinketStatus.GetTrinketId(data.playerEquipedTrinket1);
            foreach (GameObject trinket in gameObject.GetComponent<LoadResources>().items)
            {
                if (trinket.GetComponent<TrinketStatus>() != null)
                {

                    if (Convert.ToInt32(id) == trinket.GetComponent<TrinketStatus>().itemID)
                    {
                        nowPlaying.equipedTrinket1 = Instantiate(trinket, new Vector2(2000f, 2000f), Quaternion.identity);
                        TrinketStatus.DataToTrinket(data.playerEquipedTrinket1, nowPlaying.equipedTrinket1);
                        nowPlaying.equipedTrinket1.transform.parent = trinket1Slot.transform;
                        nowPlaying.equipedTrinket1.transform.position = trinket1Slot.transform.position;
                    }
                }
            }
        }
        if (data.playerEquipedTrinket2 != null)
        {
            var id = TrinketStatus.GetTrinketId(data.playerEquipedTrinket2);
            foreach (GameObject trinket in gameObject.GetComponent<LoadResources>().items)
            {
                if (trinket.GetComponent<TrinketStatus>() != null)
                {

                    if (Convert.ToInt32(id) == trinket.GetComponent<TrinketStatus>().itemID)
                    {
                        nowPlaying.equipedTrinket2 = Instantiate(trinket, new Vector2(2000f, 2000f), Quaternion.identity);
                        TrinketStatus.DataToTrinket(data.playerEquipedTrinket2, nowPlaying.equipedTrinket2);
                        nowPlaying.equipedTrinket2.transform.parent = trinket2Slot.transform;
                        nowPlaying.equipedTrinket2.transform.position = trinket2Slot.transform.position;
                    }
                }
            }
        }
        foreach (string item in data.playerMenuInventory)
        {
            if (item != null)
            {
                if (item[0] == 'w')
                {
                    var id = WeaponStatus.GetWeaponId(item);
                    foreach (GameObject weapon in gameObject.GetComponent<LoadResources>().items)
                    {
                        if (weapon.GetComponent<WeaponStatus>() != null)
                        {
                            if (Convert.ToInt32(id) == weapon.GetComponent<WeaponStatus>().itemID)
                            {
                                var actualWeapon = Instantiate(weapon, new Vector2(2000f, 2000f), Quaternion.identity);
                                WeaponStatus.DataToWeapon(item, actualWeapon);

                                for (var i = 0; i < 16; i++)
                                {
                                    if (nowPlaying.menuInventorySlots[i] == null)
                                    {
                                        nowPlaying.menuInventorySlots[i] = actualWeapon;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                }

                if (item[0] == 'a')
                {
                    var id = ArmorStatus.GetArmorId(item);
                    foreach (GameObject armor in gameObject.GetComponent<LoadResources>().items)
                    {

                        if (armor.GetComponent<ArmorStatus>() != null)
                        {
                            if (Convert.ToInt32(id) == armor.GetComponent<ArmorStatus>().itemID)
                            {
                                var actualArmor = Instantiate(armor, new Vector2(2000f, 2000f), Quaternion.identity);
                                ArmorStatus.DataToArmor(item, actualArmor);

                                for (var i = 0; i < 16; i++)
                                {
                                    if (nowPlaying.menuInventorySlots[i] == null)
                                    {
                                        nowPlaying.menuInventorySlots[i] = actualArmor;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (item[0] == 't')
                {
                    var id = TrinketStatus.GetTrinketId(item);
                    foreach (GameObject trinket in gameObject.GetComponent<LoadResources>().items)
                    {
                        if (trinket.GetComponent<TrinketStatus>() != null)
                        {
                            if (Convert.ToInt32(id) == trinket.GetComponent<TrinketStatus>().itemID)
                            {
                                var actualTrinket = Instantiate(trinket, new Vector2(2000f, 2000f), Quaternion.identity);
                                TrinketStatus.DataToTrinket(item, actualTrinket);

                                for (var i = 0; i < 16; i++)
                                {
                                    if (nowPlaying.menuInventorySlots[i] == null)
                                    {
                                        nowPlaying.menuInventorySlots[i] = actualTrinket;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }


    }

    public void GoToInputName()
    {
        if (!string.IsNullOrEmpty(playerObject.GetComponent<PlayerStatus>().playerClass))
        {
            DisableAll();
            nameWrite.SetActive(true);
        }
    }

    public void GoToInventoryMenu()
    {
        DisableAll();
        var inventory = playerObject.GetComponent<PlayerStatus>().menuInventorySlots;
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {

                foreach (Transform slot in inventoryMenuSlots.transform)
                {
                    if ((i + 1).ToString() == slot.name)
                    {
                        inventory[i].transform.position = slot.transform.position;
                        inventory[i].transform.SetParent(slot);
                    }
                }
                inventory[i].name = "Slot " + i;
            }
        }

        inventoryMenu.SetActive(true);
    }

    public void GoToStatusScreen()
    {
        DisableAll();
        statusScreen.SetActive(true);
    }
    public void SetClass()
    {
        confirmClass.GetComponent<Button>().interactable = true;
        playerObject.GetComponent<PlayerStatus>().playerClass = EventSystem.current.currentSelectedGameObject.name;
        buttonText.text = "Create " + playerObject.GetComponent<PlayerStatus>().playerClass;

    }
    public void GoBack()
    {
        if (sellConfirmation.activeSelf == true)
        {
            sellConfirmation.SetActive(false);
        }
        if (upgradeConfirmation.activeSelf == true)
        {
            upgradeConfirmation.SetActive(false);
        }
        if (fileSelect.activeSelf == true)
        {
            DisableAll();
        }
        if (nameWrite.activeSelf == true)
        {
            DisableAll();
            classSelect.SetActive(true);
        }
        if (classSelect.activeSelf == true)
        {
            DisableAll();
            startScreen.SetActive(true);
        }
        if (mainMenu.activeSelf == true)
        {
            DisableAll();
            UpdateEverything();
            playerObject.GetComponent<PlayerStatus>().ClearTrashData();
            topBar.SetActive(false);
            startScreen.SetActive(true);
        }
        if (explorationSelect.activeSelf == true)
        {
            DisableAll();
            if (playerData != null)
            {

                GoToMainMenu(playerData, false);
            }
            else
            {
                mainMenu.SetActive(true);
            }
        }
        if (shopMenu.activeSelf == true)
        {
            DisableAll();
            if (playerData != null)
            {

                GoToMainMenu(playerData, false);
            }
            else
            {
                mainMenu.SetActive(true);
            }
        }
        if (loadOut.activeSelf == true)
        {
            DisableAll();
            GoToExplorationSelect();
        }
        if (inventoryMenu.activeSelf == true)
        {
            foreach (Transform slot in inventoryMenuSlots.transform)
            {
                if (slot.childCount > 0 && slot.childCount < 3)
                {
                    foreach (Transform child in slot.transform)
                    {
                        child.name = "OLD";
                    }
                }
            }
            DisableAll();
            if (playerData != null)
            {

                GoToMainMenu(playerData, false);
            }
            else
            {
                mainMenu.SetActive(true);
            }
        }
        if (statusScreen.activeSelf == true)
        {
            DisableAll();
            GoToMainMenu(playerData, false);
        }
    }

    public void CreateFile()
    {
        if (inputText.text.Length >= 3)
        {
            SetClassStatus(playerObject.GetComponent<PlayerStatus>().playerClass);
            playerObject.GetComponent<PlayerStatus>().playerName = inputText.text;

            SaveSystem.SavePlayer(playerObject.GetComponent<PlayerStatus>());
            DisableAll();
            GoToMainMenu(SaveSystem.LoadPlayer1());
        }
        else
        {
            Debug.Log("Not allow");
        }

    }


    public void DeleteModeToggle()
    {
        deleteMode = !deleteMode;
        if (deleteMode == true)
        {
            deleteModeText.SetActive(true);
        }
        else
        {
            deleteModeText.SetActive(false);
        }
    }
    void GoToCreateClass()
    {
        confirmClass.GetComponent<Button>().interactable = false;
        confirmClass.GetComponentInChildren<Text>().text = "Create";
        classSelect.SetActive(true);
    }

    public void StartExploration()
    {
        Debug.Log("AAA");
        if (refillPotionsWithGold)
        {
            while (playerObject.GetComponent<PlayerStatus>().healthPotions < 3)
            {
                if (playerObject.GetComponent<PlayerStatus>().playerGold >= (playerObject.GetComponent<PlayerStatus>().playerLevel * 10))
                {
                    playerObject.GetComponent<PlayerStatus>().playerGold -= (playerObject.GetComponent<PlayerStatus>().playerLevel * 10);
                    playerObject.GetComponent<PlayerStatus>().healthPotions++;
                }
                else
                {
                    break;
                }
            }
            while (playerObject.GetComponent<PlayerStatus>().manaPotions < 3)
            {
                if (playerObject.GetComponent<PlayerStatus>().playerGold >= (playerObject.GetComponent<PlayerStatus>().playerLevel * 10))
                {
                    playerObject.GetComponent<PlayerStatus>().playerGold -= (playerObject.GetComponent<PlayerStatus>().playerLevel * 10);
                    playerObject.GetComponent<PlayerStatus>().manaPotions++;
                }
                else
                {
                    break;
                }
            }
        }
        if (refillPotionsWithRubies)
        {

        }
        playerObject.GetComponent<PlayerStatus>().playerLastVisitedStage = selectedStage;
        UpdateEverything();
        SceneManager.LoadScene(selectedStage);
    }

    public void GoToLoadOut()
    {
        DisableAll();
        selectedStage = EventSystem.current.currentSelectedGameObject.name;
        loadOut.SetActive(true);
    }

    public void GoToShop()
    {
        DisableAll();
        string stage = playerObject.GetComponent<PlayerStatus>().playerLastVisitedStage;
        int playerGold = playerObject.GetComponent<PlayerStatus>().playerGold;
        if (String.IsNullOrEmpty(stage))
        {
            stage = "FIELDS";
        }
        switch (stage)
        {
            case "FIELDS":
                itemPrice.text = "1000";
                weaponPrice.text = "50";
                armorPrice.text = "50";
                acessoryPrice.text = "25";
                break;
            default:
                itemPrice.text = "10000";
                weaponPrice.text = "500";
                armorPrice.text = "500";
                acessoryPrice.text = "250";
                break;
        }
        stageItemText.GetComponent<TextMeshProUGUI>().SetText(stage);
        stageWeaponText.GetComponent<TextMeshProUGUI>().SetText(stage);
        stageArmorText.GetComponent<TextMeshProUGUI>().SetText(stage);
        stageAcessoryText.GetComponent<TextMeshProUGUI>().SetText(stage);
        if (playerGold < Convert.ToInt32(itemPrice.text))
        {
            itemBuy.GetComponent<Button>().interactable = false;
        }
        else
        {
            itemBuy.GetComponent<Button>().interactable = true;
        }
        shopMenu.SetActive(true);
    }
    public void GoToExplorationSelect()
    {
        DisableAll();
        explorationSelect.SetActive(true);
    }

    void GoToMainMenu(PlayerData data, bool reload = true)
    {
        DisableAll();
        if (reload)
        {
            LoadPlayerObject(data);
        }
        UpdateEverything();
        topBar.SetActive(true);
        mainMenu.SetActive(true);
    }


    void DisableAll()
    {
        startScreen.SetActive(false);
        fileSelect.SetActive(false);
        classSelect.SetActive(false);
        nameWrite.SetActive(false);
        mainMenu.SetActive(false);
        explorationSelect.SetActive(false);
        loadOut.SetActive(false);
        inventoryMenu.SetActive(false);
        statusScreen.SetActive(false);
        shopMenu.SetActive(false);
    }

    private void SetClassStatus(string playerClass)
    {
        PlayerStatus status = playerObject.GetComponent<PlayerStatus>();
        if (playerClass == "Mage")
        {
            CreateBaseCharacter(status);
        }
    }
    public void ShowWeapon(WeaponStatus item)
    {
        if (item.damage > 0)
        {
            itemAttributeValues.Add(item.damage.ToString());
            itemAttributeNames.Add("<color=#ffffff> Damage</color>");
        }
        if (item.rateOfFire > 0)
        {
            itemAttributeValues.Add(item.rateOfFire.ToString());
            itemAttributeNames.Add("<color=#ffffff> Atk. Rate</color>");
        }
        if (item.criticalRate > 0)
        {
            itemAttributeValues.Add(item.criticalRate.ToString());
            itemAttributeNames.Add("% <color=#ffffff>Crit. Rate</color>");
        }
        if (item.criticalDamage > 0)
        {
            itemAttributeValues.Add(item.criticalDamage.ToString());
            itemAttributeNames.Add("% <color=#ffffff>Crit. Dmg</color>");
        }

        foreach (Transform child in inventoryWindow.transform)
        {
            if (item != null)
            {
                if (child.name == "ItemName")
                {
                    child.GetComponent<Text>().text = item.itemName + " (T" + item.itemTier + ")";
                }
                if (child.name == "FlavorText")
                {
                    child.GetComponent<Text>().text = item.flavourText;
                }
                if (child.name == "SellValue")
                {
                    child.GetComponent<Text>().text = (((item.valueInGold * item.itemTier) / 2) + item.timesUpgraded * 1000).ToString();
                }
                for (int i = 0; i < itemAttributeValues.Count; i++)
                {
                    if (i == 0 && itemAttributeValues.Count > 0)
                    {
                        if (child.name == "StatsLine1")
                        {
                            child.GetComponent<Text>().text = "+" + Convert.ToDecimal(itemAttributeValues[i]).ToString("F1") + itemAttributeNames[i];
                        }
                    }
                    else if (itemAttributeValues.Count <= 0)
                    {
                        if (child.name == "StatsLine1")
                        {
                            child.GetComponent<Text>().text = "";
                        }
                    }
                    if (i == 1 && itemAttributeValues.Count > 1)
                    {
                        if (child.name == "StatsLine2")
                        {
                            child.GetComponent<Text>().text = "+" + Convert.ToDecimal(itemAttributeValues[i]).ToString("F1") + itemAttributeNames[i];
                        }
                    }
                    else if (itemAttributeValues.Count <= 1)
                    {
                        if (child.name == "StatsLine2")
                        {
                            child.GetComponent<Text>().text = "";
                        }
                    }
                    if (i == 2 && itemAttributeValues.Count > 2)
                    {
                        if (child.name == "StatsLine3")
                        {
                            child.GetComponent<Text>().text = "+" + Convert.ToDecimal(itemAttributeValues[i]).ToString("F1") + itemAttributeNames[i];
                        }
                    }
                    else if (itemAttributeValues.Count <= 2)
                    {
                        if (child.name == "StatsLine3")
                        {
                            child.GetComponent<Text>().text = "";
                        }
                    }
                    if (i == 3 && itemAttributeValues.Count > 3)
                    {

                        if (child.name == "StatsLine4")
                        {
                            child.GetComponent<Text>().text = "+" + Convert.ToDecimal(itemAttributeValues[i]).ToString("F1") + itemAttributeNames[i];

                        }
                    }
                    else if (itemAttributeValues.Count <= 3)
                    {
                        if (child.name == "StatsLine4")
                        {
                            child.GetComponent<Text>().text = "";
                        }
                    }
                }
            }
        }
    }
    public void ShowTrinket(TrinketStatus item)
    {
        if (item.damage > 0)
        {
            itemAttributeValues.Add(item.damage.ToString());
            itemAttributeNames.Add("<color=#ffffff> Damage</color>");
        }
        if (item.rateOfFire > 0)
        {
            itemAttributeValues.Add(item.rateOfFire.ToString());
            itemAttributeNames.Add("<color=#ffffff> Atk. Rate</color>");
        }
        if (item.criticalRate > 0)
        {
            itemAttributeValues.Add(item.criticalRate.ToString());
            itemAttributeNames.Add("% <color=#ffffff>Crit. Rate</color>");
        }
        if (item.criticalDamage > 0)
        {
            itemAttributeValues.Add(item.criticalDamage.ToString());
            itemAttributeNames.Add("% <color=#ffffff>Crit. Dmg</color>");
        }
        if (item.health > 0)
        {
            itemAttributeValues.Add(item.health.ToString());
            itemAttributeNames.Add("<color=#ffffff> Health</color>");
        }
        if (item.defense > 0)
        {
            itemAttributeValues.Add(item.defense.ToString());
            itemAttributeNames.Add("<color=#ffffff> Defense</color>");
        }
        if (item.toughness > 0)
        {
            itemAttributeValues.Add(item.toughness.ToString());
            itemAttributeNames.Add(" <color=#ffffff>Toughness</color>");
        }
        if (item.evasion > 0)
        {
            itemAttributeValues.Add(item.evasion.ToString());
            itemAttributeNames.Add("% <color=#ffffff>Evasion</color>");
        }

        foreach (Transform child in inventoryWindow.transform)
        {
            if (item != null)
            {
                if (child.name == "ItemName")
                {
                    child.GetComponent<Text>().text = item.itemName + " (T" + item.itemTier + ")";
                }
                if (child.name == "FlavorText")
                {
                    child.GetComponent<Text>().text = item.flavourText;
                }
                if (child.name == "SellValue")
                {
                    child.GetComponent<Text>().text = (((item.valueInGold * item.itemTier) / 2) + item.timesUpgraded * 1000).ToString();
                }
                for (int i = 0; i < itemAttributeValues.Count; i++)
                {
                    if (i == 0 && itemAttributeValues.Count > 0)
                    {
                        if (child.name == "StatsLine1")
                        {
                            child.GetComponent<Text>().text = "+" + Convert.ToDecimal(itemAttributeValues[i]).ToString("F1") + itemAttributeNames[i];
                        }
                    }
                    else if (itemAttributeValues.Count <= 0)
                    {
                        if (child.name == "StatsLine1")
                        {
                            child.GetComponent<Text>().text = "";
                        }
                    }
                    if (i == 1 && itemAttributeValues.Count > 1)
                    {
                        if (child.name == "StatsLine2")
                        {
                            child.GetComponent<Text>().text = "+" + Convert.ToDecimal(itemAttributeValues[i]).ToString("F1") + itemAttributeNames[i];
                        }
                    }
                    else if (itemAttributeValues.Count <= 1)
                    {
                        if (child.name == "StatsLine2")
                        {
                            child.GetComponent<Text>().text = "";
                        }
                    }
                    if (i == 2 && itemAttributeValues.Count > 2)
                    {
                        if (child.name == "StatsLine3")
                        {
                            child.GetComponent<Text>().text = "+" + Convert.ToDecimal(itemAttributeValues[i]).ToString("F1") + itemAttributeNames[i];
                        }
                    }
                    else if (itemAttributeValues.Count <= 2)
                    {
                        if (child.name == "StatsLine3")
                        {
                            child.GetComponent<Text>().text = "";
                        }
                    }
                    if (i == 3 && itemAttributeValues.Count > 3)
                    {

                        if (child.name == "StatsLine4")
                        {
                            child.GetComponent<Text>().text = "+" + Convert.ToDecimal(itemAttributeValues[i]).ToString("F1") + itemAttributeNames[i];

                        }
                    }
                    else if (itemAttributeValues.Count <= 3)
                    {
                        if (child.name == "StatsLine4")
                        {
                            child.GetComponent<Text>().text = "";
                        }
                    }
                }
            }
        }
    }
    public void ShowArmor(ArmorStatus item)
    {
        if (item.health > 0)
        {
            itemAttributeValues.Add(item.health.ToString());
            itemAttributeNames.Add("<color=#ffffff> Health</color>");
        }
        if (item.defense > 0)
        {
            itemAttributeValues.Add(item.defense.ToString());
            itemAttributeNames.Add("<color=#ffffff> Defense</color>");
        }
        if (item.toughness > 0)
        {
            itemAttributeValues.Add(item.toughness.ToString());
            itemAttributeNames.Add(" <color=#ffffff>Toughness</color>");
        }
        if (item.evasion > 0)
        {
            itemAttributeValues.Add(item.evasion.ToString());
            itemAttributeNames.Add("% <color=#ffffff>Evasion</color>");
        }

        foreach (Transform child in inventoryWindow.transform)
        {
            if (item != null)
            {
                if (child.name == "ItemName")
                {
                    child.GetComponent<Text>().text = item.itemName + " (T" + item.itemTier + ")";
                }
                if (child.name == "FlavorText")
                {
                    child.GetComponent<Text>().text = item.flavourText;
                }
                if (child.name == "SellValue")
                {
                    child.GetComponent<Text>().text = (((item.valueInGold * item.itemTier) / 2) + item.timesUpgraded * 1000).ToString();
                }
                for (int i = 0; i < itemAttributeValues.Count; i++)
                {
                    if (i == 0 && itemAttributeValues.Count > 0)
                    {
                        if (child.name == "StatsLine1")
                        {
                            child.GetComponent<Text>().text = "+" + Convert.ToDecimal(itemAttributeValues[i]).ToString("F2") + itemAttributeNames[i];
                        }
                    }
                    else if (itemAttributeValues.Count <= 0)
                    {
                        if (child.name == "StatsLine1")
                        {
                            child.GetComponent<Text>().text = "";
                        }
                    }
                    if (i == 1 && itemAttributeValues.Count > 1)
                    {
                        if (child.name == "StatsLine2")
                        {
                            child.GetComponent<Text>().text = "+" + Convert.ToDecimal(itemAttributeValues[i]).ToString("F2") + itemAttributeNames[i];
                        }
                    }
                    else if (itemAttributeValues.Count <= 1)
                    {
                        if (child.name == "StatsLine2")
                        {
                            child.GetComponent<Text>().text = "";
                        }
                    }
                    if (i == 2 && itemAttributeValues.Count > 2)
                    {
                        if (child.name == "StatsLine3")
                        {
                            child.GetComponent<Text>().text = "+" + Convert.ToDecimal(itemAttributeValues[i]).ToString("F2") + itemAttributeNames[i];
                        }
                    }
                    else if (itemAttributeValues.Count <= 2)
                    {
                        if (child.name == "StatsLine3")
                        {
                            child.GetComponent<Text>().text = "";
                        }
                    }
                    if (i == 3 && itemAttributeValues.Count > 3)
                    {

                        if (child.name == "StatsLine4")
                        {
                            child.GetComponent<Text>().text = "+" + Convert.ToDecimal(itemAttributeValues[i]).ToString("F2") + itemAttributeNames[i];

                        }
                    }
                    else if (itemAttributeValues.Count <= 3)
                    {
                        if (child.name == "StatsLine4")
                        {
                            child.GetComponent<Text>().text = "";
                        }
                    }
                }
            }
        }
    }

    public void ShowItemInSlot()
    {
        if (sellConfirmation.activeSelf == true)
        {
            sellConfirmation.SetActive(false);
        }
        if (upgradeConfirmation.activeSelf == true)
        {
            upgradeConfirmation.SetActive(false);
        }
        if (EventSystem.current.currentSelectedGameObject.transform.childCount > 0)
        {
            selectedItem = EventSystem.current.currentSelectedGameObject.transform.GetChild(0);
            if (selectedItem.parent.name == "WeaponSlot" || selectedItem.parent.name == "ArmorSlot" || selectedItem.parent.name == "AccessorySlot1" || selectedItem.parent.name == "AccessorySlot2")
            {

                equipButton.SetActive(false);
                unequipButton.SetActive(true);
            }
            else
            {

                equipButton.SetActive(true);
                unequipButton.SetActive(false);
            }
        }
        else
        {
            selectedItem = null;
        }
        inventoryWindow.SetActive(true);

        itemAttributeNames.Clear();
        itemAttributeValues.Clear();
        var weapon = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<WeaponStatus>();
        var armor = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<ArmorStatus>();
        var trinket = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TrinketStatus>();

        if (weapon != null)
        {
            ShowWeapon(weapon);
        }
        else if (armor != null)
        {
            ShowArmor(armor);
        }
        else if (trinket != null)
        {
            ShowTrinket(trinket);
        }
        else if (armor == null && weapon == null)
        {
            foreach (Transform child in inventoryWindow.transform)
            {
                if (child.name == "ItemName")
                {
                    child.GetComponent<Text>().text = "<color=#FFFFFF>(Empty)</color>";
                }
                if (child.name == "StatsLine1")
                {
                    child.GetComponent<Text>().text = "";
                }

                if (child.name == "StatsLine2")
                {
                    child.GetComponent<Text>().text = "";
                }
                if (child.name == "StatsLine3")
                {
                    child.GetComponent<Text>().text = "";
                }

                if (child.name == "StatsLine4")
                {
                    child.GetComponent<Text>().text = "";
                }

                if (child.name == "FlavorText")
                {
                    child.GetComponent<Text>().text = "";
                }
                if (child.name == "SellValue")
                {
                    child.GetComponent<Text>().text = "";
                }
            }
        }
    }

    public void CancelOperation()
    {
        if (sellConfirmation.activeSelf)
        {
            sellConfirmation.SetActive(false);
        }
        if (upgradeConfirmation.activeSelf)
        {
            upgradeConfirmation.SetActive(false);
        }
        if (shopConfirmation.activeSelf)
        {
            shopConfirmation.SetActive(false);
        }
        if (itemBoughtWindow.activeSelf)
        {
            itemBoughtWindow.SetActive(false);
            Destroy(itemBought.transform.GetChild(0).gameObject);
        }
    }
    public void SellItem()
    {
        if (selectedItem != null)
        {
            sellConfirmation.SetActive(true);
        }
    }

    public void UpgradeItem()
    {

        int timesUpgraded = 0;
        int itemTier = 1;
        if (selectedItem != null)
        {
            if (selectedItem.GetComponent<WeaponStatus>() != null)
            {
                timesUpgraded = selectedItem.GetComponent<WeaponStatus>().timesUpgraded;
                itemTier = selectedItem.GetComponent<WeaponStatus>().itemTier;
            }
            if (selectedItem.GetComponent<ArmorStatus>() != null)
            {
                timesUpgraded = selectedItem.GetComponent<ArmorStatus>().timesUpgraded;
                itemTier = selectedItem.GetComponent<ArmorStatus>().itemTier;
            }
            if (selectedItem.GetComponent<TrinketStatus>() != null)
            {
                timesUpgraded = selectedItem.GetComponent<TrinketStatus>().timesUpgraded;
                itemTier = selectedItem.GetComponent<TrinketStatus>().itemTier;
            }
            if (itemTier < 13)
            {
                switch (timesUpgraded)
                {
                    case 0:
                        upgradeCost.text = "2000";
                        upgradeRequirement.text = "Upgrade this item for:";
                        break;
                    case 1:
                        upgradeCost.text = "4000";
                        upgradeRequirement.text = "Upgrade this item for:";
                        break;
                    case 2:
                        upgradeCost.text = "6000";
                        upgradeRequirement.text = "Requires completing stage " + (itemTier - timesUpgraded);
                        break;
                    case 3:
                        upgradeCost.text = "10000";
                        upgradeRequirement.text = "Requires completing stage " + (itemTier - timesUpgraded + 1);
                        break;
                    case 4:
                        upgradeCost.text = "16000";
                        upgradeRequirement.text = "Requires completing stage " + (itemTier - timesUpgraded + 2);
                        break;
                    case 5:
                        upgradeCost.text = "26000";
                        upgradeRequirement.text = "Requires completing stage " + (itemTier - timesUpgraded + 3);
                        break;
                    case 6:
                        upgradeCost.text = "42000";
                        upgradeRequirement.text = "Requires completing stage " + (itemTier - timesUpgraded + 4);
                        break;
                    case 7:
                        upgradeCost.text = "68000";
                        upgradeRequirement.text = "Requires completing stage " + (itemTier - timesUpgraded + 5);
                        break;
                    case 8:
                        upgradeCost.text = "110000";
                        upgradeRequirement.text = "Requires completing stage " + (itemTier - timesUpgraded + 6);
                        break;
                    case 9:
                        upgradeCost.text = "178000";
                        upgradeRequirement.text = "Requires completing stage " + (itemTier - timesUpgraded + 7);
                        break;
                    case 10:
                        upgradeCost.text = "288000";
                        upgradeRequirement.text = "Requires completing stage " + (itemTier - timesUpgraded + 8);
                        break;
                    case 11:
                        upgradeCost.text = "466000";
                        upgradeRequirement.text = "Requires completing stage " + (itemTier - timesUpgraded + 8);

                        break;
                    default:
                        upgradeCost.text = "0";
                        upgradeRequirement.text = "Item fully upgraded";
                        break;
                }
            }
            else
            {
                upgradeCost.text = "0";
                upgradeRequirement.text = "Item fully upgraded";
            }


            upgradeConfirmation.SetActive(true);
        }

    }


    public void ConfirmUpgrade()
    {
        if (selectedItem != null && Convert.ToInt32(upgradeCost.text) > 0)
        {
            if (selectedItem.GetComponent<WeaponStatus>() != null && Convert.ToInt32(upgradeCost.text) <= playerObject.GetComponent<PlayerStatus>().playerGold)
            {
                if (selectedItem.parent.name == "WeaponSlot")
                {
                    UnequipWeapon();
                    selectedItem.GetComponent<WeaponStatus>().damage -= RoundValuesForUpgradeInt(selectedItem.GetComponent<WeaponStatus>().damage, selectedItem.GetComponent<WeaponStatus>().itemTier);
                    selectedItem.GetComponent<WeaponStatus>().damage += (selectedItem.GetComponent<WeaponStatus>().damage / (selectedItem.GetComponent<WeaponStatus>().itemTier));
                    selectedItem.GetComponent<WeaponStatus>().rateOfFire -= RoundValuesForUpgradeFloat(selectedItem.GetComponent<WeaponStatus>().rateOfFire, selectedItem.GetComponent<WeaponStatus>().itemTier);
                    selectedItem.GetComponent<WeaponStatus>().rateOfFire += (selectedItem.GetComponent<WeaponStatus>().rateOfFire / (selectedItem.GetComponent<WeaponStatus>().itemTier));
                    selectedItem.GetComponent<WeaponStatus>().criticalRate -= RoundValuesForUpgradeInt(selectedItem.GetComponent<WeaponStatus>().criticalRate, selectedItem.GetComponent<WeaponStatus>().itemTier);
                    selectedItem.GetComponent<WeaponStatus>().criticalRate += (selectedItem.GetComponent<WeaponStatus>().criticalRate / (selectedItem.GetComponent<WeaponStatus>().itemTier));
                    selectedItem.GetComponent<WeaponStatus>().criticalDamage -= RoundValuesForUpgradeInt(selectedItem.GetComponent<WeaponStatus>().criticalDamage, selectedItem.GetComponent<WeaponStatus>().itemTier);
                    selectedItem.GetComponent<WeaponStatus>().criticalDamage += (selectedItem.GetComponent<WeaponStatus>().criticalDamage / (selectedItem.GetComponent<WeaponStatus>().itemTier));
                    selectedItem.GetComponent<WeaponStatus>().timesUpgraded++;
                    selectedItem.GetComponent<WeaponStatus>().itemTier++;
                    playerObject.GetComponent<PlayerStatus>().damage += playerObject.GetComponent<PlayerStatus>().equipedWeapon.GetComponent<WeaponStatus>().damage;
                    playerObject.GetComponent<PlayerStatus>().fireRate += playerObject.GetComponent<PlayerStatus>().equipedWeapon.GetComponent<WeaponStatus>().rateOfFire;
                    playerObject.GetComponent<PlayerStatus>().playerCritChance += playerObject.GetComponent<PlayerStatus>().equipedWeapon.GetComponent<WeaponStatus>().criticalRate;
                    playerObject.GetComponent<PlayerStatus>().playerCritDamage += playerObject.GetComponent<PlayerStatus>().equipedWeapon.GetComponent<WeaponStatus>().criticalDamage;
                    if (sellConfirmation.activeSelf == true)
                    {
                        sellConfirmation.SetActive(false);
                    }
                    if (upgradeConfirmation.activeSelf == true)
                    {
                        upgradeConfirmation.SetActive(false);
                    }
                }
                else
                {

                    selectedItem.GetComponent<WeaponStatus>().damage -= RoundValuesForUpgradeInt(selectedItem.GetComponent<WeaponStatus>().damage, selectedItem.GetComponent<WeaponStatus>().itemTier);
                    selectedItem.GetComponent<WeaponStatus>().damage += (selectedItem.GetComponent<WeaponStatus>().damage / (selectedItem.GetComponent<WeaponStatus>().itemTier));
                    selectedItem.GetComponent<WeaponStatus>().rateOfFire -= RoundValuesForUpgradeFloat(selectedItem.GetComponent<WeaponStatus>().rateOfFire, selectedItem.GetComponent<WeaponStatus>().itemTier);
                    selectedItem.GetComponent<WeaponStatus>().rateOfFire += (selectedItem.GetComponent<WeaponStatus>().rateOfFire / (selectedItem.GetComponent<WeaponStatus>().itemTier));
                    selectedItem.GetComponent<WeaponStatus>().criticalRate -= RoundValuesForUpgradeInt(selectedItem.GetComponent<WeaponStatus>().criticalRate, selectedItem.GetComponent<WeaponStatus>().itemTier);
                    selectedItem.GetComponent<WeaponStatus>().criticalRate += (selectedItem.GetComponent<WeaponStatus>().criticalRate / (selectedItem.GetComponent<WeaponStatus>().itemTier));
                    selectedItem.GetComponent<WeaponStatus>().criticalDamage -= RoundValuesForUpgradeInt(selectedItem.GetComponent<WeaponStatus>().criticalDamage, selectedItem.GetComponent<WeaponStatus>().itemTier);
                    selectedItem.GetComponent<WeaponStatus>().criticalDamage += (selectedItem.GetComponent<WeaponStatus>().criticalDamage / (selectedItem.GetComponent<WeaponStatus>().itemTier));
                    selectedItem.GetComponent<WeaponStatus>().timesUpgraded++;
                    selectedItem.GetComponent<WeaponStatus>().itemTier++;
                    if (sellConfirmation.activeSelf == true)
                    {
                        sellConfirmation.SetActive(false);
                    }
                    if (upgradeConfirmation.activeSelf == true)
                    {
                        upgradeConfirmation.SetActive(false);
                    }
                }
                playerObject.GetComponent<PlayerStatus>().playerGold -= Convert.ToInt32(upgradeCost.text);
                inventoryWindow.SetActive(false);
            }
            else
            {
                upgradeRequirement.text = "<color=#FF0000>Not enough gold</color>";
            }
            if (selectedItem.GetComponent<ArmorStatus>() != null && Convert.ToInt32(upgradeCost.text) <= playerObject.GetComponent<PlayerStatus>().playerGold)
            {
                if (selectedItem.parent.name == "ArmorSlot")
                {
                    UnequipArmor();
                    selectedItem.GetComponent<ArmorStatus>().health -= RoundValuesForUpgradeInt(selectedItem.GetComponent<ArmorStatus>().health, selectedItem.GetComponent<ArmorStatus>().itemTier);
                    selectedItem.GetComponent<ArmorStatus>().health += (selectedItem.GetComponent<ArmorStatus>().health / (selectedItem.GetComponent<ArmorStatus>().itemTier));
                    selectedItem.GetComponent<ArmorStatus>().defense -= RoundValuesForUpgradeInt(selectedItem.GetComponent<ArmorStatus>().defense, selectedItem.GetComponent<ArmorStatus>().itemTier);
                    selectedItem.GetComponent<ArmorStatus>().defense += (selectedItem.GetComponent<ArmorStatus>().defense / (selectedItem.GetComponent<ArmorStatus>().itemTier));
                    selectedItem.GetComponent<ArmorStatus>().toughness -= RoundValuesForUpgradeInt(selectedItem.GetComponent<ArmorStatus>().toughness, selectedItem.GetComponent<ArmorStatus>().itemTier);
                    selectedItem.GetComponent<ArmorStatus>().toughness += (selectedItem.GetComponent<ArmorStatus>().toughness / (selectedItem.GetComponent<ArmorStatus>().itemTier));
                    selectedItem.GetComponent<ArmorStatus>().evasion -= RoundValuesForUpgradeFloat(selectedItem.GetComponent<ArmorStatus>().evasion, selectedItem.GetComponent<ArmorStatus>().itemTier);
                    selectedItem.GetComponent<ArmorStatus>().evasion += (selectedItem.GetComponent<ArmorStatus>().evasion / (selectedItem.GetComponent<ArmorStatus>().itemTier));
                    selectedItem.GetComponent<ArmorStatus>().timesUpgraded++;
                    selectedItem.GetComponent<ArmorStatus>().itemTier++;
                    playerObject.GetComponent<PlayerStatus>().maxHealth += playerObject.GetComponent<PlayerStatus>().equipedArmor.GetComponent<ArmorStatus>().health;
                    playerObject.GetComponent<PlayerStatus>().defense += playerObject.GetComponent<PlayerStatus>().equipedArmor.GetComponent<ArmorStatus>().defense;
                    playerObject.GetComponent<PlayerStatus>().toughness += playerObject.GetComponent<PlayerStatus>().equipedArmor.GetComponent<ArmorStatus>().toughness;
                    playerObject.GetComponent<PlayerStatus>().evasion += playerObject.GetComponent<PlayerStatus>().equipedArmor.GetComponent<ArmorStatus>().evasion;
                    if (sellConfirmation.activeSelf == true)
                    {
                        sellConfirmation.SetActive(false);
                    }
                    if (upgradeConfirmation.activeSelf == true)
                    {
                        upgradeConfirmation.SetActive(false);
                    }
                }
                else
                {
                    selectedItem.GetComponent<ArmorStatus>().health -= RoundValuesForUpgradeInt(selectedItem.GetComponent<ArmorStatus>().health, selectedItem.GetComponent<ArmorStatus>().itemTier);
                    selectedItem.GetComponent<ArmorStatus>().health += (selectedItem.GetComponent<ArmorStatus>().health / (selectedItem.GetComponent<ArmorStatus>().itemTier));
                    selectedItem.GetComponent<ArmorStatus>().defense -= RoundValuesForUpgradeInt(selectedItem.GetComponent<ArmorStatus>().defense, selectedItem.GetComponent<ArmorStatus>().itemTier);
                    selectedItem.GetComponent<ArmorStatus>().defense += (selectedItem.GetComponent<ArmorStatus>().defense / (selectedItem.GetComponent<ArmorStatus>().itemTier));
                    selectedItem.GetComponent<ArmorStatus>().toughness -= RoundValuesForUpgradeInt(selectedItem.GetComponent<ArmorStatus>().toughness, selectedItem.GetComponent<ArmorStatus>().itemTier);
                    selectedItem.GetComponent<ArmorStatus>().toughness += (selectedItem.GetComponent<ArmorStatus>().toughness / (selectedItem.GetComponent<ArmorStatus>().itemTier));
                    selectedItem.GetComponent<ArmorStatus>().evasion -= RoundValuesForUpgradeFloat(selectedItem.GetComponent<ArmorStatus>().evasion, selectedItem.GetComponent<ArmorStatus>().itemTier);
                    selectedItem.GetComponent<ArmorStatus>().evasion += (selectedItem.GetComponent<ArmorStatus>().evasion / (selectedItem.GetComponent<ArmorStatus>().itemTier));
                    selectedItem.GetComponent<ArmorStatus>().timesUpgraded++;
                    selectedItem.GetComponent<ArmorStatus>().itemTier++;
                    if (sellConfirmation.activeSelf == true)
                    {
                        sellConfirmation.SetActive(false);
                    }
                    if (upgradeConfirmation.activeSelf == true)
                    {
                        upgradeConfirmation.SetActive(false);
                    }
                }

                playerObject.GetComponent<PlayerStatus>().playerGold -= Convert.ToInt32(upgradeCost.text);
                inventoryWindow.SetActive(false);
            }
            else
            {
                upgradeRequirement.text = "<color=#FF0000>Not enough gold</color>";
            }
            if (selectedItem.GetComponent<TrinketStatus>() != null && Convert.ToInt32(upgradeCost.text) <= playerObject.GetComponent<PlayerStatus>().playerGold)
            {
                if (selectedItem.parent.name == "AccessorySlot1")
                {
                    UnequipTrinket1();
                    selectedItem.GetComponent<TrinketStatus>().health -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().health, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().health += (selectedItem.GetComponent<TrinketStatus>().health / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().defense -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().defense, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().defense += (selectedItem.GetComponent<TrinketStatus>().defense / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().toughness -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().toughness, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().toughness += (selectedItem.GetComponent<TrinketStatus>().toughness / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().evasion -= RoundValuesForUpgradeFloat(selectedItem.GetComponent<TrinketStatus>().evasion, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().evasion += (selectedItem.GetComponent<TrinketStatus>().evasion / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().damage -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().damage, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().damage += (selectedItem.GetComponent<TrinketStatus>().damage / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().rateOfFire -= RoundValuesForUpgradeFloat(selectedItem.GetComponent<TrinketStatus>().rateOfFire, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().rateOfFire += (selectedItem.GetComponent<TrinketStatus>().rateOfFire / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().criticalRate -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().criticalRate, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().criticalRate += (selectedItem.GetComponent<TrinketStatus>().criticalRate / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().criticalDamage -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().criticalDamage, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().criticalDamage += (selectedItem.GetComponent<TrinketStatus>().criticalDamage / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().timesUpgraded++;
                    selectedItem.GetComponent<TrinketStatus>().itemTier++;
                    playerObject.GetComponent<PlayerStatus>().maxHealth += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().health;
                    playerObject.GetComponent<PlayerStatus>().defense += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().defense;
                    playerObject.GetComponent<PlayerStatus>().toughness += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().toughness;
                    playerObject.GetComponent<PlayerStatus>().evasion += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().evasion;
                    if (sellConfirmation.activeSelf == true)
                    {
                        sellConfirmation.SetActive(false);
                    }
                    if (upgradeConfirmation.activeSelf == true)
                    {
                        upgradeConfirmation.SetActive(false);
                    }
                }
                else if (selectedItem.parent.name == "AccessorySlot2")
                {
                    UnequipTrinket2();
                    selectedItem.GetComponent<TrinketStatus>().health -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().health, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().health += (selectedItem.GetComponent<TrinketStatus>().health / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().defense -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().defense, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().defense += (selectedItem.GetComponent<TrinketStatus>().defense / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().toughness -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().toughness, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().toughness += (selectedItem.GetComponent<TrinketStatus>().toughness / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().evasion -= RoundValuesForUpgradeFloat(selectedItem.GetComponent<TrinketStatus>().evasion, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().evasion += (selectedItem.GetComponent<TrinketStatus>().evasion / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().damage -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().damage, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().damage += (selectedItem.GetComponent<TrinketStatus>().damage / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().rateOfFire -= RoundValuesForUpgradeFloat(selectedItem.GetComponent<TrinketStatus>().rateOfFire, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().rateOfFire += (selectedItem.GetComponent<TrinketStatus>().rateOfFire / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().criticalRate -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().criticalRate, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().criticalRate += (selectedItem.GetComponent<TrinketStatus>().criticalRate / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().criticalDamage -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().criticalDamage, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().criticalDamage += (selectedItem.GetComponent<TrinketStatus>().criticalDamage / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().timesUpgraded++;
                    selectedItem.GetComponent<TrinketStatus>().itemTier++;
                    playerObject.GetComponent<PlayerStatus>().maxHealth += playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().health;
                    playerObject.GetComponent<PlayerStatus>().defense += playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().defense;
                    playerObject.GetComponent<PlayerStatus>().toughness += playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().toughness;
                    playerObject.GetComponent<PlayerStatus>().evasion += playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().evasion;
                    if (sellConfirmation.activeSelf == true)
                    {
                        sellConfirmation.SetActive(false);
                    }
                    if (upgradeConfirmation.activeSelf == true)
                    {
                        upgradeConfirmation.SetActive(false);
                    }

                }
                else
                {
                    selectedItem.GetComponent<TrinketStatus>().health -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().health, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().health += (selectedItem.GetComponent<TrinketStatus>().health / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().defense -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().defense, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().defense += (selectedItem.GetComponent<TrinketStatus>().defense / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().toughness -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().toughness, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().toughness += (selectedItem.GetComponent<TrinketStatus>().toughness / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().evasion -= RoundValuesForUpgradeFloat(selectedItem.GetComponent<TrinketStatus>().evasion, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().evasion += (selectedItem.GetComponent<TrinketStatus>().evasion / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().damage -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().damage, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().damage += (selectedItem.GetComponent<TrinketStatus>().damage / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().rateOfFire -= RoundValuesForUpgradeFloat(selectedItem.GetComponent<TrinketStatus>().rateOfFire, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().rateOfFire += (selectedItem.GetComponent<TrinketStatus>().rateOfFire / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().criticalRate -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().criticalRate, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().criticalRate += (selectedItem.GetComponent<TrinketStatus>().criticalRate / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().criticalDamage -= RoundValuesForUpgradeInt(selectedItem.GetComponent<TrinketStatus>().criticalDamage, selectedItem.GetComponent<TrinketStatus>().itemTier);
                    selectedItem.GetComponent<TrinketStatus>().criticalDamage += (selectedItem.GetComponent<TrinketStatus>().criticalDamage / (selectedItem.GetComponent<TrinketStatus>().itemTier));
                    selectedItem.GetComponent<TrinketStatus>().timesUpgraded++;
                    selectedItem.GetComponent<TrinketStatus>().itemTier++;
                    if (sellConfirmation.activeSelf == true)
                    {
                        sellConfirmation.SetActive(false);
                    }
                    if (upgradeConfirmation.activeSelf == true)
                    {
                        upgradeConfirmation.SetActive(false);
                    }
                }
                playerObject.GetComponent<PlayerStatus>().playerGold -= Convert.ToInt32(upgradeCost.text);
                inventoryWindow.SetActive(false);
            }
            else
            {
                upgradeRequirement.text = "<color=#FF0000>Not enough gold</color>";
            }

        }
        else
        {
            upgradeRequirement.text = "<color=#FF0000>Item fully upgraded</color>";
        }
        UpdateEverything();
    }

    public int RoundValuesForUpgradeInt(float value1, int itemTier)
    {
        if (value1 % itemTier != 0)
        {
            return Convert.ToInt32(value1 % itemTier);
        }
        else
        {
            return 0;
        }
    }

    public float RoundValuesForUpgradeFloat(float value1, int itemTier)
    {
        if ((value1 * 10) % itemTier != 0)
        {
            return Convert.ToInt32(((value1 * 100) % itemTier) / 100);
        }
        else
        {
            return 0;
        }
    }

    public void ConfirmSale()
    {
        if (selectedItem != null)
        {
            int goldValue = 0;
            if (selectedItem.GetComponent<WeaponStatus>() != null)
            {
                if (selectedItem.parent.name == "WeaponSlot")
                {
                    UnequipWeapon();
                }
                goldValue = (((selectedItem.GetComponent<WeaponStatus>().valueInGold * selectedItem.GetComponent<WeaponStatus>().itemTier) / 2) + selectedItem.GetComponent<WeaponStatus>().timesUpgraded * 1000);
                //selectedItem.GetComponent<WeaponStatus>().valueInGold / 2;
            }
            if (selectedItem.GetComponent<ArmorStatus>() != null)
            {
                if (selectedItem.parent.name == "ArmorSlot")
                {
                    UnequipArmor();
                }
                goldValue = (((selectedItem.GetComponent<ArmorStatus>().valueInGold * selectedItem.GetComponent<ArmorStatus>().itemTier) / 2) + selectedItem.GetComponent<ArmorStatus>().timesUpgraded * 1000);
            }
            if (selectedItem.GetComponent<TrinketStatus>() != null)
            {
                if (selectedItem.parent.name == "AccessorySlot1")
                {
                    UnequipTrinket1();
                }
                if (selectedItem.parent.name == "AccessorySlot2")
                {
                    UnequipTrinket2();
                }
                goldValue = (((selectedItem.GetComponent<TrinketStatus>().valueInGold * selectedItem.GetComponent<TrinketStatus>().itemTier) / 2) + selectedItem.GetComponent<TrinketStatus>().timesUpgraded * 1000);
            }

            playerObject.GetComponent<PlayerStatus>().playerGold += goldValue;

            Destroy(selectedItem.gameObject);
            inventoryWindow.SetActive(false);
        }
        if (sellConfirmation.activeSelf == true)
        {
            sellConfirmation.SetActive(false);
        }
        if (upgradeConfirmation.activeSelf == true)
        {
            upgradeConfirmation.SetActive(false);
        }
        UpdateEverything();
    }
    public void UnequipItem()
    {
        if (selectedItem != null)
        {
            for (var i = 0; i < 16; i++)
            {
                if (playerObject.GetComponent<PlayerStatus>().menuInventorySlots[i] == null)
                {
                    playerObject.GetComponent<PlayerStatus>().menuInventorySlots[i] = Instantiate(selectedItem.gameObject);
                    if (selectedItem.parent.name == "WeaponSlot" && selectedItem.gameObject.GetComponent<WeaponStatus>() != null)
                    {
                        UnequipWeapon();
                    }
                    if (selectedItem.parent.name == "ArmorSlot" && selectedItem.gameObject.GetComponent<ArmorStatus>() != null)
                    {
                        UnequipArmor();
                    }
                    if (selectedItem.parent.name == "AccessorySlot1" && selectedItem.gameObject.GetComponent<TrinketStatus>() != null)
                    {

                        UnequipTrinket1();
                    }
                    if (selectedItem.parent.name == "AccessorySlot2" && selectedItem.gameObject.GetComponent<TrinketStatus>() != null)
                    {
                        UnequipTrinket2();
                    }
                    Destroy(selectedItem.gameObject);


                    foreach (Transform slot in inventoryMenuSlots.transform)
                    {
                        if ((i + 1).ToString() == slot.name)
                        {
                            playerObject.GetComponent<PlayerStatus>().menuInventorySlots[i].transform.position = slot.transform.position;
                            playerObject.GetComponent<PlayerStatus>().menuInventorySlots[i].transform.localScale = new Vector2(1, 1);
                            playerObject.GetComponent<PlayerStatus>().menuInventorySlots[i].transform.SetParent(slot);
                        }
                    }
                    playerObject.GetComponent<PlayerStatus>().menuInventorySlots[i].name = "Slot " + i;


                    break;
                }
            }
        }
        UpdateEverything();
        inventoryWindow.SetActive(false);
    }
    public void EquipItem()
    {
        if (selectedItem != null)
        {
            if (selectedItem.GetComponent<WeaponStatus>() != null)
            {
                if (playerObject.GetComponent<PlayerStatus>().equipedWeapon != null)
                {
                    UnequipWeapon();
                    Destroy(playerObject.GetComponent<PlayerStatus>().equipedWeapon);
                    Destroy(selectedItem.gameObject);

                    GameObject replacedWeapon = Instantiate(playerObject.GetComponent<PlayerStatus>().equipedWeapon, selectedItem.position, Quaternion.identity, selectedItem.parent.transform);
                    playerObject.GetComponent<PlayerStatus>().menuInventorySlots[Convert.ToInt32(selectedItem.parent.name) - 1] = replacedWeapon;

                }
                playerObject.GetComponent<PlayerStatus>().equipedWeapon = Instantiate(selectedItem.gameObject, weaponSlot.transform.position, Quaternion.identity, weaponSlot.transform);
                playerObject.GetComponent<PlayerStatus>().damage += playerObject.GetComponent<PlayerStatus>().equipedWeapon.GetComponent<WeaponStatus>().damage;
                playerObject.GetComponent<PlayerStatus>().fireRate += playerObject.GetComponent<PlayerStatus>().equipedWeapon.GetComponent<WeaponStatus>().rateOfFire;
                playerObject.GetComponent<PlayerStatus>().playerCritChance += playerObject.GetComponent<PlayerStatus>().equipedWeapon.GetComponent<WeaponStatus>().criticalRate;
                playerObject.GetComponent<PlayerStatus>().playerCritDamage += playerObject.GetComponent<PlayerStatus>().equipedWeapon.GetComponent<WeaponStatus>().criticalDamage;
                Destroy(selectedItem.gameObject);
                inventoryWindow.SetActive(false);
            }

            if (selectedItem.GetComponent<ArmorStatus>() != null)
            {
                if (playerObject.GetComponent<PlayerStatus>().equipedArmor != null)
                {
                    UnequipArmor();
                    Destroy(playerObject.GetComponent<PlayerStatus>().equipedArmor);
                    Destroy(selectedItem.gameObject);

                    GameObject replacedWeapon = Instantiate(playerObject.GetComponent<PlayerStatus>().equipedArmor, selectedItem.position, Quaternion.identity, selectedItem.parent.transform);
                    playerObject.GetComponent<PlayerStatus>().menuInventorySlots[Convert.ToInt32(selectedItem.parent.name) - 1] = replacedWeapon;

                }
                playerObject.GetComponent<PlayerStatus>().equipedArmor = Instantiate(selectedItem.gameObject, armorSlot.transform.position, Quaternion.identity, armorSlot.transform);
                playerObject.GetComponent<PlayerStatus>().maxHealth += playerObject.GetComponent<PlayerStatus>().equipedArmor.GetComponent<ArmorStatus>().health;
                playerObject.GetComponent<PlayerStatus>().defense += playerObject.GetComponent<PlayerStatus>().equipedArmor.GetComponent<ArmorStatus>().defense;
                playerObject.GetComponent<PlayerStatus>().toughness += playerObject.GetComponent<PlayerStatus>().equipedArmor.GetComponent<ArmorStatus>().toughness;
                playerObject.GetComponent<PlayerStatus>().evasion += playerObject.GetComponent<PlayerStatus>().equipedArmor.GetComponent<ArmorStatus>().evasion;
                Destroy(selectedItem.gameObject);
                inventoryWindow.SetActive(false);
            }
            if (selectedItem.GetComponent<TrinketStatus>() != null)
            {
                if (playerObject.GetComponent<PlayerStatus>().equipedTrinket1 != null)
                {
                    if (playerObject.GetComponent<PlayerStatus>().equipedTrinket2 != null)
                    {
                        UnequipTrinket1();
                        Destroy(playerObject.GetComponent<PlayerStatus>().equipedTrinket1);
                        Destroy(selectedItem.gameObject);

                        GameObject replacedTrinket = Instantiate(playerObject.GetComponent<PlayerStatus>().equipedTrinket1, selectedItem.position, Quaternion.identity, selectedItem.parent.transform);
                        playerObject.GetComponent<PlayerStatus>().menuInventorySlots[Convert.ToInt32(selectedItem.parent.name) - 1] = replacedTrinket;

                        playerObject.GetComponent<PlayerStatus>().equipedTrinket1 = Instantiate(selectedItem.gameObject, trinket1Slot.transform.position, Quaternion.identity, trinket1Slot.transform);
                        playerObject.GetComponent<PlayerStatus>().damage += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().damage;
                        playerObject.GetComponent<PlayerStatus>().fireRate += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().rateOfFire;
                        playerObject.GetComponent<PlayerStatus>().playerCritChance += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().criticalRate;
                        playerObject.GetComponent<PlayerStatus>().playerCritDamage += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().criticalDamage;
                        playerObject.GetComponent<PlayerStatus>().maxHealth += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().health;
                        playerObject.GetComponent<PlayerStatus>().defense += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().defense;
                        playerObject.GetComponent<PlayerStatus>().toughness += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().toughness;
                        playerObject.GetComponent<PlayerStatus>().evasion += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().evasion;
                        Destroy(selectedItem.gameObject);
                        inventoryWindow.SetActive(false);
                    }
                    else
                    {
                        playerObject.GetComponent<PlayerStatus>().equipedTrinket2 = Instantiate(selectedItem.gameObject, trinket2Slot.transform.position, Quaternion.identity, trinket2Slot.transform);
                        playerObject.GetComponent<PlayerStatus>().damage += playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().damage;
                        playerObject.GetComponent<PlayerStatus>().fireRate += playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().rateOfFire;
                        playerObject.GetComponent<PlayerStatus>().playerCritChance += playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().criticalRate;
                        playerObject.GetComponent<PlayerStatus>().playerCritDamage += playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().criticalDamage;
                        playerObject.GetComponent<PlayerStatus>().maxHealth += playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().health;
                        playerObject.GetComponent<PlayerStatus>().defense += playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().defense;
                        playerObject.GetComponent<PlayerStatus>().toughness += playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().toughness;
                        playerObject.GetComponent<PlayerStatus>().evasion += playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().evasion;
                        Destroy(selectedItem.gameObject);
                        inventoryWindow.SetActive(false);
                    }

                }
                else
                {
                    playerObject.GetComponent<PlayerStatus>().equipedTrinket1 = Instantiate(selectedItem.gameObject, trinket1Slot.transform.position, Quaternion.identity, trinket1Slot.transform);
                    playerObject.GetComponent<PlayerStatus>().damage += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().damage;
                    playerObject.GetComponent<PlayerStatus>().fireRate += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().rateOfFire;
                    playerObject.GetComponent<PlayerStatus>().playerCritChance += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().criticalRate;
                    playerObject.GetComponent<PlayerStatus>().playerCritDamage += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().criticalDamage;
                    playerObject.GetComponent<PlayerStatus>().maxHealth += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().health;
                    playerObject.GetComponent<PlayerStatus>().defense += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().defense;
                    playerObject.GetComponent<PlayerStatus>().toughness += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().toughness;
                    playerObject.GetComponent<PlayerStatus>().evasion += playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().evasion;
                    Destroy(selectedItem.gameObject);
                    inventoryWindow.SetActive(false);
                }
            }
        }


        UpdateEverything();
    }

    void UnequipWeapon()
    {
        playerObject.GetComponent<PlayerStatus>().damage -= playerObject.GetComponent<PlayerStatus>().equipedWeapon.GetComponent<WeaponStatus>().damage;
        playerObject.GetComponent<PlayerStatus>().fireRate -= playerObject.GetComponent<PlayerStatus>().equipedWeapon.GetComponent<WeaponStatus>().rateOfFire;
        playerObject.GetComponent<PlayerStatus>().playerCritChance -= playerObject.GetComponent<PlayerStatus>().equipedWeapon.GetComponent<WeaponStatus>().criticalRate;
        playerObject.GetComponent<PlayerStatus>().playerCritDamage -= playerObject.GetComponent<PlayerStatus>().equipedWeapon.GetComponent<WeaponStatus>().criticalDamage;
    }
    void UnequipArmor()
    {
        playerObject.GetComponent<PlayerStatus>().maxHealth -= playerObject.GetComponent<PlayerStatus>().equipedArmor.GetComponent<ArmorStatus>().health;
        playerObject.GetComponent<PlayerStatus>().defense -= playerObject.GetComponent<PlayerStatus>().equipedArmor.GetComponent<ArmorStatus>().defense;
        playerObject.GetComponent<PlayerStatus>().toughness -= playerObject.GetComponent<PlayerStatus>().equipedArmor.GetComponent<ArmorStatus>().toughness;
        playerObject.GetComponent<PlayerStatus>().evasion -= playerObject.GetComponent<PlayerStatus>().equipedArmor.GetComponent<ArmorStatus>().evasion;
    }
    void UnequipTrinket1()
    {
        playerObject.GetComponent<PlayerStatus>().damage -= playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().damage;
        playerObject.GetComponent<PlayerStatus>().fireRate -= playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().rateOfFire;
        playerObject.GetComponent<PlayerStatus>().playerCritChance -= playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().criticalRate;
        playerObject.GetComponent<PlayerStatus>().playerCritDamage -= playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().criticalDamage;
        playerObject.GetComponent<PlayerStatus>().maxHealth -= playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().health;
        playerObject.GetComponent<PlayerStatus>().defense -= playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().defense;
        playerObject.GetComponent<PlayerStatus>().toughness -= playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().toughness;
        playerObject.GetComponent<PlayerStatus>().evasion -= playerObject.GetComponent<PlayerStatus>().equipedTrinket1.GetComponent<TrinketStatus>().evasion;
    }
    void UnequipTrinket2()
    {
        playerObject.GetComponent<PlayerStatus>().damage -= playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().damage;
        playerObject.GetComponent<PlayerStatus>().fireRate -= playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().rateOfFire;
        playerObject.GetComponent<PlayerStatus>().playerCritChance -= playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().criticalRate;
        playerObject.GetComponent<PlayerStatus>().playerCritDamage -= playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().criticalDamage;
        playerObject.GetComponent<PlayerStatus>().maxHealth -= playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().health;
        playerObject.GetComponent<PlayerStatus>().defense -= playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().defense;
        playerObject.GetComponent<PlayerStatus>().toughness -= playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().toughness;
        playerObject.GetComponent<PlayerStatus>().evasion -= playerObject.GetComponent<PlayerStatus>().equipedTrinket2.GetComponent<TrinketStatus>().evasion;
    }

    public void IncreaseHealth()
    {
        if (playerObject.GetComponent<PlayerStatus>().playerPointsLeft > 0)
        {
            playerObject.GetComponent<PlayerStatus>().maxHealth += 10;
            playerObject.GetComponent<PlayerStatus>().playerPointsLeft -= 1;
            UpdateEverything();
        }
    }
    public void IncreaseDamage()
    {
        if (playerObject.GetComponent<PlayerStatus>().playerPointsLeft > 0)
        {
            playerObject.GetComponent<PlayerStatus>().damage += 1;
            playerObject.GetComponent<PlayerStatus>().playerPointsLeft -= 1;
            UpdateEverything();
        }
    }
    public void IncreaseAtkRate()
    {
        if (playerObject.GetComponent<PlayerStatus>().playerPointsLeft > 0)
        {
            playerObject.GetComponent<PlayerStatus>().fireRate += 0.05f;
            playerObject.GetComponent<PlayerStatus>().playerPointsLeft -= 1;
            UpdateEverything();
        }
    }
    public void IncreaseCritChance()
    {
        if (playerObject.GetComponent<PlayerStatus>().playerPointsLeft > 0)
        {
            playerObject.GetComponent<PlayerStatus>().playerCritChance += 1;
            playerObject.GetComponent<PlayerStatus>().playerPointsLeft -= 1;
            UpdateEverything();
        }
    }


    void UpdateStatusWindow(GameObject window)
    {
        PlayerStatus currentPlayer = playerObject.GetComponent<PlayerStatus>();
        foreach (Transform child in window.transform)
        {

            var textList = child.GetComponentsInChildren<Text>();
            foreach (Text value in textList)
            {
                if (value.name == "ClassLevel")
                {
                    value.text = "Level " + Convert.ToString(currentPlayer.playerLevel) + " - " + Convert.ToString(currentPlayer.playerClass);
                }
                if (value.name == "PointsLeft")
                {
                    value.text = "Points Left: " + Convert.ToString(currentPlayer.playerPointsLeft);
                }
                if (value.name == "PlayerName")
                {
                    value.text = currentPlayer.playerName;
                }
                if (value.transform.parent.name == "HeartIcon")
                {
                    value.text = Convert.ToString(currentPlayer.maxHealth);
                }
                if (value.transform.parent.name == "DamageIcon")
                {
                    value.text = Convert.ToString(currentPlayer.damage);
                }
                if (value.transform.parent.name == "AttackSpeedIcon")
                {
                    value.text = currentPlayer.fireRate.ToString("F2");
                }
                if (value.transform.parent.name == "CriticalChanceIcon")
                {
                    value.text = Convert.ToString(currentPlayer.playerCritChance) + "%";
                }
                if (value.transform.parent.name == "CriticalDamageIcon")
                {
                    value.text = Convert.ToString(currentPlayer.playerCritDamage) + "%";
                }

                if (value.transform.parent.name == "DefenseIcon")
                {
                    value.text = Convert.ToString(currentPlayer.defense);
                }

                if (value.transform.parent.name == "ToughnessIcon")
                {
                    value.text = Convert.ToString(currentPlayer.toughness);
                }

                if (value.transform.parent.name == "EvasionIcon")
                {
                    value.text = currentPlayer.evasion.ToString("F2") + "%";
                }

            }
            child.gameObject.SetActive(true);

        }

    }
    void CreateBaseCharacter(PlayerStatus status)
    {
        status.playerLevel = 1;
        status.currentExp = 0;
        status.nextExp = 100;
        status.maxHealth = 100;
        status.currentHealth = status.maxHealth;
        status.playerPointsLeft = 0;
        status.damage = 10;
        status.fireRate = 2;
        status.playerCritChance = 5;
        status.playerCritDamage = 50;
        status.playerGold = 0;
        status.defense = 0;
        status.toughness = 0;
        status.evasion = 0;
    }

    public void BuyRandomItem()
    {
        if (itemBoughtWindow.activeSelf)
        {
            itemBoughtWindow.SetActive(false);
        }
        GetListOfPossibleItems();
        shopConfirmation.SetActive(true);
    }
    public void ConfirmItemBuy()
    {
        itemBoughtWindow.SetActive(true);
        shopConfirmation.SetActive(false);
        GameObject item = Instantiate(possibleItems[UnityEngine.Random.Range(0, possibleItems.Count - 1)], itemBought.transform.position, Quaternion.identity, itemBought.transform);
        item.transform.localScale = item.transform.localScale * 0.75f;
    }

    void GetListOfPossibleItems()
    {
        string stage = playerObject.GetComponent<PlayerStatus>().playerLastVisitedStage;
        possibleItems.Clear();
        switch (stage)
        {
            case "FIELDS":
                foreach (GameObject item in transform.GetComponent<LoadResources>().items)
                {
                    if (item.GetComponent<WeaponStatus>() != null)
                    {
                        if (item.GetComponent<WeaponStatus>().itemTier == 1)
                        {
                            possibleItems.Add(item);
                        }
                    }

                    if (item.GetComponent<ArmorStatus>() != null)
                    {
                        if (item.GetComponent<ArmorStatus>().itemTier == 1)
                        {
                            possibleItems.Add(item);
                        }
                    }

                    if (item.GetComponent<TrinketStatus>() != null)
                    {
                        if (item.GetComponent<TrinketStatus>().itemTier == 1)
                        {
                            possibleItems.Add(item);
                        }
                    }
                }
                break;
            default:
                foreach (GameObject item in transform.GetComponent<LoadResources>().items)
                {
                    if (item.GetComponent<WeaponStatus>() != null)
                    {
                        possibleItems.Add(item);
                    }

                    if (item.GetComponent<ArmorStatus>() != null)
                    {
                        possibleItems.Add(item);
                    }

                    if (item.GetComponent<TrinketStatus>() != null)
                    {
                        possibleItems.Add(item);
                    }
                }
                break;
        }
    }
    public void RefillPotionsWithGoldChecked()
    {
        if (!refillPotionsWithGold)
        {

            refillPotionsWithGold = true;
        }
        else
        {
            refillPotionsWithGold = false;
        }
    }

    public void RefillPotionsWithRubiesChecked()
    {
        if (!refillPotionsWithRubies)
        {

            refillPotionsWithRubies = true;
        }
        else
        {
            refillPotionsWithRubies = false;
        }
    }
}

