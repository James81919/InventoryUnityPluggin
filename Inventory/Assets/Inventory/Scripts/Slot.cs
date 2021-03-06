﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    /// Indicates if the slot is avaialble for stacking more items
    /// </summary>
    public bool IsAvailable
    {
        get {return CurrentItem.maxSize > items.Count; }
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
	void Start () 
    {   
        //Creates a reference to the slot slot's recttransform
        RectTransform slotRect = GetComponent<RectTransform>();

        //Creates a reference to the stackTxt's recttransform
        RectTransform txtRect = stackTxt.GetComponent<RectTransform>();

        //Calculates the scalefactor of the text by taking 60% of the slots width
        int txtScleFactor = (int)(slotRect.sizeDelta.x * 0.60);

        //Sets the min and max textSize of the stackTxt
        stackTxt.resizeTextMaxSize = txtScleFactor;
        stackTxt.resizeTextMinSize = txtScleFactor;

        //Sets the actual size of the txtRect
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
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
        //Sets the neutralsprite
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

                Inventory.EmptySlots++; //Adds 1 to the amount of empty slots
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
        //If the right mousebutton was clicked, and we aren't moving an item and the inventory is visible
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && Inventory.CanvasGroup.alpha > 0)
        {
            //Uses an item on the slot
            UseItem();
        }
        //Checks if we need to show the split stack dialog , this is only done if we shiftclick a slot and we aren't moving an item
        else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && !IsEmpty && !GameObject.Find("Hover"))
        {
            //The dialogs spawnposition
            Vector2 position;

            //Translates the mouse position to onscreen coords so that we can spawn the dialog at the correct position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Inventory.Instance.canvas.transform as RectTransform, Input.mousePosition, Inventory.Instance.canvas.worldCamera, out position);

            //Shows the dialog
            Inventory.Instance.selectStackSize.SetActive(true);

            //Sets the position
            Inventory.Instance.selectStackSize.transform.position = Inventory.Instance.canvas.transform.TransformPoint(position);

            //Tell the inventory the item count on the selected slot
            Inventory.Instance.SetStackInfo(items.Count);
        }
    }
}
