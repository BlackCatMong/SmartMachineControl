using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class UiBtnClick : MonoBehaviour
{
    public GameObject mIndicator;			// 오브젝트 생성 위치 표시 ..
    public GameObject mWorkAreaPrefabs;		// 작업 오브젝트 프리팹
	public GameObject mWarningAreaPrefabs;	// 위험지역 오브젝트 프리팹 
    public GameObject mCanvas;              // UI 

	public GameObject mDataUI;              //상시 출력 UI...
	string[] mDataUIChildName;

    ARRaycastManager mArRaycastManager;     //Ray 관리 .. 
	ARCameraManager mCameraManager;
	ARPlaneManager mPlaneManager;
	GameObject prefabs;


	bool mIndicatorOn;                      //오브젝트 생성을 위해 인디케이터 유무 확인 
	bool mWorkFlag;
	bool mWarningFlag;

    // Start is called before the first frame update
    void Start()
    {
		mIndicatorOn = false;
		mWorkFlag = false;
		mWarningFlag = false;
		mIndicator = Instantiate(mIndicator);
		mIndicator.SetActive(false);
		mArRaycastManager = GetComponent<ARRaycastManager>();
		mDataUIChildName = new string[] { "Date", "Speed", "Gear", "Battery", "WorkTime" };
		mCameraManager = gameObject.GetComponent<ARSessionOrigin>().transform.GetComponentInChildren<ARCameraManager>();
		mPlaneManager = gameObject.GetComponent<ARSessionOrigin>().transform.GetComponentInChildren<ARPlaneManager>();
	}

    // Update is called once per frame
    void Update()
    {
        CreateLocation();
        CreateWorkArea();
        ButtonCheck();
		TestFunc();
	}

	//지정 위치에 Cube 만들기 ... (CreateCube 누를 경우 생성 위치 표시 후 클릭시 그 위치에 생성 .. )
	public void CreateWorkAreaClick()
    {
		//mIndicatorOn = !mIndicatorOn;
		mWorkFlag = !mWorkFlag;
		mWarningFlag = false;
	}
	public void CreateWarningAreaClick()
	{

		mWarningFlag = !mWarningFlag;
		mWorkFlag = false;

	}
    //생성위치 표현용 .. 
    void CreateLocation()
    {
		if (mWorkFlag || mWarningFlag)
			mIndicatorOn = true;
		else
			mIndicatorOn = false;

		ARPlaneManager aRPlaneManager = GetComponent<ARPlaneManager>();
		if (mIndicatorOn)
		{
			foreach (var plane in gameObject.GetComponent<ARPlaneManager>().trackables)
			{
				plane.gameObject.SetActive(true);
			}
		}
		else
		{
			foreach (var plane in gameObject.GetComponent<ARPlaneManager>().trackables)
			{
				plane.gameObject.SetActive(false);
			}
			
		}
	
		Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f); //화면 가운데

        List<ARRaycastHit> aRRaycastHits = new List<ARRaycastHit>();

        if(mArRaycastManager.Raycast(screenSize, aRRaycastHits, TrackableType.Planes) && mIndicatorOn) //화면 가운데 Plane이 있으면
        {
			mIndicator.SetActive(true); //생성 위치 표기

			mIndicator.transform.position = aRRaycastHits[0].pose.position;
			mIndicator.transform.rotation = aRRaycastHits[0].pose.rotation;

			mIndicator.transform.position += mIndicator.transform.up * 0.1f; // 겹치므로 올려줌
        }
        else
        {
			mIndicator.SetActive(false);
        }
    }
    //작업 구역 생성..
    void CreateWorkArea()
    {
        if(mIndicator.activeInHierarchy && mIndicatorOn && Input.touchCount > 0 )
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began && !EventSystem.current.currentSelectedGameObject)
            {
				if(mWorkFlag)
				{
					Instantiate(mWorkAreaPrefabs, mIndicator.transform.position, mIndicator.transform.rotation);
					mWorkFlag = false;
					Debug.Log("Work Area Object Create !!!!!!!!!!!!");
				}
				else if (mWarningFlag)
				{
					Instantiate(mWarningAreaPrefabs, mIndicator.transform.position, mIndicator.transform.rotation);
					mWarningFlag = false;
				}
            }
        }
    }
	
    //Button 눌렀을때 버튼 색 변경
    void ButtonCheck()
    {
        if (mWorkFlag)
        {
            GameObject canvasButton = mCanvas.transform.GetChild(0).gameObject;
            canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.pressedColor;
        }
        else
        {
            GameObject canvasButton = mCanvas.transform.GetChild(0).gameObject;
            canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.normalColor;
        }
		if(mWarningFlag)
		{
			GameObject canvasButton = mCanvas.transform.GetChild(1).gameObject;
			canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.pressedColor;
		}
		else
		{
			GameObject canvasButton = mCanvas.transform.GetChild(1).gameObject;
			canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.normalColor;
		}

    }

	System.Random random = new System.Random(); //Test용 데이터 변경 .. 
	void TestFunc()
	{
		DateCheck();
		SpeedCheck();
		GearCheck();
		BattryCheck();
		WorkTimeCheck();
	}
	void DateCheck()
	{
		DateTime date = DateTime.Now;
		GameObject dataUI = mDataUI.transform.Find(mDataUIChildName[0]).gameObject;
		dataUI.GetComponent<Text>().text = date.ToString("F");
	}
	void SpeedCheck()
	{
		GameObject dataUI = mDataUI.transform.Find(mDataUIChildName[1]).gameObject;

		dataUI.GetComponent<Text>().text = random.Next().ToString();
	}
	void GearCheck()
	{
		GameObject dataUI = mDataUI.transform.Find(mDataUIChildName[2]).gameObject;
		dataUI.GetComponent<Text>().text = random.Next().ToString();
	}
	void BattryCheck()
	{
		GameObject dataUI = mDataUI.transform.Find(mDataUIChildName[3]).gameObject;
		dataUI.GetComponent<Text>().text = random.Next().ToString();
	}
	void WorkTimeCheck()
	{
		GameObject dataUI = mDataUI.transform.Find(mDataUIChildName[4]).gameObject;
		dataUI.GetComponent<Text>().text = random.Next().ToString();
	}

	public void DriveBtnClick()
	{
		mCameraManager.requestedFacingDirection = CameraFacingDirection.World;
		Debug.Log("Drive Btn Click -> " + mCameraManager.currentFacingDirection + "//////" + mCameraManager.subsystem + "//////" + mCameraManager.enabled);
	}
	public void ReverseBtnClick()
	{
		mCameraManager.requestedFacingDirection = CameraFacingDirection.User;
		Debug.Log("R Btn Click -> " + mCameraManager.currentFacingDirection + "//////" + mCameraManager.subsystem + "//////" + mCameraManager.enabled);
	}
}
