using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    /// <summary>
    /// The player's movement speed
    /// </summary>
    public float speed;

    /// <summary>
    /// A reference to the inventory
    /// </summary>
    public Inventory inventory;

    private Inventory chest;

    public Text helperText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.B))
        {
            inventory.Open();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (chest != null)
            {
                chest.Open();
            }
        }
    }

    /// <summary>
    /// Handles the players movement
    /// </summary>
    private void HandleMovement()
    {
        //Calculates the players translation so that we will move framerate independent
        float translation = speed * Time.deltaTime;

        //Moves the player
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * translation, 0, Input.GetAxis("Vertical") * translation));
    }

    /// <summary>
    /// Handles the player's collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item") //If we collide with an item that we can pick up
        {
            inventory.AddItem(other.GetComponent<Item>()); //Adds the item to the inventory.
        }
        if (other.tag == "Chest")
        {
            helperText.gameObject.SetActive(true);
            chest = other.GetComponent<ChestScript>().chestInventory;
        }
    }


    /// <summary>
    /// Handles the player's trigger collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Chest") //If we collide with a chest
        {
            helperText.gameObject.SetActive(false);

            if (chest.IsOpen)
            {
                chest.Open(); //This will close the chest if the player runs away from the chest
            }
            chest = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Item") //If we collide with an item that we can pick up
        {
            inventory.AddItem(collision.gameObject.GetComponent<Item>()); //Adds the item to the inventory.

            Destroy(collision.gameObject);
        }
    }
}
