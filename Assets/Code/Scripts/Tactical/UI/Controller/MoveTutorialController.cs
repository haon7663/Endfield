using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class MoveTutorialController : MonoBehaviour
{
    [SerializeField] private Panel movePanel,turnPanel;
    [SerializeField] private GameObject[] checkObj;
    [SerializeField] private int tutoIndex;
    [SerializeField]private int turnIndex,moveIndex;
    [SerializeField] private KeyCode  turnKeycode;
    bool isActive;

    void Start()
    {
        transform.position = new Vector3(CameraTransition.Inst.gameObject.transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        if (!GameManager.Inst.isGameActive) return;
        
        if (tutoIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                moveIndex++;
                if (moveIndex >= 3)
                {
                    tutoIndex++;
                    Action1();
                }
            }
        }
        else if (tutoIndex == 1)
        {
            if (Input.GetKeyDown(turnKeycode))
            {
                turnIndex++;
                if (turnIndex >= 2)
                {
                    tutoIndex++;
                    Action2();
                }
            }
        }

    }

    public void Show()
    {
        
        DOVirtual.DelayedCall(1f,()=>
        {
            isActive = true;
            movePanel.SetPosition(PanelStates.Show, true, 0.5f);
        });

    }

    public void Hide()
    {
        isActive = false;
        DOVirtual.DelayedCall(1f,()=>
        {
            movePanel.SetPosition(PanelStates.Hide, true, 0.5f);
            turnPanel.SetPosition(PanelStates.Hide, true, 0.5f);
        });
    }

    private void Action1()
    {
        checkObj[0].SetActive(true);
        turnPanel.SetPosition(PanelStates.Show, true, 0.5f);
    }

    private void Action2()
    {
        
        checkObj[1].SetActive(true);
        Hide();
        GridManager.Inst.GenerateTransitionTiles();
    }


}
