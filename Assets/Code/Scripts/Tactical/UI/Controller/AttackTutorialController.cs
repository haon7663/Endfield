using DG.Tweening;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AttackTutorialController : MonoBehaviour
{
    [SerializeField] private Panel skillPanel,scarecrowPanel;
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [SerializeField] private GameObject[] keySolid;
    [SerializeField] private GameObject scareCrowCheck;
    private int tutoIndex;
    bool keyJ, keyK, keyL;

    private bool isActive;
    
    private void Start()
    {
        transform.position = new Vector3(CameraTransition.Inst.gameObject.transform.position.x, transform.position.y, transform.position.z);
    }

    public void Show()
    {
        isActive = true;
        DOVirtual.DelayedCall(1f, () => skillPanel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack));
    }
    
    private void Update()
    {
        if (!isActive) return;
        if (!GameManager.Inst.isGameActive) return;
        if (tutoIndex != 0) return;
        
        if (Input.GetKeyUp(KeyCode.J))
        {
            keyJ = true;
            keySolid[0].SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            keyK = true;
            keySolid[1].SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            keyL = true;
            keySolid[2].SetActive(true);
        }

        if (keyJ && keyK && keyL)
        {
            tutoIndex++;
            Action1();
        }
    }

    private void Action1()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            skillPanel.SetPosition(PanelStates.Hide, true, 0.3f, Ease.OutBack);
            scarecrowPanel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
            foreach (GameObject solid in keySolid)
            {
                solid.SetActive(false);
            }
            Unit scareCrow =  SpawnManager.Inst.Summon("Double Flower", GridManager.Inst.GetTile(5));
            scareCrow.Health.onDeath += ScarecrowDead;
        });
      
    }

    public void ScarecrowDead()
    {
        scareCrowCheck.SetActive(true);
        GridManager.Inst.GenerateTransitionTiles();
        DOVirtual.DelayedCall(1f, () =>
        {
            scarecrowPanel.SetPosition(PanelStates.Hide, true, 0.5f, Ease.OutBack);

        });
    }
}
