using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class BookText : MonoBehaviour
{
    public TMP_Text bookText;
    public int maxLength = 3; // max letters in sequence
    public GameObject crystal;

    [Header("Sounds")]
    public AudioClip crystalCrush;

    private bool isCasting = false;
    private AudioSource aSource;

    private void Start()
    {
        // Add or get AudioSource
        aSource = gameObject.GetComponent<AudioSource>();
        if (aSource == null)
            aSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        //if right click
        if (Input.GetMouseButtonDown(1) && !isCasting)
        {
            isCasting = true;

            // Play the crystal sound
            if (crystalCrush != null)
                aSource.PlayOneShot(crystalCrush);

            crystal.SetActive(false);
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

            crystal.SetActive(true);
        }
    }
}
