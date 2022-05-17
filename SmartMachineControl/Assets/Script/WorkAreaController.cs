using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class WorkAreaController : MonoBehaviour
{
    public GameObject canvasActiveBtn;
	public GameObject canvasSelectBtn;

	Button OkBtn;
	Button CancelBtn;

	bool[] flagIndex;

	bool sizeChangeFlag;
	bool rotateChangeFlag;
	bool moveFlag;

	// Start is called before the first frame update
	void Start()
    {
		flagIndex = new bool[] {sizeChangeFlag, rotateChangeFlag, moveFlag};

		for (int i = 0; i < flagIndex.Length; i++)
			flagIndex[i] = false;

		Debug.Log("Bool Index Count -> " + flagIndex.Length);
		canvasSelectBtn.SetActive(false);

		OkBtn = canvasSelectBtn.GetComponent<Transform>().GetChild(0).GetComponent<Button>();
		CancelBtn = canvasSelectBtn.GetComponent<Transform>().GetChild(1).GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
		SaveFlagIndex();
        ButtonCheck();
		RotateChange();
		Move();
		SizeChange();
    }
	void SaveFlagIndex() // 배열을 주소값 참조로 변경해야될듯...ㅠㅠㅠㅠㅠㅠㅠ
	{
		flagIndex[0] = sizeChangeFlag;
		flagIndex[1] = moveFlag; 
		flagIndex[2] = rotateChangeFlag;
	}

	public void SizeChangeBtnClick()
    {
        Debug.Log("SizeChange");
        sizeChangeFlag = !sizeChangeFlag;
    }
    public void MoveBtnClick()
    {
        Debug.Log("Move");
        moveFlag = !moveFlag;
    }
    public void RotateChangeBtnClick()
    {
        Debug.Log("RotateChange");
        rotateChangeFlag = !rotateChangeFlag;
    }
	public void OkBtnClick()
	{
		Debug.Log("Ok Button Click");
		canvasSelectBtn.SetActive(false);
		AllFlagOff();
	}
	public void CancelBtnClick()
	{
		Debug.Log("Cancel Button Click");
		canvasSelectBtn.SetActive(false);
		AllFlagOff();

	}
	void AllFlagOff()
	{
		for (int i = 0; i < flagIndex.Length; i++)
		{
			flagIndex[i] = false;
		}
	}
	void ButtonCheck()	
    {
        bool flagCheck = false;
		for (int i = 0; i < flagIndex.Length; i++)
        {
            GameObject canvasButton = canvasActiveBtn.transform.GetChild(i).gameObject;

            if (flagIndex[i])
            {
                canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.pressedColor;
                for (int j = 0; j < canvasActiveBtn.transform.childCount; j++)
                {
                    if (i != j)
                    {
                        canvasButton = canvasActiveBtn.transform.GetChild(j).gameObject;
                        canvasButton.SetActive(false);
						canvasSelectBtn.SetActive(true);
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
                for (int j = 0; j < canvasActiveBtn.transform.childCount; j++)
                {
					canvasButton = canvasActiveBtn.transform.GetChild(j).gameObject;
                    canvasButton.SetActive(true);
					canvasSelectBtn.SetActive(false);
				}
            }
        }
    }
	void RotateChange()
	{

	}
	void Move()
	{

	}
	void SizeChange()
	{

	}
}
