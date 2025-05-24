using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public static int max_hp = 30;
    public static int cur_hp = 30;
    public static int atk = 5;

    public Sprite[] images;
    public GameObject enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetImage(int index)
    {
        enemy.GetComponent<SpriteRenderer>().sprite = images[index%2];
    }
}
