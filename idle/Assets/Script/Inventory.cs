using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Item[] inventory = new Item[5];
    public Sprite[] images;
    public GameObject[] slots;
    private Slot[] slots_com = new Slot[5];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = new Item(images[i], 0);
            slots_com[i] = slots[i].GetComponent<Slot>();
            slots_com[i].image.GetComponent<Image>().sprite = images[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == slots_com.Length - 1)
            {
                slots_com[i].amount.GetComponent<TextMeshProUGUI>().text = "¡Ä";
            }
            else
            {
                slots_com[i].amount.GetComponent<TextMeshProUGUI>().text = inventory[i].amount.ToString();
            }
        }
    }

    public Sprite getImage(int index)
    {
        return images[index];
    }

    public static void addItem(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].Equals(item))
            {
                inventory[i].amount += item.amount;
                break;
            }
        }
    }
}
