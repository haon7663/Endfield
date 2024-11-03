using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum MapState
{
    PlayerMap,FinishMap,DefaultMap
}

public enum MapProperty
{
    EnemyMap,Eventmap
}

public class MapIconController : MonoBehaviour
{
    [SerializeField] private List<MapProperty> mapSequence = new List<MapProperty>();

    [SerializeField]  private GameObject mapIconPrefab;
    [SerializeField] private Panel panel;
    [SerializeField] private Sprite enemyIcon, eventIcon;
    [SerializeField] private RectTransform icons;
    [SerializeField]private int showLastMapIndex = 4;
    
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
                case MapProperty.Eventmap:
                    mapIconCS.SetDefaultIcon(eventIcon);
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

  

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))MoveNextMap();
    }

    public void MoveNextMap()
    {
        _mapIcons[_curMapIndex].SetMapState(MapState.FinishMap);
        ++_curMapIndex;
        _curMapIndex = Mathf.Clamp(_curMapIndex,0, mapSequence.Count-1);
       
        _mapIcons[_curMapIndex].SetMapState(MapState.PlayerMap);
        
        if(showLastMapIndex >= mapSequence.Count-1) return;
        if (showLastMapIndex - _curMapIndex <= 1)
        {
            ++showLastMapIndex;
            showLastMapIndex = Mathf.Clamp(showLastMapIndex,0, mapSequence.Count-1);
            Destroy(_mapIcons[_destoryIndex++].gameObject); 
        }
           
        
    }

    
}
