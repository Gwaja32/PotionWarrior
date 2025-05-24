using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static int max_hp = 100;
    public static int cur_hp = 100;
    public static int atk = 10;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static void SendDamage(int damage)
    {
        cur_hp -= damage;
    }
}
