using UnityEngine;
using UnityEngine.UI;

public class MapIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage,iconFrame,iconFnish;
    [SerializeField] private Sprite playerIcon,defaultIcon;

    private MapState _mapState;

    public void SetDefaultIcon(Sprite sprite)
    {
        defaultIcon = sprite;
        iconImage.sprite = defaultIcon;
    }
    public void SetMapState(MapState mapState)
    {
        _mapState = mapState;
        UpdateMapState();
    }
    
    public void UpdateMapState()
    {
        iconFrame.gameObject.SetActive(false);
        switch (_mapState)
        {
            case MapState.PlayerMap:
                iconImage.sprite = playerIcon;
                iconFrame.gameObject.SetActive(true);
                break;
            case MapState.DefaultMap:
                iconImage.sprite = defaultIcon;
                break;
            case MapState.FinishMap:
                iconImage.sprite = defaultIcon;
                iconFnish.gameObject.SetActive(true);
                break;
        }
    }
}
