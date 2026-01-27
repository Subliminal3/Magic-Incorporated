using TMPro;
using UnityEngine;

public class BookText : MonoBehaviour
{
    public TMP_Text bookText;
    public int maxLength = 3; // max letters in sequence

    private bool isCasting = false;


    void Update()
    {
        //if right click
        if (Input.GetMouseButtonDown(1) && !isCasting)
        {
            isCasting = true;
        }

        if (!(bookText.text.Length >= maxLength) && isCasting)
        {
            if (Input.GetKeyDown(KeyCode.E))
                bookText.text += "E";
            else if (Input.GetKeyDown(KeyCode.R))
                bookText.text += "R";
            else if (Input.GetKeyDown(KeyCode.F))
                bookText.text += "F";
        }

        if (Input.GetMouseButtonDown(0) && isCasting)
        {
            bookText.text = "";
            isCasting = false;
        }
    }
}
