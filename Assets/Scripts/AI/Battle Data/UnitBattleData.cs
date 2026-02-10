using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UnitBattleData : MonoBehaviour
{
    public UnitData data;
    public int quantity = 0;

    private TMP_Text quantityText;

    private void Awake()
    {
        Image thumbnail = transform.Find("Thumbnail")?.GetComponentInChildren<Image>();
        TMP_Text nameText = transform.Find("Name")?.GetComponent<TMP_Text>();
        TMP_Text description = transform.Find("Description")?.GetComponent<TMP_Text>();
        quantityText = transform.Find("ButtonGroup/Quantity")?.GetComponent<TMP_Text>();


        if (data.unitName == null) return;
        nameText.text = data.unitName;

        if (data.description == null) return;
        description.text = data.description;

        if (data.thumbnail == null) return;
        thumbnail.sprite = data.thumbnail;
    }

    private void Update()
    {
        quantityText.text = quantity.ToString();
    }

    public void IncreaseQuantity()
    {
        quantity++;
    }

    public void DecreaseQuantity()
    {
        if (quantity > 0)
            quantity--;
    }

}
