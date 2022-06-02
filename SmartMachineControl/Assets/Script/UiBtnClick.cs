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
    public GameObject mIndicator;			// ������Ʈ ���� ��ġ ǥ�� ..
    public GameObject mWorkAreaPrefabs;		// �۾� ������Ʈ ������
	public GameObject mWarningAreaPrefabs;	// �������� ������Ʈ ������ 
    public GameObject mCanvas;              // UI 

	public GameObject mDataUI;              //��� ��� UI...
	string[] mDataUIChildName;

    ARRaycastManager mArRaycastManager;     //Ray ���� .. 
	ARCameraManager mCameraManager;
	ARPlaneManager mPlaneManager;
	GameObject prefabs;


	bool mIndicatorOn;                      //������Ʈ ������ ���� �ε������� ���� Ȯ�� 
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

	//���� ��ġ�� Cube ����� ... (CreateCube ���� ��� ���� ��ġ ǥ�� �� Ŭ���� �� ��ġ�� ���� .. )
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
    //������ġ ǥ���� .. 
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
	
		Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f); //ȭ�� ���

        List<ARRaycastHit> aRRaycastHits = new List<ARRaycastHit>();

        if(mArRaycastManager.Raycast(screenSize, aRRaycastHits, TrackableType.Planes) && mIndicatorOn) //ȭ�� ��� Plane�� ������
        {
			mIndicator.SetActive(true); //���� ��ġ ǥ��

			mIndicator.transform.position = aRRaycastHits[0].pose.position;
			mIndicator.transform.rotation = aRRaycastHits[0].pose.rotation;

			mIndicator.transform.position += mIndicator.transform.up * 0.1f; // ��ġ�Ƿ� �÷���
        }
        else
        {
			mIndicator.SetActive(false);
        }
    }
    //�۾� ���� ����..
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
	
    //Button �������� ��ư �� ����
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

	System.Random random = new System.Random(); //Test�� ������ ���� .. 
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
