using UnityEngine;
using UnityEngine.UI;

public class MixButton : MonoBehaviour
{
    public Sprite[] images;
    public GameObject button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.GetComponent<Image>().sprite = images[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetImage(bool boolean)
    {
        if (boolean)
        {
            button.GetComponent<Image>().sprite = images[1];
        }
        else
        {
            button.GetComponent<Image>().sprite = images[0];
        }
    }


}
