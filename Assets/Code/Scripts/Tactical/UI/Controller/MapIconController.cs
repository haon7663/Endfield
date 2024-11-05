using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum MapState
{
    PlayerMap,FinishMap,DefaultMap
}

public enum MapProperty
{
    EnemyMap,EventMap,ShopMap,BossMap
}

public class MapIconController : MonoBehaviour
{
    [SerializeField] private List<MapProperty> mapSequence = new List<MapProperty>();

    [SerializeField]  private GameObject mapIconPrefab;
    [SerializeField] private Panel panel;
    [SerializeField] private Sprite enemyIcon, eventIcon,shopIcon,bossIcon;
    [SerializeField] private RectTransform icons;
    [SerializeField] private EventController eventController;
    [SerializeField] private ShopController shopController;
    private int showLastMapIndex = 4;
  
    
    private List<MapIcon> _mapIcons = new List<MapIcon>();
    private int _curMapIndex = 0;
    private int _destoryIndex = 0;

    private void Awake()
    {
        for (int i = 0; i < mapSequence.Count; i++)
        {
            GameObject _icon =  Instantiate(mapIconPrefab, icons);
            MapIcon mapIconCS = _icon.GetComponent<MapIcon>();
            switch (mapSequence[i])
            {
                case MapProperty.EnemyMap:
                    mapIconCS.SetDefaultIcon(enemyIcon);
                    break;
                case MapProperty.EventMap:
                    mapIconCS.SetDefaultIcon(eventIcon);
                    break;
                case MapProperty.ShopMap:
                    mapIconCS.SetDefaultIcon(shopIcon);
                    break;
                case MapProperty.BossMap:
                    mapIconCS.SetDefaultIcon(bossIcon);
                    break;
            }
            _mapIcons.Add(mapIconCS);
        }
        
        foreach (Transform child in icons)
        {
            _mapIcons.Add(child.GetComponent<MapIcon>());
        }
        _mapIcons[_curMapIndex].SetMapState(MapState.PlayerMap);
       
   
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => GridManager.Inst);
        Instance();
    }


    public void Show()
    {
        panel.SetPosition(PanelStates.Show, true);
        MoveNextMap();
    }
    
    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true);
    }

    public void Instance()
    {
        _curMapIndex = DataManager.Inst.Data.stageCount;

        switch (mapSequence[_curMapIndex])
        {
            case MapProperty.EnemyMap:
                Debug.Log("�� ����");
                break;
            case MapProperty.BossMap:
                Debug.Log("���� ����");
                break;
            case MapProperty.EventMap:
                GridManager.Inst.GenerateTransitionTiles();
                GameManager.Inst.MapIconShow(true);
                eventController.Show();
                SpawnManager.Inst.DoNotSpawn();              
                break;
            case MapProperty.ShopMap:
                GridManager.Inst.GenerateTransitionTiles();
                GameManager.Inst.MapIconShow(true);
                shopController.Active();
                SpawnManager.Inst.DoNotSpawn();
                break;
        }
        SpawnManager.Inst.SpawnEnemies();
    }

    public void MoveNextMap()
    {
        _curMapIndex = DataManager.Inst.Data.stageCount;
        if (_curMapIndex >= mapSequence.Count)
        {
            Debug.Log("게임 끝");
            //게임끝 함수
        }
        
        for (int i = 0; i < _curMapIndex; i++)
        {
            _mapIcons[i].SetMapState(MapState.FinishMap);
        }
       
       
        _curMapIndex = Mathf.Clamp(_curMapIndex,0, mapSequence.Count-1);
       
        _mapIcons[_curMapIndex].SetMapState(MapState.PlayerMap);
        if (_curMapIndex >= 3)
        {
            for (int i = 0; i < _curMapIndex - 2; i++)
            {
                if(i>=mapSequence.Count-1-4) return;
                Destroy(_mapIcons[i].gameObject); 
            }
        }


        /*
        if(showLastMapIndex >= mapSequence.Count-1) return;
        if (showLastMapIndex - _curMapIndex <= 1)
        {
            ++showLastMapIndex;
            showLastMapIndex = Mathf.Clamp(showLastMapIndex,0, mapSequence.Count-1);
            Destroy(_mapIcons[_destoryIndex++].gameObject); 
        }*/


    }

    
}
