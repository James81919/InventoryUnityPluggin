using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ItemType {MANA, HEALTH, WEAPON};
public enum Quality {COMMON,UNCOMMON,RARE,EPIC,LEGENDARY,ARTIFACT}

public struct EffectType
{
    string name;
    int value;
}

public class Item : MonoBehaviour
{
    [Header("Sprites")]
    /// <summary>
    /// The item's neutral sprite
    /// </summary>
    public Sprite spriteNeutral;

    /// <summary>
    /// The item's highlighted sprite
    /// </summary>
    public Sprite spriteHighlighted;

    [Header("Stats")]
    /// <summary>
    /// The current item type
    /// </summary>
    public ItemType type;

    /// <summary>
    /// The items quality
    /// </summary>
    public Quality quality;

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

    [Header("Text Formatting")]
    /// <summary>
    /// The font size of the item name text
    /// </summary>
    public float NameTextSize = 16.0f;

    /// <summary>
    /// Determines whether or not the description text wraps
    /// </summary>
    public bool WrapDescription = false;

    /// <summary>
    /// The font size of the item description text
    /// </summary>
    public float DescriptionTextSize = 14.0f;

    /// <summary>
    /// The font color of the item description text
    /// </summary>
    public string DescriptionTextColor = "lime";

    /// <summary>
    /// The font size of the item stats text
    /// </summary>
    public float StatsTextSize = 14.0f;

    /// <summary>
    /// The font color the item stats text for positive values
    /// </summary>
    public string StatsTextPositiveColor = "White";

    /// <summary>
    /// The font color the item stats text for negative values
    /// </summary>
    public string StatsTextNegativeColor = "red";

    /// <summary>
    /// Uses the item
    /// </summary>
    public virtual void Use()
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
        string stats = string.Empty;  //Resets the stats info
        string color = string.Empty;  //Resets the color info
        string newLine = string.Empty; //Resets the new line

        if (description != string.Empty) //Creates a newline if the item has a description, this is done to makes sure that the headline and the describion isn't on the same line
        {
            newLine = "\n";
        }

        switch (quality) //Sets the color according to the quality of the item
        {
            case Quality.COMMON:
                color = "white";
                break;
            case Quality.UNCOMMON:
                color = "lime";
                break;
            case Quality.RARE:
                color = "navy";
                break;
            case Quality.EPIC:
                color = "magenta";
                break;
            case Quality.LEGENDARY:
                color = "orange";
                break;
            case Quality.ARTIFACT:
                color = "red";
                break;
        }

        //Adds the stats to the string if the value is larger than 0. If the value is 0 we dont need to show it on the tooltip
        if (strength > 0)
        {
            stats += "\n<color=" + StatsTextPositiveColor + ">+" + strength.ToString() + " Strength</color>";
        }
        else if (strength < 0)
        {
            stats += "\n<color=" + StatsTextNegativeColor + ">" + strength.ToString() + " Strength</color>";
        }
        if (intellect > 0)
        {
            stats += "\n<color=" + StatsTextPositiveColor + ">+" + intellect.ToString() + " Intellect</color>";
        }
        else if (intellect < 0)
        {
            stats += "\n<color=" + StatsTextNegativeColor + ">" + intellect.ToString() + " Intellect</color>";
        }
        if (agility > 0)
        {
            stats += "\n<color=" + StatsTextPositiveColor + ">+" + agility.ToString() + " Agility</color>";
        }
        else if (agility < 0)
        {
            stats += "\n<color=" + StatsTextNegativeColor + ">" + agility.ToString() + " Agility</color>";
        }
        if (stamina > 0)
        {
            stats += "\n<color=" + StatsTextPositiveColor + ">+" + stamina.ToString() + " Stamina</color>";
        }
        if (stamina < 0)
        {
            stats += "\n<color=" + StatsTextNegativeColor + ">" + stamina.ToString() + " Stamina</color>";
        }

        //Returns the formated string
        return string.Format("<color=" + color + "><size=" +  NameTextSize + ">{0}</size></color><size=" + DescriptionTextSize + "><i><color=" + DescriptionTextColor + ">" + newLine + "{1}</color></i></size><size=" + StatsTextSize + ">{2}</size>", itemName, description, stats);
    }
}
