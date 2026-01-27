using TMPro;
using UnityEngine;

public class BookText : MonoBehaviour
{
    public TMP_Text bookText;
    public TMP_Text spellDisplay;
    public int maxLength = 3;

    private SpellCastManager spellCastManager;

    private void Start()
    {
        spellCastManager = FindFirstObjectByType<SpellCastManager>();

        if (spellCastManager == null)
            Debug.LogError("No SpellCastManager found in the scene!");
    }

    private void Update()
    {
        if (spellCastManager == null)
            return;

        // Add letters while casting
        if (spellCastManager.IsCasting && bookText.text.Length < maxLength)
        {
            if (Input.GetKeyDown(KeyCode.E))
                spellCastManager.AddLetter('E');
            else if (Input.GetKeyDown(KeyCode.R))
                spellCastManager.AddLetter('R');
            else if (Input.GetKeyDown(KeyCode.F))
                spellCastManager.AddLetter('F');
        }

        // Update display
        bookText.text = spellCastManager.CurrentSequence;
        spellDisplay.text = spellCastManager.CurrentSpell;
    }
}
