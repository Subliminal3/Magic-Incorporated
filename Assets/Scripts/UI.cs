using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public Slider castSlider;

    private SpellCastManager spellCastManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spellCastManager = FindFirstObjectByType<SpellCastManager>();

        if (spellCastManager == null)
            Debug.LogError("No SpellCastManager found in the scene!");
    }

    // Update is called once per frame
    void Update()
    {
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
