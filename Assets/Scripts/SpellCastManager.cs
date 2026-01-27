using UnityEngine;

public class SpellCastManager : MonoBehaviour
{
    [Header("Sounds")]
    public AudioClip crystalCrush;
    public GameObject crystal;

    private AudioSource aSource;
    private bool isCasting = false;

    // Lambda property to expose casting state
    public bool IsCasting => isCasting;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
        if (aSource == null)
            aSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        // Start casting on right click
        if (Input.GetMouseButtonDown(1) && !isCasting)
        {
            isCasting = true;

            if (crystalCrush != null)
                aSource.PlayOneShot(crystalCrush);

            if (crystal != null)
                crystal.SetActive(false);
        }

        // Stop casting on left click
        if (Input.GetMouseButtonDown(0) && isCasting)
        {
            isCasting = false;

            if (crystal != null)
                crystal.SetActive(true);
        }
    }
}
