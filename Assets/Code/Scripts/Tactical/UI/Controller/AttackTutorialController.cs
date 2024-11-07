using DG.Tweening;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AttackTutorialController : MonoBehaviour
{
    [SerializeField] private Panel skillPanel,scarecrowPanel;
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [SerializeField] private GameObject[] keySolid;
    private int tutoIndex;
    bool keyJ, keyK, keyL;

    private bool isActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(CameraTransition.Inst.gameObject.transform.position.x, transform.position.y, transform.position.z);
    }

    public void Show()
    {
        isActive = true;
        skillPanel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (tutoIndex == 0)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    keyJ = true;
                    keySolid[0].SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.K))
                {
                    keyK = true;
                    keySolid[1].SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.L))
                {
                    keyL = true;
                    keySolid[2].SetActive(true);
                }

                if (keyJ && keyK && keyL)
                {
                    tutoIndex++;
                    Action1();
                    return;
                }
            }
      
        }

    }

    private void Action1()
    {
        DOVirtual.DelayedCall(1f, () =>
        {
            skillPanel.SetPosition(PanelStates.Hide, true, 0.5f, Ease.OutBack);
            scarecrowPanel.SetPosition(PanelStates.Show, true, 0.5f, Ease.OutBack);
            foreach (GameObject solid in keySolid)
            {
                solid.SetActive(false);
            }
            //허수아비 소환
        });
      
    }

    public void ScarecrowDead()
    {
        GridManager.Inst.GenerateTransitionTiles();
    }
}
