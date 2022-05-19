using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class WorkAreaController : MonoBehaviour
{
	public GameObject mWorkArea;
    public GameObject mCanvasActiveBtn;
	public GameObject mCanvasSelectBtn;

	bool[] mFlagIndex;

	bool mSizeChangeFlag;
	bool mRotateChangeFlag;
	bool mMoveFlag;
	float mOldTouchMoveDis;

	Vector3 mDeltaPos = new Vector3();

	Quaternion mOriginRotation = new Quaternion();
	Vector3 mOriginScale = new Vector3();
	Vector3 mOriginPosition = new Vector3();



	// Start is called before the first frame update
	void Start()
    {
		mFlagIndex = new bool[] { mSizeChangeFlag, mRotateChangeFlag, mMoveFlag}; //�迭�� �ּڰ����� �ؾ��ҵ� .

		for (int i = 0; i < mFlagIndex.Length; i++) //false�� �ʱ�ȭ ..
			mFlagIndex[i] = false;
		
		Debug.Log("Bool Index Count -> " + mFlagIndex.Length);
		AllFlagOff();
		SelectBtnActiveOnOff(false); //OK False ��ư�Ⱥ��̰� ..

		mOriginRotation = mWorkArea.transform.rotation;
		mOldTouchMoveDis = 0;
	}

    // Update is called once per frame
    void Update()
    {
		SaveFlagIndex(); //�迭 �ּڰ����� �����ϸ� ���� ..
        ButtonCheck(); // ��ư Ŭ���ϸ� ������ �� �ٸ� ��ư �Ⱥ��̰� ..
		RotateChange(); // ȸ���� ���� ..
		Move(); //�̵� ..
		SizeChange(); //������ ����
    }
	void SaveFlagIndex() // �迭�� �ּҰ� ������ �����ؾߵɵ�...�ФФФФФФ�
	{
		mFlagIndex[0] = mSizeChangeFlag;
		mFlagIndex[1] = mMoveFlag;
		mFlagIndex[2] = mRotateChangeFlag;
	}

	public void SizeChangeBtnClick()
    {
        Debug.Log("SizeChange");
		mSizeChangeFlag = !mSizeChangeFlag;
    }
    public void MoveBtnClick()
    {
        Debug.Log("Move");
		mMoveFlag = !mMoveFlag;
    }
    public void RotateChangeBtnClick()
    {
        Debug.Log("RotateChange");
		mRotateChangeFlag = !mRotateChangeFlag;
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
		if (mRotateChangeFlag)
		{
			Debug.Log("Cancel Btn Click Origin Transform Rotate .. -> " + mOriginRotation);
			mWorkArea.transform.rotation = mOriginRotation;
		}
		else if(mRotateChangeFlag)
		{
			mWorkArea.transform.localScale = mOriginScale;
		}
		else if(mMoveFlag)
		{
			mWorkArea.transform.position = mOriginPosition;
		}
		SelectBtnActiveOnOff(false);
		AllFlagOff();
	}
	public void SelectBtnActiveOnOff(bool onOff)
	{
		for (int i = 0; i < mCanvasSelectBtn.transform.childCount; i++)
			mCanvasSelectBtn.transform.GetChild(i).transform.gameObject.SetActive(onOff);
	}
	void AllFlagOff() //���⵵ �ּҰ� ������ �ٲ���ҵ� �̤�
	{
		mSizeChangeFlag = false;
		mMoveFlag = false;
		mRotateChangeFlag = false;
	}
	void ButtonCheck()	
    {
        bool flagCheck = false;
		for (int i = 0; i < mFlagIndex.Length; i++)
        {
            GameObject canvasButton = mCanvasActiveBtn.transform.GetChild(i).gameObject;

            if (mFlagIndex[i])
            {
                canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.pressedColor;
                for (int j = 0; j < mCanvasActiveBtn.transform.childCount; j++)
                {
                    if (i != j)
                    {
                        canvasButton = mCanvasActiveBtn.transform.GetChild(j).gameObject;
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
            
            if ((i == mFlagIndex.Length - 1) && !flagCheck)
            {
                for (int j = 0; j < mCanvasActiveBtn.transform.childCount; j++)
                {
					canvasButton = mCanvasActiveBtn.transform.GetChild(j).gameObject;
                    canvasButton.SetActive(true);
					SelectBtnActiveOnOff(false);
					mOriginRotation = mWorkArea.transform.rotation; // ���� �� ���� �� ��ҽ� ������ ���� .. 
					mOriginScale = mWorkArea.transform.localScale;
					mOriginPosition = mWorkArea.transform.position;

				}
            }
        }
    }
	void RotateChange()
	{
		if (mRotateChangeFlag)
		{
			if(Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				if(touch.phase == TouchPhase.Moved)
				{
					mDeltaPos = touch.deltaPosition;

					mWorkArea.transform.Rotate(transform.up, mDeltaPos.x * -1.0f * 0.1f);
				}
			}
		}
	}
	void Move()
	{
		Vector2 touchMoveDis = new Vector2(0f, 0f);
		if(mMoveFlag)
		{
			if(Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				if(touch.phase == TouchPhase.Moved)
				{
					touchMoveDis = touch.deltaPosition;
					Vector3 tmp = touchMoveDis;
					mWorkArea.transform.position = mWorkArea.transform.position + tmp * 0.001f;
				}
			}
		}
	}
	void SizeChange()
	{
		float touchMovedDis = 0f;
		float disDiff = 0f;
		if(mRotateChangeFlag)
		{
			if(Input.touchCount > 1)
			{
				Touch touch0 = Input.GetTouch(0);
				Touch touch1 = Input.GetTouch(1);
				if(touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
				{
					touchMovedDis = (touch0.position - touch1.position).sqrMagnitude;
					disDiff = touchMovedDis - mOldTouchMoveDis;

					if (disDiff > 0)
						mWorkArea.transform.localScale *= 1.1f;
					else if (disDiff < 0)
						mWorkArea.transform.localScale *= 0.9f;

					mOldTouchMoveDis = touchMovedDis;
				}
			}
		}
	}
}
