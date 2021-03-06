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
		mFlagIndex = new bool[] { mSizeChangeFlag, mRotateChangeFlag, mMoveFlag}; //壕伸聖 爽借葵生稽 背醤拝牛 .

		for (int i = 0; i < mFlagIndex.Length; i++) //false稽 段奄鉢 ..
			mFlagIndex[i] = false;
		
		Debug.Log("Bool Index Count -> " + mFlagIndex.Length);
		AllFlagOff();
		SelectBtnActiveOnOff(false); //OK False 獄動照左戚惟 ..

		mCanvasPopUp.SetActive(false); // Pop Off

		mOriginRotation = mWorkArea.transform.rotation;
		mOldTouchMoveDis = 0;
		mStopwatch.Reset();
		mClickCount = 0;
	}

    // Update is called once per frame
    void Update()
    {
		SaveFlagIndex(); //壕伸 爽借葵生稽 痕井馬檎 肢薦 ..
        ButtonCheck(); // 獄動 適遣馬檎 事痕井 貢 陥献 獄動 照左戚惟 ..
		RotateChange(); // 噺穿葵 痕井 ..
		Move(); //戚疑 ..
		SizeChange(); //紫戚綜 痕井
		StopWatchCheck();
		PopUpDataCheck();
		RotateSign();
	}
	void SaveFlagIndex() // 壕伸聖 爽社葵 凧繕稽 痕井背醤吃牛...ばばばばばばば
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
	void AllFlagOff() //食奄亀 爽社葵 凧繕稽 郊蚊醤拝牛 ぬぬ
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

            if (mFlagIndex[i])//true昔 Index達生檎 ..
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
                //flagCheck = true;	//喚鍵 Button戚 赤陥 ..
            }
            else
            {
                canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.normalColor;
            }
//
//		if ((i == mFlagIndex.Length - 1) && !flagCheck) //for庚 原走厳拭 端滴 ..? 益撹 鉱生稽 皐檎 吉暗 焼観亜 .. 
//		{
//			for (int j = 0; j < mCanvasActiveBtn.transform.childCount; j++)
//			{
//				canvasButton = mCanvasActiveBtn.transform.GetChild(j).gameObject;
//				canvasButton.SetActive(true);
//				SelectBtnActiveOnOff(false);
//				mOriginRotation = mWorkArea.transform.rotation; // 奄糎 葵 煽舌 板 昼社獣 据差聖 是背 .. 
//				mOriginScale = mWorkArea.transform.localScale;
//				mOriginPosition = mWorkArea.transform.position;
//			}
//		}
		}
		//if(!flagCheck && !mDeleteFlag)
		if(mDeleteFlag) // 肢薦 溌昔 五室走 窒径貢 陥献 獄動 搾醗失鉢 琶推..
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
				mOriginRotation = mWorkArea.transform.rotation; // 奄糎 葵 煽舌 板 昼社獣 据差聖 是背 .. 
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
		if(!mDeleteFlag && !GetAllFlag()) // 陥献 獄動聖 喚卦聖 井酔拭 肢薦 照鞠亀系 .. 
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
	void StopWatchCheck() //希鷺適遣遂 獣娃 端滴 .. 
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
				new string[] { mWorkArea.transform.position.ToString(), mWorkArea.transform.rotation.ToString(),	mWorkArea.transform.localScale.ToString() };//for庚生稽 廃腰拭 拙穣遂

			if(mCanvasPopUp.activeInHierarchy) //馬戚嬢虞徹拭 醗失鉢 鞠檎 ..
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
	void RotateSign()
	{
		if (mWorkArea.transform.Find("SignType") != null)
			mWorkArea.transform.Find("SignType").transform.Rotate(new Vector3(0, 100f * Time.deltaTime, 0));
	}
}
