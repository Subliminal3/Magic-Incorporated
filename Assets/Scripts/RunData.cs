using UnityEngine;

public class RunData : MonoBehaviour
{
    public static RunData Instance;

    public HexCell hexData;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
