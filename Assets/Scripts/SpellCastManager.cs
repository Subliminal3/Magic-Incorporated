using System.Collections.Generic;
using UnityEngine;

public class SpellCastManager : MonoBehaviour
{
    public int maxLength = 3;

    [Header("Sounds")]
    public AudioClip crystalCrush;
    public GameObject crystal;

    private AudioSource aSource;
    private bool isCasting = false;
    private List<char> currentSequence = new List<char>();

    // Lambda property for BookText
    public bool IsCasting => isCasting;

    // Exposed for display
    public string CurrentSequence => new string(currentSequence.ToArray());
    public string CurrentSpell { get; private set; } = "";

    // Dictionary mapping sequences to spell names
    private Dictionary<string, string> spellDictionary = new Dictionary<string, string>()
    {
        { "EE", "Push" },
        { "ER", "Fireball" },
        { "RF", "Shield" },
        // add more sequences here
    };

    private void Start()
    {
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

        // Stop casting on left-click and check spell
        if (Input.GetMouseButtonDown(0) && isCasting)
        {
            CheckSpell();
            StopCasting();
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

        if (crystalCrush != null)
            aSource.PlayOneShot(crystalCrush);

        if (crystal != null)
            crystal.SetActive(false);
    }

    private void StopCasting()
    {
        isCasting = false;
        currentSequence.Clear();

        if (crystal != null)
            crystal.SetActive(true);
    }

    private void CheckSpell()
    {
        string sequence = CurrentSequence;

        if (spellDictionary.ContainsKey(sequence))
            CurrentSpell = spellDictionary[sequence];
        else
            CurrentSpell = "Failed";
    }
}
