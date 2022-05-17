using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class WorkAreaController : MonoBehaviour
{
	public GameObject workArea;
    public GameObject canvasActiveBtn;
	public GameObject canvasSelectBtn;

	bool[] flagIndex;

	bool sizeChangeFlag;
	bool rotateChangeFlag;
	bool moveFlag;
	float oldTouchMoveDis;

	Vector3 deltaPos = new Vector3();

	Quaternion originRotation = new Quaternion();
	Vector3 originScale = new Vector3();
	Vector3 originPosition = new Vector3();

	// Start is called before the first frame update
	void Start()
    {
		flagIndex = new bool[] {sizeChangeFlag, rotateChangeFlag, moveFlag}; //배열을 주솟값으로 해야할듯 .

		for (int i = 0; i < flagIndex.Length; i++) //false로 초기화 ..
			flagIndex[i] = false;
		
		Debug.Log("Bool Index Count -> " + flagIndex.Length);
		AllFlagOff();
		SelectBtnActiveOnOff(false); //OK False 버튼안보이게 ..

		originRotation = workArea.transform.rotation;
		oldTouchMoveDis = 0;
	}

    // Update is called once per frame
    void Update()
    {
		SaveFlagIndex(); //배열 주솟값으로 변경하면 삭제 ..
        ButtonCheck(); // 버튼 클릭하면 색변경 및 다른 버튼 안보이게 ..
		RotateChange(); // 회전값 변경 ..
		Move(); //이동 ..
		SizeChange(); //사이즈 변경
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
		SelectBtnActiveOnOff(false);
		AllFlagOff();
	}
	public void CancelBtnClick() 
	{
		Debug.Log("Cancel Button Click");
		if (rotateChangeFlag)
		{
			Debug.Log("Cancel Btn Click Origin Transform Rotate .. -> " + originRotation);
			workArea.transform.rotation = originRotation;
		}
		else if(sizeChangeFlag)
		{
			workArea.transform.localScale = originScale;
		}
		else if(moveFlag)
		{
			workArea.transform.position = originPosition;
		}
		SelectBtnActiveOnOff(false);
		AllFlagOff();

		

	}
	public void SelectBtnActiveOnOff(bool onOff)
	{
		for (int i = 0; i < canvasSelectBtn.transform.childCount; i++)
			canvasSelectBtn.transform.GetChild(i).transform.gameObject.SetActive(onOff);
	}
	void AllFlagOff() //여기도 주소값 참조로 바꿔야할듯 ㅜㅜ
	{
		sizeChangeFlag = false;
		moveFlag = false;
		rotateChangeFlag = false;
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
						SelectBtnActiveOnOff(true);
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
					SelectBtnActiveOnOff(false);
					originRotation = workArea.transform.rotation; // 기존 값 저장 후 취소시 원복을 위해 .. 
					originScale = workArea.transform.localScale;
					originPosition = workArea.transform.position;

				}
            }
        }
    }
	void RotateChange()
	{
		if (rotateChangeFlag)
		{
			if(Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				if(touch.phase == TouchPhase.Moved)
				{
					deltaPos = touch.deltaPosition;

					workArea.transform.Rotate(transform.up, deltaPos.x * -1.0f * 0.1f);
				}
			}
		}
	}
	void Move()
	{
		Vector2 touchMoveDis = new Vector2(0f, 0f);
		if(moveFlag)
		{
			if(Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				if(touch.phase == TouchPhase.Moved)
				{
					touchMoveDis = touch.deltaPosition;
					Vector3 tmp = touchMoveDis;
					workArea.transform.position = workArea.transform.position + tmp * 0.001f;
				}
			}
		}
	}
	void SizeChange()
	{
		float touchMovedDis = 0f;
		float disDiff = 0f;
		if(sizeChangeFlag)
		{
			if(Input.touchCount > 1)
			{
				Touch touch0 = Input.GetTouch(0);
				Touch touch1 = Input.GetTouch(1);
				if(touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
				{
					touchMovedDis = (touch0.position - touch1.position).sqrMagnitude;
					disDiff = touchMovedDis - oldTouchMoveDis;

					if (disDiff > 0)
						workArea.transform.localScale *= 1.1f;
					else if (disDiff < 0)
						workArea.transform.localScale *= 0.9f;

					oldTouchMoveDis = touchMovedDis;
				}
			}
		}
	}
}
