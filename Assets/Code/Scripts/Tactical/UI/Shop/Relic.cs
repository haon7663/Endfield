using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Relic : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] TextMeshPro nameTxt, descriptionTxt;


    public void Init(RelicSO relicSO)
    {
        image.sprite = relicSO.sprite;
        nameTxt.text = relicSO.name;
        descriptionTxt.text = relicSO.description;
    }

}
