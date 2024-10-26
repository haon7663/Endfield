using System;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Transform inventoryContent, relicContent;
    [SerializeField] private GameObject skillPrefab, relicPrefab;

    private void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void AddInventorySkill()
    {
        Instantiate(skillPrefab, inventoryContent);
    }

    public void AddRelic()
    {
        Instantiate(relicPrefab, relicContent);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            inventoryCanvas.gameObject.SetActive(!inventoryCanvas.gameObject.activeSelf);
        }
    }
}
