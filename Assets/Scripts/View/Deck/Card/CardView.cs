using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public void UpdateCardVisual(bool isSelected) 
    {
        var image = GetComponent<Image>();

        if (image != null) 
        {
            Color color = image.color;
            color.a = isSelected ? 1f : 0.2f;
            image.color = color;
        }
    }

    public void DisplayInformationCard(CardSO cardData)
    {
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();

        foreach (TMP_Text text in texts)
        {
            if (text.gameObject.name == "Name")
            {
                text.text = cardData.Name;
            }

            if (text.gameObject.name == "Type")
            {
                text.text = cardData.Type;
            }

            if (text.gameObject.name == "Description")
            {
                text.text = cardData.Detail.Description.Replace("R", cardData.Detail.RangeSkillImpact.ToString())
                                                        .Replace("V", cardData.Detail.ImpactValue.ToString());
            }
        }
    }
}
