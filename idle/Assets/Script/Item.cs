using UnityEngine;

public class Item
{
    public Sprite image;
    public int amount;

    public Item(Sprite image, int amount)
    {
        this.image = image;
        this.amount = amount;
    }

    public bool Equals(Item other) { 
        return image.Equals(other.image);
    }
}