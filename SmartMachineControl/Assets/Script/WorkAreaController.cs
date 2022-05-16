using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class WorkAreaController : MonoBehaviour
{
    public GameObject canvas;

    GameObject clickedButton;
    
    bool sizeChangeFlag;
    bool RotateChangeFlag;
    bool MoveFlag;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ButtonCheck();
    }

    public void SizeChange()
    {
        Debug.Log("SizeChange");
        sizeChangeFlag = !sizeChangeFlag;
    }
    public void Move()
    {
        Debug.Log("Move");
        MoveFlag = !MoveFlag;
    }
    public void RotateChange()
    {
        Debug.Log("RotateChange");
        RotateChangeFlag = !RotateChangeFlag;
    }
    void ButtonCheck()
    {
        bool[] flagIndex = {sizeChangeFlag, MoveFlag, RotateChangeFlag};
        bool flagCheck = false;
        for (int i = 0; i < flagIndex.Length; i++)
        {
            GameObject canvasButton = canvas.transform.GetChild(i).gameObject;
            if (flagIndex[i])
            {
                canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.pressedColor;
                for (int j = 0; j < canvas.transform.childCount; j++)
                {
                    if (i != j)
                    {
                        canvasButton = canvas.transform.GetChild(j).gameObject;
                        canvasButton.SetActive(false);
                    }
                }
                flagCheck = true;
            }
            else
            {
                canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.normalColor;
            
            }
            
            if ((i == flagIndex.Length - 1) && !flagCheck)
            {
                for (int j = 0; j < canvas.transform.childCount; j++)
                {
                    canvasButton = canvas.transform.GetChild(j).gameObject;
                    canvasButton.SetActive(true);
                }
            }
        }
    }
}
