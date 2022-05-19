using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiBtnClick : MonoBehaviour
{
    public GameObject mIndicator;			// 오브젝트 생성 위치 표시 ..
    public GameObject mWorkAreaPrefabs;		// 작업 오브젝트 프리팹
    public GameObject mCanvas;				// UI 

    ARRaycastManager mArRaycastManager;		//Ray 관리 .. 
    
    bool mIndicatorOn;						//오브젝트 생성을 위해 인디케이터 유무 확인 

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
    }
	void FirstInit()
	{
		mIndicatorOn = false;
		mIndicator = Instantiate(mIndicator);
		mIndicator.SetActive(false);
		mArRaycastManager = GetComponent<ARRaycastManager>();
	}
    //지정 위치에 Cube 만들기 ... (CreateCube 누를 경우 생성 위치 표시 후 클릭시 그 위치에 생성 .. )
    public void CreateCubeClick()
    {
		mIndicatorOn = !mIndicatorOn;
    }
    //생성위치 표현용 .. 
    void CreateLocation()
    {
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
    //Button 눌렀을때 확인
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
}
