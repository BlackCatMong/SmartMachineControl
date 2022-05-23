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
    public GameObject mCanvas;              // UI 

	public GameObject mDataUI;              //��� ��� UI...
	string[] mDataUIChildName;

    ARRaycastManager mArRaycastManager;		//Ray ���� .. 
    
    bool mIndicatorOn;						//������Ʈ ������ ���� �ε������� ���� Ȯ�� 

    // Start is called before the first frame update
    void Start()
    {
		FirstInit();
    }

    // Update is called once per frame
    void Update()
    {
        CreateLocation();
        CreateWorkArea();
        ButtonCheck();
		TestFunc();
	}

	void FirstInit()
	{
		mIndicatorOn = false;
		mIndicator = Instantiate(mIndicator);
		mIndicator.SetActive(false);
		mArRaycastManager = GetComponent<ARRaycastManager>();
		mDataUIChildName = new string[] { "Date", "Speed", "Gear", "Battery", "WorkTime"};
	}
    //���� ��ġ�� Cube ����� ... (CreateCube ���� ��� ���� ��ġ ǥ�� �� Ŭ���� �� ��ġ�� ���� .. )
    public void CreateCubeClick()
    {
		mIndicatorOn = !mIndicatorOn;
    }
    //������ġ ǥ���� .. 
    void CreateLocation()
    {
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
        if(mIndicator.activeInHierarchy && mIndicatorOn && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began && !EventSystem.current.currentSelectedGameObject)
            {
                Instantiate(mWorkAreaPrefabs, mIndicator.transform.position, mIndicator.transform.rotation);
				mIndicatorOn = false;
            }
        }
    }
    //Button �������� Ȯ��
    void ButtonCheck()
    {
        if (mIndicatorOn)
        {
            GameObject canvasButton = mCanvas.transform.GetChild(0).gameObject;
            canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.pressedColor;
        }
        else
        {
            GameObject canvasButton = mCanvas.transform.GetChild(0).gameObject;
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
}
