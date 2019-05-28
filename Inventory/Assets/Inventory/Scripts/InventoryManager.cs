using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{

    #region fields

    /// <summary>
    /// This is the InventoryManager's singleton instance
    /// </summary>
    private static InventoryManager instance;

    /// <summary>
    /// The slots prefab
    /// </summary>
    public GameObject slotPrefab;

    /// <summary>
    /// A prefab used for instantiating the hoverObject
    /// </summary>
    public GameObject iconPrefab;

    /// <summary>
    /// A reference to the object that hovers next to the mouse
    /// </summary>
    private GameObject hoverObject;

    /// <summary>
    /// A prototy of our mana item
    /// This is used when loading a saved inventory
    /// </summary>
    public GameObject mana;

    /// <summary>
    /// A prototype of our healt potion
    /// This is used when loading a saved inventory
    /// </summary>
    public GameObject health;

    /// <summary>
    /// A prototype of our weapon
    /// This is used when loading a saved inventory
    /// </summary>
    public GameObject weapon;

    /// <summary>
    /// A prototype of the item to drop
    /// </summary>
    public GameObject dropItem;

    /// <summary>
    /// The tool tip to show at the screen
    /// </summary>
    public GameObject tooltipObject;

    /// <summary>
    /// This object is used for scaling the tooltip
    /// </summary>
    public Text sizeTextObject;

    /// <summary>
    /// This is the visual text on the tooltip
    /// </summary>
    public Text visualTextObject;

    /// <summary>
    /// A reference to the inventorys canvas
    /// </summary>
    public Canvas canvas;

    /// <summary>
    /// The slots that we are moving an item from
    /// </summary>
    private Slot from;

    /// <summary>
    /// The slots that we are moving and item to
    /// </summary>
    private Slot to;


    /// <summary>
    /// This is sed to store our items when moving them from one slot to another
    /// </summary>
    private Slot movingSlot;

    /// <summary>
    /// The clicked object
    /// </summary>
    private GameObject clicked;


    /// <summary>
    /// The amount of items to pickup (this is the text on the UI element we use for splitting)
    /// </summary>
    public Text stackText;


    /// <summary>
    /// The UI element that we are using when we need to split a stack
    /// </summary>
    public GameObject selectStackSize;


    /// <summary>
    /// The amount of items we have in our "hand"
    /// </summary>
    private int splitAmount;

    /// <summary>
    /// The maximum amount of items we are allowed to remove from the stack
    /// </summary>
    private int maxStackCount;


    /// <summary>
    /// A reference to the EventSystem 
    /// </summary>
    public EventSystem eventSystem;

    #endregion

    #region properties

    /// <summary>
    /// This is the property for the singleton instance
    /// </summary>
    public static InventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryManager>();
            }

            return instance;

        }
    }

    public Slot From
    {
        get { return from; }
        set { from = value; }
    }

    public Slot To
    {
        get { return to; }
        set { to = value; }
    }

    public GameObject Clicked
    {
        get { return clicked; }
        set { clicked = value; }
    }

    public int SplitAmount
    {
        get { return splitAmount; }
        set { splitAmount = value; }
    }

    public int MaxStackCount
    {
        get { return maxStackCount; }
        set { maxStackCount = value; }
    }

    public Slot MovingSlot
    {
        get { return movingSlot; }
        set { movingSlot = value; }
    }

    public GameObject HoverObject
    {
        get { return hoverObject; }
        set { hoverObject = value; }
    }

    #endregion

    /// <summary>
    /// Sets the stacks info, so that we know how many items we can remove
    /// </summary>
    /// <param name="maxStackCount"></param>
    public void SetStackInfo(int maxStackCount)
    {
        //Shows the UI for splitting a stack
        selectStackSize.SetActive(true);

        //Hides the tooltip so that it doesn't overlap the splitstack ui
        tooltipObject.SetActive(false);
        

        //Resets the amount of split items
        splitAmount = 0;

        //Stores the maxcount
        this.maxStackCount = maxStackCount;

        //Writes writes the selected amount of itesm in the UI
        stackText.text = splitAmount.ToString();
    }

    /// <summary>
    /// Saves every single inventory in the scene
    /// </summary>
    public void Save()
    {   
        //Finds all inventories
        GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");

        //Loads all inventories
        foreach (GameObject inventory in inventories)
        {
            inventory.GetComponent<Inventory>().SaveInventory();
        }
    }

    /// <summary>
    /// Loads every single inventory in the scene
    /// </summary>
    public void Load()
    {
        //Finds all inventorys
        GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");

        //Loads all inventories
        foreach (GameObject inventory in inventories)
        {
            inventory.GetComponent<Inventory>().LoadInventory();
        }
    }
}
