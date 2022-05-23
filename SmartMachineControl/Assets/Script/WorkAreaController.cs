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
		mFlagIndex = new bool[] { mSizeChangeFlag, mRotateChangeFlag, mMoveFlag}; //배열을 주솟값으로 해야할듯 .

		for (int i = 0; i < mFlagIndex.Length; i++) //false로 초기화 ..
			mFlagIndex[i] = false;
		
		Debug.Log("Bool Index Count -> " + mFlagIndex.Length);
		AllFlagOff();
		SelectBtnActiveOnOff(false); //OK False 버튼안보이게 ..

		mCanvasPopUp.SetActive(false); // Pop Off

		mOriginRotation = mWorkArea.transform.rotation;
		mOldTouchMoveDis = 0;
		mStopwatch.Reset();
		mClickCount = 0;
	}

    // Update is called once per frame
    void Update()
    {
		SaveFlagIndex(); //배열 주솟값으로 변경하면 삭제 ..
        ButtonCheck(); // 버튼 클릭하면 색변경 및 다른 버튼 안보이게 ..
		RotateChange(); // 회전값 변경 ..
		Move(); //이동 ..
		SizeChange(); //사이즈 변경
		StopWatchCheck();
		PopUpDataCheck();
	}
	void SaveFlagIndex() // 배열을 주소값 참조로 변경해야될듯...ㅠㅠㅠㅠㅠㅠㅠ
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
	void AllFlagOff() //여기도 주소값 참조로 바꿔야할듯 ㅜㅜ
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

            if (mFlagIndex[i])//true인 Index찾으면 ..
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
                //flagCheck = true;	//눌린 Button이 있다 ..
            }
            else
            {
                canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.normalColor;
            }
//
//		if ((i == mFlagIndex.Length - 1) && !flagCheck) //for문 마지막에 체크 ..? 그냥 밖으로 빼면 된거 아닌가 .. 
//		{
//			for (int j = 0; j < mCanvasActiveBtn.transform.childCount; j++)
//			{
//				canvasButton = mCanvasActiveBtn.transform.GetChild(j).gameObject;
//				canvasButton.SetActive(true);
//				SelectBtnActiveOnOff(false);
//				mOriginRotation = mWorkArea.transform.rotation; // 기존 값 저장 후 취소시 원복을 위해 .. 
//				mOriginScale = mWorkArea.transform.localScale;
//				mOriginPosition = mWorkArea.transform.position;
//			}
//		}
		}
		//if(!flagCheck && !mDeleteFlag)
		if(mDeleteFlag) // 삭제 확인 메세지 출력및 다른 버튼 비활성화 필요..
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
				mOriginRotation = mWorkArea.transform.rotation; // 기존 값 저장 후 취소시 원복을 위해 .. 
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
		if(!mDeleteFlag && !GetAllFlag()) // 다른 버튼을 눌럿을 경우에 삭제 안되도록 .. 
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
	void StopWatchCheck() //더블클릭용 시간 체크 .. 
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
				new string[] { mWorkArea.transform.position.ToString(), mWorkArea.transform.rotation.ToString(),	mWorkArea.transform.localScale.ToString() };//for문으로 한번에 작업용

			if(mCanvasPopUp.activeInHierarchy) //하이어라키에 활성화 되면 ..
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
