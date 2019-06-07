using UnityEngine;
using System.Collections;

public enum ItemType { MANA, HEALTH, WEAPON };
public enum Quality { COMMON, UNCOMMON, RARE, EPIC, LEGENDARY, ARTIFACT }

public class Item : MonoBehaviour
{
    /// <summary>
    /// The current item type
    /// </summary>
    public ItemType type;

    /// <summary>
    /// The items quality
    /// </summary>
    public Quality quality;

    /// <summary>
    /// The item's neutral sprite
    /// </summary>
    public Sprite spriteNeutral;

    /// <summary>
    /// The item's highlighted sprite
    /// </summary>
    public Sprite spriteHighlighted;

    /// <summary>
    /// The max amount of times the item can stack
    /// </summary>
    public int maxSize;

    /// <summary>
    /// These variable contains the stats of the item
    /// </summary>
    public float strength, intellect, agility, stamina;

    /// <summary>
    /// The item's name
    /// </summary>
    public string itemName;

    /// <summary>
    /// The item's description
    /// </summary>
    public string description;

    public Material colormaterial;

    private float NameTextSize;

    private float DescriptionTextSize;
    private string DescriptionTextColor;

    private float StatsTextSize;
    private string StatsTextColor;
    private string StatsTextNegativeColor;

    [System.Serializable]
    public struct StatType
    {
        public string StatName;
        public float StatValue;
    }

    public StatType[] statsList;

    /// <summary>
    /// Uses the item
    /// </summary>
    public void Use()
    {
        switch (type) //Checks which kind of item this is
        {
            case ItemType.MANA:
                Debug.Log("I just used a mana potion");
                break;
            case ItemType.HEALTH:
                Debug.Log("I just used a health potion");
                break;
        }

    }

    public string GetTooltip()
    {
        NameTextSize = PlayerPrefs.GetFloat("InventoryNameTextSize");
        DescriptionTextSize = PlayerPrefs.GetFloat("InventoryDescriptionTextSize");
        DescriptionTextColor = PlayerPrefs.GetString("IntentoryDescriptionTextColor");
        StatsTextSize = PlayerPrefs.GetFloat("InventoryStatsTextSize");
        StatsTextColor = PlayerPrefs.GetString("InventoryStatsTextColor");
        StatsTextNegativeColor = PlayerPrefs.GetString("InventoryStatsTextNegativeColor");

        string stats = string.Empty;  //Resets the stats info
        string color = string.Empty;  //Resets the color info
        string newLine = string.Empty; //Resets the new line

        if (description != string.Empty) //Creates a newline if the item has a description, this is done to makes sure that the headline and the description isn't on the same line
        {
            newLine = "\n";
        }

        switch (quality) //Sets the color according to the quality of the item
        {
            case Quality.COMMON:
                color = PlayerPrefs.GetString("InventoryQualityColorCommon");
                break;
            case Quality.UNCOMMON:
                color = PlayerPrefs.GetString("InventoryQualityColorUncommon");
                break;
            case Quality.RARE:
                color = PlayerPrefs.GetString("InventoryQualityColorRare");
                break;
            case Quality.EPIC:
                color = PlayerPrefs.GetString("InventoryQualityColorEpic");
                break;
            case Quality.LEGENDARY:
                color = PlayerPrefs.GetString("InventoryQualityColorLegendary");
                break;
            case Quality.ARTIFACT:
                color = PlayerPrefs.GetString("InventoryQualityColorArtifact");
                break;
        }

        //Adds the stats to the string if the value is larger than 0. If the value is 0 we don't need to show it on the tooltip
        foreach (StatType statItem in statsList)
        {
            if (strength > 0)
            {
                stats += "\n<color=" + StatsTextColor + ">+" + statItem.StatValue.ToString() + statItem + "</color>";
            }
            else if (strength < 0)
            {
                stats += "\n<color=" + StatsTextNegativeColor + ">" + statItem.StatValue.ToString() + " Strength </color>";
            }
        }

        if (strength > 0)
        {
            stats += "\n<color=" + StatsTextColor + ">+" + strength.ToString() + " Strength </color>";
        }
        else if (strength < 0)
        {
            stats += "\n<color=" + StatsTextNegativeColor + ">" + strength.ToString() + " Strength </color>";
        }
        if (intellect > 0)
        {
            stats += "\n<color=" + StatsTextColor + ">+" + strength.ToString() + " Intellect </color>";
        }
        else if (intellect < 0)
        {
            stats += "\n<color=" + StatsTextNegativeColor + ">" + strength.ToString() + " Intellect </color>";
        }
        if (agility > 0)
        {
            stats += "\n<color=" + StatsTextColor + ">+" + strength.ToString() + " Agility </color>";
        }
        else if (agility < 0)
        {
            stats += "\n<color=" + StatsTextNegativeColor + ">" + strength.ToString() + " Agility </color>";
        }
        if (stamina > 0)
        {
            stats += "\n<color=" + StatsTextColor + ">+" + strength.ToString() + " Stamina </color>";
        }
        else if (stamina < 0)
        {
            stats += "\n<color=" + StatsTextNegativeColor + ">" + strength.ToString() + " Stamina </color>";
        }

        //Returns the formatted string
        return string.Format("<color=" + color + "><size=" + NameTextSize + ">{0}</size></color><size=" + DescriptionTextSize + "><i><color=" + DescriptionTextColor + ">" + newLine + "{1}</color></i>{2}</size>", itemName, description, stats);
    }

    public void SetStats(Item item)
    {
        this.type = item.type;

        this.quality = item.quality;

        this.spriteNeutral = item.spriteNeutral;

        this.spriteHighlighted = item.spriteHighlighted;

        this.maxSize = item.maxSize;

        this.strength = item.strength;

        this.intellect = item.intellect;

        this.agility = item.agility;

        this.stamina = item.stamina;

        this.itemName = item.itemName;

        this.description = item.description;

        this.colormaterial = item.colormaterial;


       GetComponent<Renderer>().material.color = colormaterial.color;

  
        }
    }


