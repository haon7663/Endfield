using UnityEngine;

public class SettingController : MonoBehaviour
{
    [SerializeField] private Canvas settingCanvas,keyLayoutCanvas;

    private bool isKeyLayout => keyLayoutCanvas.gameObject.activeSelf;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isKeyLayout) ActiveKeyLayoutCanvas(!isKeyLayout);
            else settingCanvas.gameObject.SetActive(!settingCanvas.gameObject.activeSelf);
        }
    }

    public void ActiveKeyLayoutCanvas(bool activeKeyCanvas)
    {
        settingCanvas.gameObject.SetActive(!activeKeyCanvas);
        keyLayoutCanvas.gameObject.SetActive(activeKeyCanvas);
    }

    public void ChangeKeyLayout()  //키 레이아웃 변경 할 때
    {
        
    }
}
