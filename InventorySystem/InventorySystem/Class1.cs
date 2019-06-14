using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    public enum ItemType { MANA, HEALTH, WEAPON };
    public enum Quality { COMMON, UNCOMMON, RARE, EPIC, LEGENDARY, ARTIFACT };

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

    public class Slot : MonoBehaviour, IPointerClickHandler
    {
        #region variables

        /// <summary>
        /// The items that the slot contains
        /// </summary>
        private Stack<Item> items;

        /// <summary>
        /// Indicates the number of items stacked on the slot
        /// </summary>
        public Text stackTxt;

        /// <summary>
        /// The slot's empty sprite
        /// </summary>
        public Sprite slotEmpty;

        /// <summary>
        /// The slot's highlighted sprite
        /// </summary>
        public Sprite slotHighlight;

        private CanvasGroup canvasGroup;

        #endregion

        #region properties

        /// <summary>
        /// A property for accessing the stack of items
        /// </summary>
        public Stack<Item> Items
        {
            get { return items; }
            set { items = value; }
        }

        /// <summary>
        /// Indicates if the slot is empty
        /// </summary>
        public bool IsEmpty
        {
            get { return items.Count == 0; }
        }

        /// <summary>
        /// Indicates if the slot is available for stacking more items
        /// </summary>
        public bool IsAvailable
        {
            get { return CurrentItem.maxSize > items.Count; }
        }

        /// <summary>
        /// Returns the current item in the stack
        /// </summary>
        public Item CurrentItem
        {
            get { return items.Peek(); }
        }

        #endregion

        void Awake()
        {
            //Instantiates the items stack
            items = new Stack<Item>();
        }

        // Use this for initialization
        void Start()
        {
            //Creates a reference to the slot slot's rect transform
            RectTransform slotRect = GetComponent<RectTransform>();

            //Creates a reference to the stackTxt's rect transform
            RectTransform txtRect = stackTxt.GetComponent<RectTransform>();

            //Calculates the scale factor of the text by taking 60% of the slots width
            int txtScleFactor = (int)(slotRect.sizeDelta.x * 0.60);

            //Sets the min and max textSize of the stackTxt
            stackTxt.resizeTextMaxSize = txtScleFactor;
            stackTxt.resizeTextMinSize = txtScleFactor;

            //Sets the actual size of the txtRect
            txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
            txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);

            if (transform.parent != null)
            {
                canvasGroup = transform.parent.GetComponent<CanvasGroup>();
            }
        }

        /// <summary>
        /// Adds a single item to th inventory
        /// </summary>
        /// <param name="item">The item to add</param>
        public void AddItem(Item item)
        {
            items.Push(item); //Adds the item to the stack

            if (items.Count > 1) //Checks if we have a stacked item
            {
                stackTxt.text = items.Count.ToString(); //If the item is stacked then we need to write the stack number on top of the icon
            }

            ChangeSprite(item.spriteNeutral, item.spriteHighlighted); //Changes the sprite so that it reflects the item the slot is occupied by
        }

        /// <summary>
        /// Adds a stack of items to the slot
        /// </summary>
        /// <param name="items">The stack of items to add</param>
        public void AddItems(Stack<Item> items)
        {
            this.items = new Stack<Item>(items); //Adds the stack of items to the slot

            stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty; //Writes the correct stack number on the icon

            ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted); //Changes the sprite so that it reflects the item the slot is occupied by
        }

        /// <summary>
        /// Changes the sprite of a slot
        /// </summary>
        /// <param name="neutral">The neutral sprite</param>
        /// <param name="highlight">The highlighted sprite</param>
        private void ChangeSprite(Sprite neutral, Sprite highlight)
        {
            //Sets the neutral sprite
            GetComponent<Image>().sprite = neutral;

            //Creates a spriteState, so that we can change the sprites of the different states
            SpriteState st = new SpriteState();
            st.highlightedSprite = highlight;
            st.pressedSprite = neutral;

            //Sets the sprite state
            GetComponent<Button>().spriteState = st;
        }

        /// <summary>
        /// Uses an item on the slot.
        /// </summary>
        private void UseItem()
        {
            if (!IsEmpty) //If there is an item on the slot
            {
                items.Pop().Use(); //Removes the top item from the stack and uses it

                stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty; //Writes the correct stack number on the icon

                if (IsEmpty) //Checks if we just removed the last item from the inventory
                {
                    ChangeSprite(slotEmpty, slotHighlight); //Changes the sprite to empty if the slot is empty

                    transform.parent.GetComponent<Inventory>().EmptySlots++; //Adds 1 to the amount of empty slots

                }
            }
        }

        /// <summary>
        /// Clears the slot
        /// </summary>
        public void ClearSlot()
        {
            //Clears all items on the slot
            items.Clear();

            //Changes the sprite to empty
            ChangeSprite(slotEmpty, slotHighlight);

            //Clears the text
            stackTxt.text = string.Empty;
        }

        /// <summary>
        /// Removes an amount of items from the slot and  returns them
        /// </summary>
        /// <param name="amount">The amount of items to remove</param>
        /// <returns>Stack of removed items</returns>
        public Stack<Item> RemoveItems(int amount)
        {
            //Creates a temporary stack for containing the items the we need to remove
            Stack<Item> tmp = new Stack<Item>();

            for (int i = 0; i < amount; i++) //Runs through the slots items and pops the into the tmp stack
            {
                tmp.Push(items.Pop());
            }

            //Makes sure that the correct number is shown on the slot
            stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

            //Returns the items that we just removed
            return tmp;
        }

        /// <summary>
        /// Removes the top item from the slot and returns it
        /// </summary>
        /// <returns>The removed item</returns>
        public Item RemoveItem()
        {
            //Remove the item from the stack and stores it in a tmp variable
            Item tmp = items.Pop();

            //Makes sure that the correct number is shown on the slot
            stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

            //Returns the removed item
            return tmp;
        }


        /// <summary>
        /// Handles OnPointer events
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            //If the right mouse button was clicked, and we aren't moving an item and the inventory is visible
            if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && canvasGroup.alpha > 0)
            {
                //Uses an item on the slot
                UseItem();
            }
            //Checks if we need to show the split stack dialog , this is only done if we shiftclick a slot and we aren't moving an item
            else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && !IsEmpty && !GameObject.Find("Hover"))
            {
                //The dialogs spawn position
                Vector2 position;

                //Translates the mouse position to on-screen coordinates so that we can spawn the dialog at the correct position
                RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.canvas.transform as RectTransform, Input.mousePosition, InventoryManager.Instance.canvas.worldCamera, out position);

                //Shows the dialog
                InventoryManager.Instance.selectStackSize.SetActive(true);

                //Sets the position
                InventoryManager.Instance.selectStackSize.transform.position = InventoryManager.Instance.canvas.transform.TransformPoint(position);

                //Tell the inventory the item count on the selected slot
                InventoryManager.Instance.SetStackInfo(items.Count);
            }
        }
    }

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
        /// A property of our mana item
        /// This is used when loading a saved inventory
        /// </summary>
        public GameObject mana;

        /// <summary>
        /// A prototype of our health potion
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
        /// A reference to the inventory canvas
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
        /// This is said to store our items when moving them from one slot to another
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

    public class Inventory : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// The number of rows
        /// </summary>
        public int rows;

        /// <summary>
        /// The number of slots
        /// </summary>
        public int slots;

        /// <summary>
        /// The number of empty slots in the inventory
        /// </summary>
        private int emptySlots;

        /// <summary>
        /// Offset used to move the hovering object away from the mouse 
        /// </summary>
        private float hoverYOffset;

        /// <summary>
        /// The width and height of the inventory
        /// </summary>
        private float inventoryWidth, inventoryHight;

        /// <summary>
        /// The left and top slots padding
        /// </summary>
        public float slotPaddingLeft, slotPaddingTop;

        /// <summary>
        /// The size of each slot
        /// </summary>
        public float slotSize;

        /// <summary>
        /// A reference to the inventorys RectTransform
        /// </summary>
        private RectTransform inventoryRect;

        /// <summary>
        /// The inventory's canvas group, this is used for hiding the inventory
        /// </summary>
        private CanvasGroup canvasGroup;

        /// <summary>
        /// The inventory's singleton instance
        /// </summary>
        private static Inventory instance;

        /// <summary>
        /// Indicates if the inventory is in the process of fading in
        /// </summary>
        private bool fadingIn;

        /// <summary>
        /// Indicates if the inventory is in the process of fading out
        /// </summary>
        private bool fadingOut;

        /// <summary>
        /// The time it takes for the inventory to fade in seconds
        /// </summary>
        public float fadeTime;

        /// <summary>
        /// This indicates if the inventory is open or closes
        /// </summary>
        private bool isOpen;

        public static bool mouseInside = false;

        #endregion

        #region Collections
        /// <summary>
        /// A list of all the slots in the inventory
        /// </summary>
        private List<GameObject> allSlots;

        #endregion

        #region Properties

        /// <summary>
        /// Property for accessing our singleton
        /// </summary>
        public static Inventory Instance
        {
            get
            {
                if (instance == null) //Creates a reference to our inventory, if it's null
                {
                    instance = GameObject.FindObjectOfType<Inventory>();
                }
                return Inventory.instance;
            }
        }

        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
        }

        /// <summary>
        /// Property for accessing the amount of empty slots
        /// </summary>
        public int EmptySlots
        {
            get { return emptySlots; }
            set { emptySlots = value; }
        }

        #endregion

        private static GameObject playerRef;

        // Use this for initialization
        void Start()
        {
            isOpen = false;

            playerRef = GameObject.Find("Player");

            canvasGroup = GetComponent<CanvasGroup>();

            //Creates the inventory layout
            CreateLayout();

            InventoryManager.Instance.MovingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonUp(0)) //Checks if the user lifted the first mouse button
            {
                //Removes the selected item from the inventory
                if (!mouseInside && InventoryManager.Instance.From != null) //If we click outside the inventory and the have picked up an item
                {
                    InventoryManager.Instance.From.GetComponent<Image>().color = Color.white; //Rests the slots color 

                    foreach (Item item in InventoryManager.Instance.From.Items)
                    {
                        float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);

                        Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

                        v *= 25;

                        GameObject tmpDrp = (GameObject)GameObject.Instantiate(InventoryManager.Instance.dropItem, playerRef.transform.position - v, Quaternion.identity);

                        tmpDrp.GetComponent<Item>().SetStats(item);
                    }

                    InventoryManager.Instance.From.ClearSlot(); //Removes the item from the slot
                    Destroy(GameObject.Find("Hover")); //Removes the hover icon

                    //Resets the objects
                    InventoryManager.Instance.To = null;
                    InventoryManager.Instance.From = null;
                    emptySlots++;
                }
                else if (!InventoryManager.Instance.eventSystem.IsPointerOverGameObject(-1) && !InventoryManager.Instance.MovingSlot.IsEmpty)
                {

                    foreach (Item item in InventoryManager.Instance.MovingSlot.Items)
                    {
                        float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);

                        Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

                        v *= 25;

                        GameObject tmpDrp = (GameObject)GameObject.Instantiate(InventoryManager.Instance.dropItem, playerRef.transform.position - v, Quaternion.identity);

                        tmpDrp.GetComponent<Item>().SetStats(item);
                    }

                    InventoryManager.Instance.MovingSlot.ClearSlot();
                    Destroy(GameObject.Find("Hover"));
                }
            }
            if (InventoryManager.Instance.HoverObject != null) //Checks if the hover object exists
            {
                //The hoverObject's position
                Vector2 position;

                //Translates the mouse screen position into a local position and stores it in the position
                RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.canvas.transform as RectTransform, Input.mousePosition, InventoryManager.Instance.canvas.worldCamera, out position);

                //Adds the offset to the position
                position.Set(position.x, position.y - hoverYOffset);

                //Sets the hoverObject's position
                InventoryManager.Instance.HoverObject.transform.position = InventoryManager.Instance.canvas.transform.TransformPoint(position);
            }
            if (Input.GetKeyDown(KeyCode.R))//Checks if we press the B button
            {
                PlayerPrefs.DeleteAll();
            }
        }

        public void OnDrag()
        {
            MoveInventory();//Moves the inventory around
        }

        public void PointerExit()
        {

            mouseInside = false;
        }

        public void PointerEnter()
        {
            if (canvasGroup.alpha > 0)
            {

                mouseInside = true;
            }
        }

        public void Open()
        {
            if (canvasGroup.alpha > 0) //If our inventory is visible, then we know that it is open
            {
                StartCoroutine("FadeOut"); //Close the inventory
                PutItemBack(); //Put all items we have in our hand back in the inventory
                HideToolTip();
                isOpen = false;

            }
            else//If it isn't open then it's closed and we need to fade in
            {
                StartCoroutine("FadeIn");

                isOpen = true;
            }
        }

        /// <summary>
        /// Shows the tooltip
        /// </summary>
        /// <param name="slot">The slot we just hovered</param>
        public void ShowToolTip(GameObject slot)
        {
            //Saves a reference to the slot we just moused over
            Slot tmpSlot = slot.GetComponent<Slot>();

            //If the slot contains an item and we aren't splitting or moving any items then we can show the tooltip
            if (slot.GetComponentInParent<Inventory>().isOpen && !tmpSlot.IsEmpty && InventoryManager.Instance.HoverObject == null && !InventoryManager.Instance.selectStackSize.activeSelf)
            {
                //Gets the information from the item on the slot we just moved our mouse over
                InventoryManager.Instance.visualTextObject.text = tmpSlot.CurrentItem.GetTooltip();

                //Makes sure that the tooltip has the correct size.
                InventoryManager.Instance.sizeTextObject.text = InventoryManager.Instance.visualTextObject.text;

                //Shows the tool tip
                InventoryManager.Instance.tooltipObject.SetActive(true);

                //Calculates the position while taking the padding into account
                float xPos = slot.transform.position.x + slotPaddingLeft;
                float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y - slotPaddingTop;

                //Sets the position
                InventoryManager.Instance.tooltipObject.transform.position = new Vector2(xPos, yPos);
            }


        }

        /// <summary>
        /// Hide the tooltip
        /// </summary>
        public void HideToolTip()
        {
            InventoryManager.Instance.tooltipObject.SetActive(false);
        }

        /// <summary>
        /// Saves the inventory and its content
        /// </summary>
        public void SaveInventory()
        {
            string content = string.Empty; //Creates a string for containing info about the items inside the inventory

            for (int i = 0; i < allSlots.Count; i++) //Runs through all slots in the inventory
            {
                Slot tmp = allSlots[i].GetComponent<Slot>(); //Creates a reference to the slot at the current index

                if (!tmp.IsEmpty) //We only want to save the info if the slot contains an item
                {
                    //Creates a string with this format: SlotIndex-ItemType-AmountOfItems; this string can be read so that we can rebuild the inventory
                    content += i + "-" + tmp.CurrentItem.type.ToString() + "-" + tmp.Items.Count.ToString() + ";";
                }
            }

            //Stores all the info in the PlayerPrefs
            PlayerPrefs.SetString(gameObject.name + "content", content);
            PlayerPrefs.SetInt(gameObject.name + "slots", slots);
            PlayerPrefs.SetInt(gameObject.name + "rows", rows);
            PlayerPrefs.SetFloat(gameObject.name + "slotPaddingLeft", slotPaddingLeft);
            PlayerPrefs.SetFloat(gameObject.name + "slotPaddingTop", slotPaddingTop);
            PlayerPrefs.SetFloat(gameObject.name + "slotSize", slotSize);
            PlayerPrefs.SetFloat(gameObject.name + "xPos", inventoryRect.position.x);
            PlayerPrefs.SetFloat(gameObject.name + "yPos", inventoryRect.position.y);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Loads the inventory
        /// </summary>
        public void LoadInventory()
        {
            //Loads all the inventory's data from the PlayerPrefs
            string content = PlayerPrefs.GetString(gameObject.name + "content");

            if (content != string.Empty)
            {
                slots = PlayerPrefs.GetInt(gameObject.name + "slots");
                rows = PlayerPrefs.GetInt(gameObject.name + "rows");
                slotPaddingLeft = PlayerPrefs.GetFloat(gameObject.name + "slotPaddingLeft");
                slotPaddingTop = PlayerPrefs.GetFloat(gameObject.name + "slotPaddingTop");
                slotSize = PlayerPrefs.GetFloat(gameObject.name + "slotSize");

                //Sets the inventory position
                inventoryRect.position = new Vector3(PlayerPrefs.GetFloat(gameObject.name + "xPos"), PlayerPrefs.GetFloat(gameObject.name + "yPos"), inventoryRect.position.z);

                //Recreates the inventory's layout
                CreateLayout();

                //Splits the loaded content string into segments, so that each index in the splitContent array contains information about a single slot
                //e.g[0]0-MANA-3
                string[] splitContent = content.Split(';');

                //Runs through every single slot we have info about -1 is to avoid an empty string error
                for (int x = 0; x < splitContent.Length - 1; x++)
                {
                    //Splits the slot's information into single values, so that each index in the splitValues array contains info about a value
                    //E.g[0]InventorIndex [1]ITEMTYPE [2]Amount of items
                    string[] splitValues = splitContent[x].Split('-');

                    int index = Int32.Parse(splitValues[0]); //InventorIndex 

                    ItemType type = (ItemType)Enum.Parse(typeof(ItemType), splitValues[1]); //ITEMTYPE

                    int amount = Int32.Parse(splitValues[2]); //Amount of items

                    for (int i = 0; i < amount; i++) //Adds the correct amount of items to the inventory
                    {
                        switch (type)
                        {
                            case ItemType.MANA: //Adds a mana potion
                                allSlots[index].GetComponent<Slot>().AddItem(InventoryManager.Instance.mana.GetComponent<Item>());
                                break;
                            case ItemType.HEALTH://Adds a health potion
                                allSlots[index].GetComponent<Slot>().AddItem(InventoryManager.Instance.health.GetComponent<Item>());
                                break;
                            case ItemType.WEAPON://Adds a weapon
                                allSlots[index].GetComponent<Slot>().AddItem(InventoryManager.Instance.weapon.GetComponent<Item>());
                                break;
                        }
                    }


                }
            }



        }

        /// <summary>
        /// Creates the inventory's layout
        /// </summary>
        private void CreateLayout()
        {
            if (allSlots != null)
            {
                foreach (GameObject go in allSlots)
                {
                    Destroy(go);
                }
            }

            //Instantiates the allSlot's list
            allSlots = new List<GameObject>();

            //Calculates the hoverYOffset by taking 1% of the slot size
            hoverYOffset = slotSize * 0.01f;

            //Stores the number of empty slots
            emptySlots = slots;

            //Calculates the width of the inventory
            inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft);

            //Calculates the highs of the inventory
            inventoryHight = rows * (slotSize + slotPaddingTop);

            //Creates a reference to the inventory's RectTransform
            inventoryRect = GetComponent<RectTransform>();

            //Sets the with and height of the inventory.
            inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth + slotPaddingLeft);
            inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHight + slotPaddingTop);

            //Calculates the amount of columns
            int columns = slots / rows;

            for (int y = 0; y < rows; y++) //Runs through the rows
            {
                for (int x = 0; x < columns; x++) //Runs through the columns
                {
                    //Instantiates the slot and creates a reference to it
                    GameObject newSlot = (GameObject)Instantiate(InventoryManager.Instance.slotPrefab);

                    //Makes a reference to the rect transform
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                    //Sets the slots name
                    newSlot.name = "Slot";

                    //Sets the canvas as the parent of the slots, so that it will be visible on the screen
                    newSlot.transform.SetParent(this.transform.parent);

                    //Sets the slots position
                    slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

                    //Sets the size of the slot
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * InventoryManager.Instance.canvas.scaleFactor);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * InventoryManager.Instance.canvas.scaleFactor);
                    newSlot.transform.SetParent(this.transform);

                    //Adds the new slots to the slot list
                    allSlots.Add(newSlot);

                }
            }
        }

        /// <summary>
        /// Adds an item to the inventory
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <returns></returns>
        public bool AddItem(Item item)
        {
            if (item.maxSize == 1) //If the item isn't stackable
            {
                //Places the item at an empty slot
                PlaceEmpty(item);
                return true;
            }
            else //If the item is stackable 
            {
                foreach (GameObject slot in allSlots) //Runs through all slots in the inventory
                {
                    Slot tmp = slot.GetComponent<Slot>(); //Creates a reference to the slot

                    if (!tmp.IsEmpty) //If the item isn't empty
                    {
                        //Checks if the om the slot is the same type as the item we want to pick up
                        if (tmp.CurrentItem.type == item.type && tmp.IsAvailable)
                        {
                            if (!InventoryManager.Instance.MovingSlot.IsEmpty && InventoryManager.Instance.Clicked.GetComponent<Slot>() == tmp.GetComponent<Slot>())
                            {
                                continue;
                            }
                            else
                            {
                                tmp.AddItem(item); //Adds the item to the inventory
                                return true;
                            }
                        }
                    }
                }
                if (emptySlots > 0) //Places the item on an empty slots
                {
                    PlaceEmpty(item);
                }
            }

            return false;
        }
        /// <summary>
        /// Moves the whole inventory
        /// </summary>
        private void MoveInventory()
        {
            Vector2 mousePos; //The inventory's new position

            //Translates the middle of the inventory into the mouse position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.canvas.transform as RectTransform, new Vector3(Input.mousePosition.x - (inventoryRect.sizeDelta.x / 2 * InventoryManager.Instance.canvas.scaleFactor), Input.mousePosition.y + (inventoryRect.sizeDelta.y / 2 * InventoryManager.Instance.canvas.scaleFactor)), InventoryManager.Instance.canvas.worldCamera, out mousePos);

            //Sets the inventorys position
            transform.position = InventoryManager.Instance.canvas.transform.TransformPoint(mousePos);
        }

        /// <summary>
        /// Places an item on an empty slot
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool PlaceEmpty(Item item)
        {
            if (emptySlots > 0) //If we have atleast 1 empty slot
            {
                foreach (GameObject slot in allSlots) //Runs through all slots
                {
                    Slot tmp = slot.GetComponent<Slot>(); //Creates a reference to the slot 

                    if (tmp.IsEmpty) //If the slot is empty
                    {
                        tmp.AddItem(item); //Adds the item
                        emptySlots--; //Reduces the number of empty slots
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Moves an item to another slot in the inventory
        /// </summary>
        /// <param name="clicked"></param>
        public void MoveItem(GameObject clicked)
        {
            if (clicked.transform.parent.GetComponent<CanvasGroup>().alpha > 0)
            {
                //Careates a reference to the object that we just clicked
                InventoryManager.Instance.Clicked = clicked;

                if (!InventoryManager.Instance.MovingSlot.IsEmpty)//Checks if we are splitting an item
                {
                    Slot tmp = clicked.GetComponent<Slot>(); //Get's a reference to the slot we just clicked

                    if (tmp.IsEmpty)//If the clicked slot is empty, then we can simply put all items down
                    {
                        tmp.AddItems(InventoryManager.Instance.MovingSlot.Items); //Puts all the items down in the slot that we clicked
                        InventoryManager.Instance.MovingSlot.Items.Clear(); //Clears the moving slot
                        Destroy(GameObject.Find("Hover")); //Removes the hover object
                    }
                    else if (!tmp.IsEmpty && InventoryManager.Instance.MovingSlot.CurrentItem.type == tmp.CurrentItem.type && tmp.IsAvailable) //If the slot we clicked isn't empty, then we need to merge the stacks
                    {
                        //Merges two stacks of the same type
                        MergeStacks(InventoryManager.Instance.MovingSlot, tmp);
                    }
                }
                else if (InventoryManager.Instance.From == null && clicked.transform.parent.GetComponent<Inventory>().isOpen && !Input.GetKey(KeyCode.LeftShift)) //If we haven't picked up an item
                {
                    if (!clicked.GetComponent<Slot>().IsEmpty && !GameObject.Find("Hover")) //If the slot we clicked sin't empty
                    {
                        InventoryManager.Instance.From = clicked.GetComponent<Slot>(); //The slot we ar emoving from

                        InventoryManager.Instance.From.GetComponent<Image>().color = Color.gray; //Sets the from slots color to gray, to visually indicate that its the slot we are moving from

                        CreateHoverIcon();

                    }
                }
                else if (InventoryManager.Instance.To == null && !Input.GetKey(KeyCode.LeftShift)) //Selects the slot we are moving to
                {
                    InventoryManager.Instance.To = clicked.GetComponent<Slot>(); //Sets the to object
                    Destroy(GameObject.Find("Hover")); //Destroys the hover object
                }
                if (InventoryManager.Instance.To != null && InventoryManager.Instance.From != null) //If both to and from are null then we are done moving. 
                {
                    if (!InventoryManager.Instance.To.IsEmpty && InventoryManager.Instance.From.CurrentItem.type == InventoryManager.Instance.To.CurrentItem.type && InventoryManager.Instance.To.IsAvailable)
                    {
                        MergeStacks(InventoryManager.Instance.From, InventoryManager.Instance.To);
                    }
                    else
                    {
                        Stack<Item> tmpTo = new Stack<Item>(InventoryManager.Instance.To.Items); //Stores the items from the to slot, so that we can do a swap

                        InventoryManager.Instance.To.AddItems(InventoryManager.Instance.From.Items); //Stores the items in the "from" slot in the "to" slot

                        if (tmpTo.Count == 0) //If "to" slot if 0 then we dont need to move anything to the "from " slot.
                        {
                            InventoryManager.Instance.From.ClearSlot(); //clears the from slot
                        }
                        else
                        {
                            InventoryManager.Instance.From.AddItems(tmpTo); //If the "to" slot contains items thne we need to move the to the "from" slot
                        }
                    }

                    //Resets all values
                    InventoryManager.Instance.From.GetComponent<Image>().color = Color.white;
                    InventoryManager.Instance.To = null;
                    InventoryManager.Instance.From = null;
                    Destroy(GameObject.Find("Hover"));
                }
            }

        }

        /// <summary>
        /// Creates a hover icon next to the mouse
        /// </summary>
        private void CreateHoverIcon()
        {
            InventoryManager.Instance.HoverObject = (GameObject)Instantiate(InventoryManager.Instance.iconPrefab); //Instantiates the hover object 

            InventoryManager.Instance.HoverObject.GetComponent<Image>().sprite = InventoryManager.Instance.Clicked.GetComponent<Image>().sprite; //Sets the sprite on the hover object so that it reflects the object we are moing

            InventoryManager.Instance.HoverObject.name = "Hover"; //Sets the name of the hover object

            //Creates references to the transforms
            RectTransform hoverTransform = InventoryManager.Instance.HoverObject.GetComponent<RectTransform>();
            RectTransform clickedTransform = InventoryManager.Instance.Clicked.GetComponent<RectTransform>();

            ///Sets the size of the hoverobject so that it has the same size as the clicked object
            hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
            hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

            //Sets the hoverobject's parent as the canvas, so that it is visible in the game
            InventoryManager.Instance.HoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);

            //Sets the local scale to make usre that it has the correct size
            InventoryManager.Instance.HoverObject.transform.localScale = InventoryManager.Instance.Clicked.gameObject.transform.localScale;

            InventoryManager.Instance.HoverObject.transform.GetChild(0).GetComponent<Text>().text = InventoryManager.Instance.MovingSlot.Items.Count > 1 ? InventoryManager.Instance.MovingSlot.Items.Count.ToString() : string.Empty;
        }

        /// <summary>
        /// Puts the items back in the inventory
        /// </summary>
        private void PutItemBack()
        {
            if (InventoryManager.Instance.From != null)//If we are carrying a whole stack of items
            {
                //put the items back and remove the hover icon
                Destroy(GameObject.Find("Hover"));
                InventoryManager.Instance.From.GetComponent<Image>().color = Color.white;
                InventoryManager.Instance.From = null;
            }
            else if (!InventoryManager.Instance.MovingSlot.IsEmpty) //If we are carrying  split stack
            {
                //Removes the hover icon
                Destroy(GameObject.Find("Hover"));

                //Puts the items back one by one
                foreach (Item item in InventoryManager.Instance.MovingSlot.Items)
                {
                    InventoryManager.Instance.Clicked.GetComponent<Slot>().AddItem(item);
                }

                InventoryManager.Instance.MovingSlot.ClearSlot(); //Makes sure that the moving slot is empty
            }

            //Hides the UI for splitting a stack
            InventoryManager.Instance.selectStackSize.SetActive(false);
        }


        /// <summary>
        /// Splits a stack of items
        /// </summary>
        public void SplitStack()
        {
            //Hids the UI for splitting a stack
            InventoryManager.Instance.selectStackSize.SetActive(false);

            if (InventoryManager.Instance.SplitAmount == InventoryManager.Instance.MaxStackCount) //If we picked up all the items then we dont need to handle it as as split stack
            {
                MoveItem(InventoryManager.Instance.Clicked);
            }
            else if (InventoryManager.Instance.SplitAmount > 0) //If the split amount is larger than 0 then we need to pick up x amount of items
            {
                InventoryManager.Instance.MovingSlot.Items = InventoryManager.Instance.Clicked.GetComponent<Slot>().RemoveItems(InventoryManager.Instance.SplitAmount); //Picks up the items 

                CreateHoverIcon(); //Careates the hover icon
            }
        }

        /// <summary>
        /// Updates the text on the split UI elemt so that it reflects the users selection
        /// </summary>
        /// <param name="i"></param>
        public void ChangeStackText(int i)
        {
            InventoryManager.Instance.SplitAmount += i;

            if (InventoryManager.Instance.SplitAmount < 0) //Makes sure we dont go below 
            {
                InventoryManager.Instance.SplitAmount = 0;
            }
            if (InventoryManager.Instance.SplitAmount > InventoryManager.Instance.MaxStackCount) //Makes sure that we dont go above max
            {
                InventoryManager.Instance.SplitAmount = InventoryManager.Instance.MaxStackCount;
            }

            //Writes the text on the UI element
            InventoryManager.Instance.stackText.text = InventoryManager.Instance.SplitAmount.ToString();
        }

        /// <summary>
        /// Merges the items on two slots
        /// </summary>
        /// <param name="source">The slot to merge the items from</param>
        /// <param name="destination">The slot to merge the items into</param>
        public void MergeStacks(Slot source, Slot destination)
        {
            //Calculates the max amount of items we are allowed to merge onto the stack
            int max = destination.CurrentItem.maxSize - destination.Items.Count;

            //Sets the correct amount so that we don't put too many items down
            int count = source.Items.Count < max ? source.Items.Count : max;

            for (int i = 0; i < count; i++) //Merges the items into the other stack
            {
                destination.AddItem(source.RemoveItem()); //Removes the items from the source and adds them to the destination
                InventoryManager.Instance.HoverObject.transform.GetChild(0).GetComponent<Text>().text = InventoryManager.Instance.MovingSlot.Items.Count.ToString(); //Updates the text on the stack that
            }
            if (source.Items.Count == 0) //We onto have more items to merge with
            {
                source.ClearSlot();
                Destroy(GameObject.Find("Hover"));
            }
        }


        /// <summary>
        /// Makes the inventory fade out
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeOut()
        {
            if (!fadingOut) //Checks if we are already fading out
            {
                //Sets the current state
                fadingOut = true;
                fadingIn = false;

                //Makes sure that we are not fading out the at same time
                StopCoroutine("FadeIn");

                //Sets the values for fading
                float startAlpha = canvasGroup.alpha;

                float rate = 1.0f / fadeTime; //Calculates the rate, so that we can fade over x amount of seconds

                float progress = 0.0f; //Progresses over the set time


                while (progress < 1.0) //Progresses over the set time
                {
                    canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);  //Lerps from the start alpha to 0 to make the inventory invisible

                    progress += rate * Time.deltaTime; //Adds to the progress so that we will get close to out goal

                    yield return null;
                }

                //Sets the end condition to make sure we are 100% invisible
                canvasGroup.alpha = 0;

                //Sets the status
                fadingOut = false;
            }
        }

        /// <summary>
        /// Makes the inventory fade in
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeIn()
        {
            if (!fadingIn) //Checks if we are already fading out
            {
                //Sets the current state
                fadingOut = false;
                fadingIn = true;

                //Makes sure that we are not fading out the at same time
                StopCoroutine("FadeOut");

                float startAlpha = canvasGroup.alpha; //Sets the start alpha value

                float rate = 1.0f / fadeTime; //Calculates the rate, so that we can fade over x amount of seconds

                float progress = 0.0f; //Resets the progress

                while (progress < 1.0) //Progresses over the set time
                {
                    canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress); //Lerps from the start alpha to 1 to make the inventory visible

                    progress += rate * Time.deltaTime; //Adds to the progress so that we will get close to out goal

                    yield return null;
                }

                //Sets the end condition to make sure we are 100% visible
                canvasGroup.alpha = 1;

                //Sets the status
                fadingIn = false;
            }
        }
    }
}
