using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public Slider castSlider;
    public Slider playerHpSlider;

    private SpellCastManager spellCastManager;
    private PlayerStats playerStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spellCastManager = FindFirstObjectByType<SpellCastManager>();
        playerStats = FindFirstObjectByType<PlayerStats>();

        if (spellCastManager == null)
            Debug.LogError("No SpellCastManager found in the scene!");
    }

    // Update is called once per frame
    void Update()
    {
        // Update casting Slider
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

        //update slider to player hp
        playerHpSlider.value = playerStats.currentHP/100;
    }
}
