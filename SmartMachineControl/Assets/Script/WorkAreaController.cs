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
	public GameObject mCanvasPopUp;

	bool[] mFlagIndex;

	bool mSizeChangeFlag;
	bool mRotateChangeFlag;
	bool mMoveFlag;
	bool mDeleteFlag;

	float mOldTouchMoveDis;

	int mClickCount;

	Vector3 mDeltaPos = new Vector3();

	Quaternion mOriginRotation = new Quaternion();
	Vector3 mOriginScale = new Vector3();
	Vector3 mOriginPosition = new Vector3();

	System.Diagnostics.Stopwatch mStopwatch = new System.Diagnostics.Stopwatch();

	// Start is called before the first frame update
	void Start()
    {
		mFlagIndex = new bool[] { mSizeChangeFlag, mRotateChangeFlag, mMoveFlag}; //�迭�� �ּڰ����� �ؾ��ҵ� .

		for (int i = 0; i < mFlagIndex.Length; i++) //false�� �ʱ�ȭ ..
			mFlagIndex[i] = false;
		
		Debug.Log("Bool Index Count -> " + mFlagIndex.Length);
		AllFlagOff();
		SelectBtnActiveOnOff(false); //OK False ��ư�Ⱥ��̰� ..

		mCanvasPopUp.SetActive(false); // Pop Off

		mOriginRotation = mWorkArea.transform.rotation;
		mOldTouchMoveDis = 0;
		mStopwatch.Reset();
		mClickCount = 0;
	}

    // Update is called once per frame
    void Update()
    {
		SaveFlagIndex(); //�迭 �ּڰ����� �����ϸ� ���� ..
        ButtonCheck(); // ��ư Ŭ���ϸ� ������ �� �ٸ� ��ư �Ⱥ��̰� ..
		RotateChange(); // ȸ���� ���� ..
		Move(); //�̵� ..
		SizeChange(); //������ ����
		StopWatchCheck();
		PopUpDataCheck();
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
		if(mDeleteFlag)
		{
			mDeleteFlag = false;
			Destroy(mWorkArea);
		}
		SelectBtnActiveOnOff(false);
		AllFlagOff();
	}
	bool GetAllFlag()
	{
		bool returnFlag = false;
		for(int i = 0; i < mFlagIndex.Length; i++)
		{
			returnFlag = returnFlag || mFlagIndex[i];
		}
		return returnFlag;

	}
	public void CancelBtnClick() 
	{
		Debug.Log("Cancel Button Click");
		if (mRotateChangeFlag)
		{
			Debug.Log("Cancel Btn Click Origin Transform Rotate .. -> " + mOriginRotation);
			mWorkArea.transform.rotation = mOriginRotation;
		}
		else if(mSizeChangeFlag)
		{
			mWorkArea.transform.localScale = mOriginScale;
		}
		else if(mMoveFlag)
		{
			mWorkArea.transform.position = mOriginPosition;
		}
		else if (mDeleteFlag)
		{
			//mWorkArea.GetComponent<MeshRenderer>().enabled = true;
			mDeleteFlag = false;
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
		//bool flagCheck = false;
		GameObject canvasButton;
		for (int i = 0; i < mFlagIndex.Length; i++)
        {
            canvasButton = mCanvasActiveBtn.transform.GetChild(i).gameObject;

            if (mFlagIndex[i])//true�� Indexã���� ..
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
                //flagCheck = true;	//���� Button�� �ִ� ..
            }
            else
            {
                canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.normalColor;
            }
//
//		if ((i == mFlagIndex.Length - 1) && !flagCheck) //for�� �������� üũ ..? �׳� ������ ���� �Ȱ� �ƴѰ� .. 
//		{
//			for (int j = 0; j < mCanvasActiveBtn.transform.childCount; j++)
//			{
//				canvasButton = mCanvasActiveBtn.transform.GetChild(j).gameObject;
//				canvasButton.SetActive(true);
//				SelectBtnActiveOnOff(false);
//				mOriginRotation = mWorkArea.transform.rotation; // ���� �� ���� �� ��ҽ� ������ ���� .. 
//				mOriginScale = mWorkArea.transform.localScale;
//				mOriginPosition = mWorkArea.transform.position;
//			}
//		}
		}
		//if(!flagCheck && !mDeleteFlag)
		if(mDeleteFlag) // ���� Ȯ�� �޼��� ��¹� �ٸ� ��ư ��Ȱ��ȭ �ʿ�..
		{
			SelectBtnActiveOnOff(true);
			mCanvasActiveBtn.SetActive(false);
		}
		else if (!GetAllFlag())
		{
			mCanvasActiveBtn.SetActive(true);
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
		if(mSizeChangeFlag)
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
	private void OnMouseDown()
	{
		if(!mDeleteFlag && !GetAllFlag()) // �ٸ� ��ư�� ������ ��쿡 ���� �ȵǵ��� .. 
		{
			switch (mClickCount)
			{
				case 0:
					mStopwatch.Start();
					mClickCount++;
					break;
				case 1:
					//mWorkArea.GetComponent<MeshRenderer>().enabled = false;
					//mDeleteFlag = true;
					mCanvasPopUp.SetActive(true); 
					break;
			}
		}
		
	}
	void StopWatchCheck() //����Ŭ���� �ð� üũ .. 
	{
		if (mStopwatch.ElapsedMilliseconds > 500)
		{
			mStopwatch.Stop();
			mStopwatch.Reset();
			mClickCount = 0;
		}
	}
	void PopUpDataCheck()
	{
		if (!GetAllFlag() && !mDeleteFlag)
		{
			string[] objectTransform =
				new string[] { mWorkArea.transform.position.ToString(), mWorkArea.transform.rotation.ToString(),	mWorkArea.transform.localScale.ToString() };//for������ �ѹ��� �۾���

			if(mCanvasPopUp.activeInHierarchy) //���̾��Ű�� Ȱ��ȭ �Ǹ� ..
			{
				GameObject dataValue = mCanvasPopUp.transform.Find("Data").transform.Find("DataValue").gameObject;
				int objectCount = dataValue.transform.childCount;
				for (int i = 0; i < objectCount; i++)
				{
					GameObject textDataValue = dataValue.transform.GetChild(i).gameObject;
					textDataValue.GetComponent<Text>().text = objectTransform[i];
				}
			}
		}
	}
	public void PopUpClose()
	{
		mCanvasPopUp.SetActive(false);
	}
	public void DeleteObject()
	{
		mCanvasPopUp.SetActive(false);
		mDeleteFlag = true;
	}
}
