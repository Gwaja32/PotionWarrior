using TMPro;
using UnityEngine.UI;
using UnityEngine;
using NUnit.Framework;
using System.Collections;
using UnityEngine.InputSystem.Controls;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Vector3 boss_start = new Vector3(1f, 4.5f, 0f);
    private Vector3 boss_target = new Vector3(0.35f, 3.7f, 0f);

    public TextMeshProUGUI text;
    public TextMeshProUGUI playerHpText;
    public TextMeshProUGUI enemyHpText;
    public TextMeshProUGUI pickText;

    public GameObject ob_pick;
    public GameObject ob_select1;
    public GameObject ob_select2;

    public GameObject ob_player;
    public GameObject ob_enemy;
    public GameObject ob_boss;
    public GameObject ob_background;

    public GameObject boss_panel;
    public GameObject boss_Image;

    public GameObject ob_gameover;

    public GameObject ob_autobutton;
    public GameObject ob_autocheck;

    public GameObject ob_clear;
    public TextMeshProUGUI clear_text1;
    public TextMeshProUGUI clear_text2;
    public TextMeshProUGUI clear_text3;

    private float time = 600.0f; // 10분
    private float pickTime = 10.0f;
    private float delay = 0.0f;
    private readonly float TIME = 1.0f;
    private State state = State.None;
    private int stage = 1;
    private int auto = 0;

    private int[] enemy_hp = new int[] {20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 200};
    private int[] enemy_atk = new int[] {5, 5, 5, 7, 7, 7, 9, 9, 9, 11, 11, 11, 13, 13, 15};

    private int enemy_state = 0;
    private int player_state = 0;

    private readonly float rotate_speed = 90f;

    private int[] potion = new int[] { 0, 0, 0 };

    void Start()
    {
        //player = GameObject.Find("Player").GetComponent<Player>();
        //enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        boss_Image.SetActive(false);
        boss_panel.SetActive(false);
        ob_clear.SetActive(false);
        Player.atk = 10;
        Player.max_hp = 100;
        Player.cur_hp = 100;
        Enemy.cur_hp = enemy_hp[0];
        Enemy.max_hp = enemy_hp[0];
        Enemy.atk = enemy_atk[0];
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        delay -= Time.deltaTime;
        text.text = $"{Mathf.Floor(time / 60.0f) }분 {Mathf.Floor(time % 60.0f)}초";
        pickText.text = $"{Mathf.Floor(pickTime)}초 후에 자동으로 선택됩니다";
        playerHpText.text = $"{Player.cur_hp} / {Player.max_hp}";
        enemyHpText.text = $"{Enemy.cur_hp} / {Enemy.max_hp}";

        if (stage == 15)
        {
            text.text = "보스 스테이지";
        }
        else
        {
            if (time < 0)
            {
                SetBossStage();
            }
        }

        if (state == State.BattleEnd)
        {
            pickTime -= Time.deltaTime;
            if (pickTime < 0)
            {
                Select1();
            }
        }

        if (player_state == 1)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, -30);
            ob_player.GetComponent<Transform>().rotation = Quaternion.RotateTowards(ob_player.GetComponent<Transform>().rotation, targetRotation, rotate_speed * Time.deltaTime);
            if (ob_player.GetComponent<Transform>().rotation == targetRotation) {
                player_state = 0;
            }
        }
        else if (player_state == 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            ob_player.GetComponent<Transform>().rotation = Quaternion.RotateTowards(ob_player.GetComponent<Transform>().rotation, targetRotation, rotate_speed * Time.deltaTime);
        }

        if (enemy_state == 1)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, 30);
            ob_enemy.GetComponent<Transform>().rotation = Quaternion.RotateTowards(ob_enemy.GetComponent<Transform>().rotation, targetRotation, rotate_speed * Time.deltaTime);
            if (ob_enemy.GetComponent<Transform>().rotation == targetRotation)
            {
                enemy_state = 0;
            }
        }
        else if (enemy_state == 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            ob_enemy.GetComponent<Transform>().rotation = Quaternion.RotateTowards(ob_enemy.GetComponent<Transform>().rotation, targetRotation, rotate_speed * Time.deltaTime);
        }
        else if (enemy_state == 100)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, 30);
            ob_boss.GetComponent<Transform>().rotation = Quaternion.RotateTowards(ob_boss.GetComponent<Transform>().rotation, targetRotation, rotate_speed * Time.deltaTime * 4);
            if (ob_boss.GetComponent<Transform>().rotation == targetRotation)
            {
                enemy_state = 101;
            }
        }
        else if (enemy_state == 101)
        {
            ob_boss.transform.position = Vector3.MoveTowards(ob_boss.transform.position, boss_target, 0.02f);
            if (ob_boss.transform.position == boss_target)
            {
                enemy_state = 102;
            }
        }
        else if (enemy_state == 102)
        {
            ob_boss.transform.position = Vector3.MoveTowards(ob_boss.transform.position, boss_start, 0.02f);
            if (ob_boss.transform.position == boss_start)
            {
                enemy_state = 103;
            }
        }
        else if (enemy_state == 103)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            ob_boss.GetComponent<Transform>().rotation = Quaternion.RotateTowards(ob_boss.GetComponent<Transform>().rotation, targetRotation, rotate_speed * Time.deltaTime);
        }

        if (delay <= 0.0f)
        {
            EventCheck();
            InitDelay();
        }
    }

    public void AutoToggle()
    {
        if (auto == 0)
        {
            auto = 1;
            ob_autocheck.SetActive(true);
            ob_autobutton.GetComponent<Image>().color = Color.green;
        }
        else
        {
            auto = 0;
            ob_autocheck.SetActive(false);
            ob_autobutton.GetComponent<Image>().color = Color.red;
        }
    }

    public bool IsAuto()
    {
        if (auto == 1)
        {
            return true;
        }
        return false;
    }

    private void EventCheck()
    {
        if (state == State.None)
        {
            SetStatePlayerTurn();
        } 
        else if (state == State.PlayerTurn)
        {
            if (Enemy.cur_hp <= 0.0f)
            {
                if (stage == 15)
                {
                    SetStateGameClear();
                }
                else
                {
                    SetStateBattleEnd();
                }
            } else
            {
                SetStateEnemyTurn();
            }
        }
        else if (state == State.EnemyTurn)
        {
            if (Player.cur_hp <= 0.0f)
            {
                SetStateGameOver();
            }
            else
            {
                SetStatePlayerTurn();
            }
        }
    }

    public void Select1()
    {
        Item item = new Item(ob_select1.GetComponent<Image>().sprite, 1);
        Inventory.addItem(item);
        NextStage();
    }

    public void Select2()
    {
        Item item = new Item(ob_select2.GetComponent<Image>().sprite, 1);
        Inventory.addItem(item);
        NextStage();
    }

    private void MixPick()
    {
        List<int> list = new List<int> { 0, 1, 2 };
        int number1 = Random.Range(0, list.Count);
        list.RemoveAt(number1);
        int number2 = Random.Range(0, list.Count);
        number2 = list[number2];

        ob_select1.GetComponent<Image>().sprite = Inventory.inventory[number1].image;
        ob_select2.GetComponent<Image>().sprite = Inventory.inventory[number2].image;
    }

    private void InitDelay()
    {
        delay = TIME; 
    }

    private void SetStateNone()
    {
        state = State.None;
        ob_pick.SetActive(false);
    }
    private void SetStatePlayerTurn()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.타격);
        Enemy.cur_hp -= Player.atk;
        state = State.PlayerTurn;
        player_state = 1;
        ob_pick.SetActive(false);
    }
    private void SetStateEnemyTurn()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.피격);
        Player.cur_hp -= Enemy.atk;
        state = State.EnemyTurn;
        if (stage == 15)
        {
            enemy_state = 100;
        }
        else
        {
            enemy_state = 1;
        }
        ob_pick.SetActive(false);
    }
    private void SetStateBattleEnd()
    {
        state = State.BattleEnd;
        pickTime = 10f;
        MixPick();
        ob_pick.SetActive(true);
    }
    private void SetStateGameOver()
    {
        state = State.GameOver;
        ob_pick.SetActive(false);
        GameOver();
    }

    private void SetStateEvent()
    {
        state = State.Event;
        ob_pick.SetActive(false);
    }

    private void SetStateGameClear()
    {
        state = State.GameClear;
        clear_text1.text = $"x{potion[0]}";
        clear_text2.text = $"x{potion[1]}";
        clear_text3.text = $"x{potion[2]}";
        ob_clear.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.팡파레);
    }

    private void WarningSfx()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.경고음);
    }

    private void BossSfx()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.보스출현);
    }

    private void NextStage()
    {
        stage++;
        if (stage == 15)
        {
            SetBossStage();
        }
        else
        {
            Enemy.max_hp = enemy_hp[stage - 1];
            Enemy.cur_hp = Enemy.max_hp;
            Enemy.atk = enemy_atk[stage - 1];
            ob_enemy.GetComponent<Enemy>().SetImage(stage - 1);
            ob_background.GetComponent<Background>().SetImage(stage - 1);
            InitDelay();
            SetStatePlayerTurn();
        }
    }

    public void SetBossStage()
    {
        stage = 15;
        Enemy.max_hp = enemy_hp[stage - 1];
        Enemy.cur_hp = Enemy.max_hp;
        Enemy.atk = enemy_atk[stage - 1];
        ob_enemy.SetActive(false);
        ob_boss.SetActive(true);
        ob_background.GetComponent<Background>().SetBossImage();
        SetStateEvent();
        boss_panel.SetActive(true);
        boss_Image.SetActive(true);
        boss_Image.GetComponent<Size>().SetEmphasis();
        Invoke("WarningSfx", 0f);
        Invoke("WarningSfx", 1f);
        Invoke("WarningSfx", 2f);
        Invoke("BossSfx", 3f);
        Invoke("SetBossStart", 3f);
        Invoke("SetStatePlayerTurn", 4f);
        Invoke("InitDelay", 4f);
    }

    public void SetBossStart()
    {
        boss_Image.GetComponent<Size>().SetEmphasis();
        boss_Image.SetActive(false);
        boss_panel.SetActive(false);
    }

    public void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        ob_gameover.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.실패);
    }

    public void PotionCount(int index)
    {
        potion[index]++;
    }
}
