using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static int selectedSlot;
    public static void SavePlayer(PlayerStatus playerStatus)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path;
        switch (playerStatus.playerSaveSlot)
        {
            case 1:
                path = Application.persistentDataPath + "/playerData1.osha";
                break;
            case 2:
                path = Application.persistentDataPath + "/playerData2.osha";
                break;
            case 3:
                path = Application.persistentDataPath + "/playerData3.osha";
                break;
            default:
                path = Application.persistentDataPath + "/TrashData.osha";
                break;

        }

        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(playerStatus);
        ManageInventory(playerStatus, data);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    static void ManageInventory(PlayerStatus playerStatus, PlayerData data)
    {

        if (playerStatus.equipedWeapon != null)
        {
            data.playerEquipedWeapon = WeaponStatus.WeaponToData(playerStatus.equipedWeapon.GetComponent<WeaponStatus>());
        }

        if (playerStatus.equipedArmor != null)
        {
            data.playerEquipedArmor = ArmorStatus.ArmorToData(playerStatus.equipedArmor.GetComponent<ArmorStatus>());
        }
        if (playerStatus.equipedTrinket1 != null)
        {
            data.playerEquipedTrinket1 = TrinketStatus.TrinketToData(playerStatus.equipedTrinket1.GetComponent<TrinketStatus>());
        }
        if (playerStatus.equipedTrinket2 != null)
        {
            data.playerEquipedTrinket2 = TrinketStatus.TrinketToData(playerStatus.equipedTrinket2.GetComponent<TrinketStatus>());
        }

        foreach (GameObject item in playerStatus.inventorySlots)
        {
            if (item != null)
            {
                if (item.GetComponent<WeaponStatus>() != null)
                {
                    for (var i = 0; i < 16; i++)
                    {
                        if (data.playerMenuInventory[i] == null)
                        {
                            data.playerMenuInventory[i] = WeaponStatus.WeaponToData(item.GetComponent<WeaponStatus>());
                            break;
                        }
                    }
                }
                if (item.GetComponent<ArmorStatus>() != null)
                {
                    for (var i = 0; i < 16; i++)
                    {
                        if (data.playerMenuInventory[i] == null)
                        {
                            data.playerMenuInventory[i] = ArmorStatus.ArmorToData(item.GetComponent<ArmorStatus>());
                            break;
                        }
                    }
                }

                if (item.GetComponent<TrinketStatus>() != null)
                {
                    for (var i = 0; i < 16; i++)
                    {
                        if (data.playerMenuInventory[i] == null)
                        {
                            data.playerMenuInventory[i] = TrinketStatus.TrinketToData(item.GetComponent<TrinketStatus>());
                            break;
                        }
                    }
                }
            }

        }
        foreach (GameObject item in playerStatus.menuInventorySlots)
        {
            if (item != null)
            {
                if (item.GetComponent<WeaponStatus>() != null)
                {
                    for (var i = 0; i < 16; i++)
                    {
                        if (data.playerMenuInventory[i] == null)
                        {
                            data.playerMenuInventory[i] = WeaponStatus.WeaponToData(item.GetComponent<WeaponStatus>());
                            break;
                        }
                    }
                }

                if (item.GetComponent<ArmorStatus>() != null)
                {
                    for (var i = 0; i < 16; i++)
                    {
                        if (data.playerMenuInventory[i] == null)
                        {
                            data.playerMenuInventory[i] = ArmorStatus.ArmorToData(item.GetComponent<ArmorStatus>());

                            break;
                        }
                    }
                }

                if (item.GetComponent<TrinketStatus>() != null)
                {
                    for (var i = 0; i < 16; i++)
                    {
                        if (data.playerMenuInventory[i] == null)
                        {
                            data.playerMenuInventory[i] = TrinketStatus.TrinketToData(item.GetComponent<TrinketStatus>());

                            break;
                        }
                    }
                }
            }
        }
    }

    public static void DeletePlayer(int slot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path;
        switch (slot)
        {
            case 1:
                path = Application.persistentDataPath + "/playerData1.osha";
                break;
            case 2:
                path = Application.persistentDataPath + "/playerData2.osha";
                break;
            case 3:
                path = Application.persistentDataPath + "/playerData3.osha";
                break;
            default:
                path = Application.persistentDataPath + "/TrashData.osha";
                break;

        }

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static PlayerData LoadPlayer1()
    {
        string path = Application.persistentDataPath + "/playerData1.osha";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            if (stream.Length != 0)
            {

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();
                return data;
            }
            else
            {
                Debug.Log("save existe mas está vazio");
                stream.Close();

                SaveSystem.DeletePlayer(1);
                return null;
            }
        }
        else
        {
            return null;
        }

    }
    public static PlayerData LoadPlayer2()
    {
        string path = Application.persistentDataPath + "/playerData2.osha";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
    public static PlayerData LoadPlayer3()
    {
        string path = Application.persistentDataPath + "/playerData3.osha";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
}
