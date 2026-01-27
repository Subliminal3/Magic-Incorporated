using TMPro;
using UnityEngine;

public class BookText : MonoBehaviour
{
    public TMP_Text bookText;
    public int maxLength = 3;

    private SpellCastManager spellCastManager; // assign in Inspector

    private void Start()
    {
        // Find the SpellCastManager in the scene
        spellCastManager = FindFirstObjectByType<SpellCastManager>();

        if (spellCastManager == null)
            Debug.LogError("No SpellCastManager found in the scene!");
    }

    private void Update()
    {
        //clear text when left-click
        if (Input.GetMouseButtonDown(0))
        {
            bookText.text = "";
        }

        if (!spellCastManager.IsCasting)
            return;

        // Add letters while casting
        if (bookText.text.Length < maxLength)
        {
            if (Input.GetKeyDown(KeyCode.E))
                bookText.text += "E";
            else if (Input.GetKeyDown(KeyCode.R))
                bookText.text += "R";
            else if (Input.GetKeyDown(KeyCode.F))
                bookText.text += "F";
        }

        //clear text when left-click
        if (Input.GetMouseButtonDown(0))
        {
            bookText.text = "";
        }
    }
}
