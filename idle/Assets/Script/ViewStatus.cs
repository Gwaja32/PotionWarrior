using TMPro;
using UnityEngine;

public class ViewStatus : MonoBehaviour
{
    public GameObject player_stat;
    public GameObject enemy_stat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player_stat.GetComponent<TextMeshProUGUI>().text = $"ü�� : {Player.max_hp}\n���ݷ� : {Player.atk}";
        enemy_stat.GetComponent<TextMeshProUGUI>().text = $"ü�� : {Enemy.max_hp}\n���ݷ� : {Enemy.atk}";
    }
}
