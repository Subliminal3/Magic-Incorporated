using System.Collections.Generic;
using UnityEngine;

public class SpellCastManager : MonoBehaviour
{
    public int maxLength = 3;
    public GameObject crystal;

    [Header("Sounds")]
    public AudioClip crystalCrush;
    public AudioClip fizzle;
    public AudioClip spellCast;


    [Header("Timer")]
    public float maxCastTime = 2f; // 5 seconds to cast
    private float currentCastTime = 0f;

    private AudioSource aSource;
    private bool isCasting = false;
    private List<char> currentSequence = new List<char>();

    private SpellEffects spellEffects;

    // Lambda property for BookText
    public bool IsCasting => isCasting;

    // Exposed for display
    public string CurrentSequence => new string(currentSequence.ToArray());
    public string CurrentSpell { get; private set; } = "";
    public float CastTimeNormalized => currentCastTime / maxCastTime; // 0–1 for UI

    // Dictionary mapping sequences to spell names
    private Dictionary<string, string> spellDictionary = new Dictionary<string, string>()
    {
        { "EE", "Push" },
        { "ER", "Pull" },
        { "RF", "Shield" },
        // add more sequences here
    };

    private void Start()
    {
        spellEffects = GetComponent<SpellEffects>();

        aSource = GetComponent<AudioSource>();
        if (aSource == null)
            aSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        // Start casting on right-click
        if (Input.GetMouseButtonDown(1) && !isCasting)
        {
            StartCasting();
        }

        // Update timer
        if (isCasting)
        {
            currentCastTime -= Time.deltaTime;
            if (currentCastTime <= 0f)
            {
                //Fizzle();// time ran out
                StopCasting();
            }
        }

        // Stop casting on left-click and check spell
        if (Input.GetMouseButtonDown(0) && isCasting)
        {
            // Dont stop casting until time runs out or correct spell is cast
            if (CheckSpell())
            {
                StopCasting();
            }
            else Fizzle();

        }
    }

    public void AddLetter(char c)
    {
        if (!isCasting) return;
        if (currentSequence.Count >= maxLength) return;

        currentSequence.Add(c);
    }

    private void StartCasting()
    {
        isCasting = true;
        currentSequence.Clear();
        CurrentSpell = "";
        currentCastTime = maxCastTime;

        if (crystalCrush != null)
            aSource.PlayOneShot(crystalCrush);

        if (crystal != null)
            crystal.SetActive(false);
    }

    private void Fizzle()
    {
        CurrentSpell = "Fizzle";

        currentSequence.Clear();

        aSource.PlayOneShot(fizzle);
    }

    private void StopCasting()
    {
        isCasting = false;
        currentSequence.Clear();
        currentCastTime = 0f;

        if (crystal != null)
            crystal.SetActive(true);
    }

    private bool CheckSpell()
    {
        string sequence = CurrentSequence;

        if (spellDictionary.TryGetValue(sequence, out string spell))
        {
            CurrentSpell = spell;
            aSource.PlayOneShot(spellCast);

            spellEffects?.CastSpell(spell);

            return true;
        }

        CurrentSpell = "Failed";
        return false; // wrong spell
    }
}
