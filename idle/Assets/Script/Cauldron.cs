using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour
{
    enum MixItem {none, salt, atk, health, heal};
    public static int state = 0;
    private int feather = 0;
    private int heart = 0;
    private int life = 0;
    private int salt = 0;

    public GameObject ob_cauldron;
    public GameObject ob_feather;
    public GameObject ob_heart;
    public GameObject ob_life;
    public GameObject ob_life2;
    public GameObject ob_salt;
    public GameObject ob_text;
    public TextMeshProUGUI text;

    public GameObject ob_button;

    public GameManager gameManager;

    private float time = 0;

    public Sprite[] images;

    void Init()
    {
        ob_cauldron.SetActive(true);
        ob_text.SetActive(false);
        ob_feather.SetActive(false);
        ob_heart.SetActive(false);
        ob_life.SetActive(false);
        ob_life2.SetActive(false);
        ob_salt.SetActive(false);
        feather = 0; heart = 0; life = 0; salt = 0; state = 0;
        SetCauldron(0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            ob_text.SetActive(false);
        }

        if (gameManager.GetComponent<GameManager>().IsAuto())
        {
            if (state != 0 && state != 1) return;
            Clear();
            if (Player.cur_hp <= Player.max_hp * 0.25)
            {
                if (Inventory.inventory[2].amount >= 2)
                {
                    Inventory.inventory[2].amount -= 2;
                    state = 1; life = 2;
                    Mix();
                }
            }

            if (Inventory.inventory[3].amount == 0)
            {
                if (Inventory.inventory[0].amount >= 1)
                {
                    Inventory.inventory[0].amount--;
                    feather = 1; state = 1;
                    Mix();
                }
                else if (Inventory.inventory[1].amount >= 1)
                {
                    Inventory.inventory[1].amount--;
                    heart = 1; state = 1;
                    Mix();
                }
                else if (Inventory.inventory[2].amount >= 1)
                {
                    //Inventory.inventory[2].amount--;
                    //life = 1; state = 1;
                    //Mix();
                }
            }
            else
            {
                if (Inventory.inventory[0].amount >= 1)
                {
                    Inventory.inventory[0].amount--;
                    Inventory.inventory[3].amount--;
                    feather = 1; state = 1; salt = 1;
                    Mix();
                }
                else if (Inventory.inventory[1].amount >= 1)
                {
                    Inventory.inventory[1].amount--;
                    Inventory.inventory[3].amount--;
                    heart = 1; state = 1; salt = 1;
                    Mix();
                }
            }
        }
    }

    public void Clear()
    {
        Inventory.inventory[0].amount += feather;
        feather = 0;

        Inventory.inventory[1].amount += heart;
        heart = 0;

        Inventory.inventory[2].amount += life;
        life = 0;

        Inventory.inventory[3].amount += salt;
        salt = 0;

        ob_feather.SetActive(false);
        ob_heart.SetActive(false);
        ob_life.SetActive(false);
        ob_life2.SetActive(false);
        ob_salt.SetActive(false);
        SetButtonImage(false);
    }

    public void SetCauldron(int index)
    {
        ob_cauldron.GetComponent<Image>().sprite = images[index];
    }


    public void SetFeather()
    {
        if (feather == 0)
        {
            if (state == 1)
            {
                if (Inventory.inventory[0].amount > 0)
                {
                    Inventory.inventory[0].amount -= 1;
                    feather = 1;
                    ob_feather.SetActive(true);
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.아이템넣기);
                }
            }
            else
            {
                ob_text.SetActive (true);
                time = 3f;
            }
        }
        else
        {
            if (feather == 1)
            {
                feather = 0;
                Inventory.inventory[0].amount += 1;
                ob_feather.SetActive(false);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.아이템빼기);
            }
        }

        if (IsMixable() == MixItem.none)
        {
            SetButtonImage(false);
        }
        else
        {
            SetButtonImage(true);
        }
    }

    public void SetHeart()
    {
        if (heart == 0)
        {
            if (state == 1)
            {
                if (Inventory.inventory[1].amount > 0)
                {
                    Inventory.inventory[1].amount -= 1;
                    heart = 1;
                    ob_heart.SetActive(true);
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.아이템넣기);
                }
            }
            else
            {
                ob_text.SetActive(true);
                time = 3f;
            }
        }
        else
        {
            if (heart == 1)
            {
                heart = 0;
                Inventory.inventory[1].amount += 1;
                ob_heart.SetActive(false);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.아이템빼기);
            }
        }
        if (IsMixable() == MixItem.none)
        {
            SetButtonImage(false);
        }
        else
        {
            SetButtonImage(true);
        }
    }

    public void SetLife(bool boolean)
    {
        if (life == 0)
        {
            if (state == 1)
            {
                if (Inventory.inventory[2].amount > 0)
                {
                    Inventory.inventory[2].amount -= 1;
                    life = 1;
                    ob_life.SetActive(true);
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.아이템넣기);
                }
            }
            else
            {
                ob_text.SetActive(true);
                time = 3f;
            }
        }
        else
        {
            if (life == 1)
            {
                if (boolean)
                {
                    if (Inventory.inventory[2].amount > 0)
                    {
                        Inventory.inventory[2].amount -= 1;
                        life = 2;
                        ob_life2.SetActive(true);
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.아이템넣기);
                    }
                }
                else
                {
                    life = 0;
                    Inventory.inventory[2].amount += 1;
                    ob_life.SetActive(false);
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.아이템빼기);
                }
            }
            else if (life == 2)
            {
                life = 1;
                Inventory.inventory[2].amount += 1;
                ob_life2.SetActive(false);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.아이템빼기);
            }
        }

        if (IsMixable() == MixItem.none)
        {
            SetButtonImage(false);
        }
        else
        {
            SetButtonImage(true);
        }
    }

    public void SetSalt()
    {
        if (salt == 0)
        {
            if (state == 1)
            {
                if (Inventory.inventory[3].amount > 0)
                {
                    Inventory.inventory[3].amount -= 1;
                    salt = 1;
                    ob_salt.SetActive(true);
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.아이템넣기);
                }
            }
            else
            {
                ob_text.SetActive(true);
                time = 3f;
            }
        }
        else
        {
            if (salt == 1)
            {
                salt = 0;
                Inventory.inventory[3].amount += 1;
                ob_salt.SetActive(false);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.아이템빼기);
            }
        }

        if (IsMixable() == MixItem.none)
        {
            SetButtonImage(false);
        }
        else
        {
            SetButtonImage(true);
        }
    }

    public void SetWater()
    {
        if (state == 0)
        {
            state = 1;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.물넣기);
            SetCauldron(1);
        }
    }

    public void SetButtonImage(bool boolean)
    {
        ob_button.GetComponent<MixButton>().SetImage(boolean);
    }

    private MixItem IsMixable()
    {
        if (state == 1 && ((feather == 1 && heart == 0 && life == 0 && salt == 0) || (feather == 0 && heart == 1 && life == 0 && salt == 0) || (feather == 0 && heart == 0 && life == 1 && salt == 0)))
        {
            return MixItem.salt;
        }
        else if ((state == 1 && feather == 1 && heart == 0 && life == 0 && salt == 1))
        {
            return MixItem.atk;
        }
        else if ((state == 1 && feather == 0 && heart == 1 && life == 0 && salt == 1))
        {
            return MixItem.health;  
        }
        else if ((state == 1 && feather == 0 && heart == 0 && life == 2 && salt == 0))
        {
            return MixItem.heal;
        }
        return MixItem.none;
    }

    public void Mix()
    {
        MixItem index;
        index = IsMixable();
        if (index != MixItem.none)
        {
            SetButtonImage(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.물약제조);
            Init();
            if (index == MixItem.salt)
            {
                Inventory.inventory[3].amount += 1;
            }
            else if (index == MixItem.atk)
            {
                state = 2;
                gameManager.PotionCount(0);
                SetCauldron(2);
                Invoke("DrinkAtk", 1f);
            }
            else if (index == MixItem.health)
            {
                state = 2;
                gameManager.PotionCount(1);
                SetCauldron(3);
                Invoke("DrinkHealth", 1f);
            }
            else if (index == MixItem.heal)
            {
                state = 2;
                gameManager.PotionCount(2);
                SetCauldron(4);
                Invoke("DrinkHeal", 1f);
            }
        }
    }

    public void DrinkAtk()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.마시기);
        Init();
        Player.atk += 10;
    }

    public void DrinkHealth()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.마시기);
        Init();
        Player.cur_hp += 50;
        Player.max_hp += 50;
    }

    public void DrinkHeal()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.마시기);
        Init();
        Player.cur_hp += (int) (Player.max_hp * 0.8);
        if (Player.cur_hp > Player.max_hp)
        {
            Player.cur_hp = Player.max_hp;
        }
    }


}
