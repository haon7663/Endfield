using System;
using System.Collections.Generic;
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
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject); // 자식 오브젝트 제거
        }
        
        List<Skill> skills = DataManager.Inst.Data.skills;
        foreach (var skill in skills)
        {
            GameObject inven = Instantiate(skillPrefab, inventoryContent);
            if(inven.TryGetComponent(out InventorySkillInfor inventorySkillInfor))
                inventorySkillInfor.SetInfor(skill);
        }
      
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
