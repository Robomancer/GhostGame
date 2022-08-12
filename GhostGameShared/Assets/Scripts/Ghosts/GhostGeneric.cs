using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostGeneric : MonoBehaviour
{
    [SerializeField] private Image TextBox;
    [SerializeField] private Text TextBody;

    private int counter;

    private void Awake()
    {
        counter = 0;
    }

    public IEnumerator OnGhostTouch()
    {
        TextBox.gameObject.SetActive(true);

        switch(counter)
        {
            case 0:
                TextBody.text = "Hello 0.";
                counter++;
                break;
            case 1:
                TextBody.text = "Hello 1.";
                counter++;
                break;
            case 2:
                TextBody.text = "Hello 2.";
                counter++;
                break;
            case 3:
                TextBody.text = "Hello 3.";
                counter = 0;
                break;
        }

        yield return new WaitForSeconds(5);

        TextBox.gameObject.SetActive(false);
    }
}
