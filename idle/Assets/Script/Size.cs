using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Size : MonoBehaviour
{
    [SerializeField]
    private GameObject Object;
    private bool check = false;

    public void SetEmphasis()
    {
        if (!check)
        {
            StartCoroutine("Emphasis", Object);
            check = true;
        }
        else
        {
            StopCoroutine("Emphasis");
            check = false;
        }
    }

    private IEnumerator Emphasis(GameObject gameObject)
    {
        float increase = 0.1f;

        while (true)
        {
            while (gameObject.GetComponent<Transform>().localScale.x > 0.5f)
            {
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - increase
                                                                , gameObject.transform.localScale.y - increase
                                                                , gameObject.transform.localScale.z - increase);
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.05f);
            while (gameObject.GetComponent<Transform>().localScale.x < 1f)
            {
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + increase
                                                                , gameObject.transform.localScale.y + increase
                                                                , gameObject.transform.localScale.z + increase);
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
