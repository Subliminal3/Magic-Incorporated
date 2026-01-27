using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookText : MonoBehaviour
{
    public TMP_Text bookText;
    public TMP_Text spellDisplay;
    public int maxLength = 3;
    public Slider castSlider;

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

        // Update slider (1 to 0)
        if (castSlider == null)
            return;

        if (spellCastManager.IsCasting)
        {
            castSlider.value = spellCastManager.CastTimeNormalized;
        }
        else
        {
            castSlider.value = 0f;
        }
    }
}
