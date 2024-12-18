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
    EnemyMap,EventMap,ShopMap,BossMap,MoveTuto,AttackTuto
}

public class MapIconController : MonoBehaviour
{
    [SerializeField] private List<MapProperty> mapSequence = new List<MapProperty>();

    [SerializeField]  private GameObject mapIconPrefab;
    [SerializeField] private Panel panel;
    [SerializeField] private Sprite enemyIcon, eventIcon,shopIcon,bossIcon,tutorialMapIcon;
    [SerializeField] private RectTransform icons;
    [SerializeField] private EventController eventController;
    [SerializeField] private ShopController shopController;
    [SerializeField] private MoveTutorialController moveTutorialController;
    [SerializeField] private AttackTutorialController attackTutorialController;
    [SerializeField] private EndingPanelController endingPanelController;
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
                case MapProperty.MoveTuto:
                    mapIconCS.SetDefaultIcon(tutorialMapIcon);
                    break;
                case MapProperty.AttackTuto:
                    mapIconCS.SetDefaultIcon(tutorialMapIcon);
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
        yield return new WaitUntil(() => DataManager.Inst);
        yield return new WaitUntil(() => GridManager.Inst);
        Instance();
    }


    public void Show()
    {
        panel.SetPosition(PanelStates.Show, true,0.5f);
        MoveNextMap();
    }
    
    public void Hide()
    {
        panel.SetPosition(PanelStates.Hide, true,0.5f);
    }

    public void Instance()
    {
        _curMapIndex = DataManager.Inst.Data.stageCount;
        if (_curMapIndex >= mapSequence.Count)
        {
            endingPanelController.Show();
            return;
        }

        switch (mapSequence[_curMapIndex])
        {
            case MapProperty.EnemyMap:
                GameStartController.Inst.Show();
                SpawnManager.Inst.SpawnEnemies();
                break;
            case MapProperty.BossMap:
                //GameStartController.Inst.Show();
                break;
            case MapProperty.EventMap:
                SpawnManager.Inst.isSpawnEnemy = false;
                GameManager.Inst.MapIconShow(true);
                GridManager.Inst.GenerateTransitionTiles();
                eventController.Show();           
                break;
            case MapProperty.ShopMap:
                SpawnManager.Inst.isSpawnEnemy = false;
                GameManager.Inst.MapIconShow(true);
                GridManager.Inst.GenerateTransitionTiles();
                shopController.Active();
                break;
            case MapProperty.MoveTuto:
                SpawnManager.Inst.isSpawnEnemy = false;
                GameManager.Inst.MapIconShow(true);
                moveTutorialController.Show();
                break;
            case MapProperty.AttackTuto:
                SpawnManager.Inst.isSpawnEnemy = false;
                GameManager.Inst.MapIconShow(true);
                attackTutorialController.Show();
                break;
        }
    }
    
    public void MoveNextMap()
    {
        _curMapIndex = DataManager.Inst.Data.stageCount;
       
        
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
