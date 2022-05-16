using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiBtnClick : MonoBehaviour
{
    public GameObject indicator;
    public GameObject workAreaPrefabs;
    public GameObject canvas;

    ARRaycastManager arRaycastManager;
    
    bool indicatorOn;
    // Start is called before the first frame update
    void Start()
    {
        indicatorOn = false;
        indicator = Instantiate(indicator);
        indicator.SetActive(false);
        
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CreateLocation();
        CreateWorkArea();
        ButtonCheck();
    }
    //지정 위치에 Cube 만들기 ... (CreateCube 누를 경우 생성 위치 표시 후 클릭시 그 위치에 생성 .. )
    public void CreateCubeClick()
    {
        indicatorOn = !indicatorOn;
    }
    //필요한 경우 생성위치 표현용 .. 
    void CreateLocation()
    {
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        List<ARRaycastHit> aRRaycastHits = new List<ARRaycastHit>();

        if(arRaycastManager.Raycast(screenSize, aRRaycastHits, TrackableType.Planes) && indicatorOn)
        {
            indicator.SetActive(true);

          indicator.transform.position = aRRaycastHits[0].pose.position;
            indicator.transform.rotation = aRRaycastHits[0].pose.rotation;

            indicator.transform.position += indicator.transform.up * 0.1f; // 겹치므로 올려줌

        }
        else
        {
            indicator.SetActive(false);
        }
    }
    //작업 구역 생성..
    void CreateWorkArea()
    {
        if(indicator.activeInHierarchy && indicatorOn && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began && !EventSystem.current.currentSelectedGameObject)
            {
                Instantiate(workAreaPrefabs, indicator.transform.position, indicator.transform.rotation);
                indicatorOn = false;
            }
        }
    }
    //Button 눌렀을때 확인
    void ButtonCheck()
    {
        if (indicatorOn)
        {
            GameObject canvasButton = canvas.transform.GetChild(0).gameObject;
            canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.pressedColor;
        }
        else
        {
            GameObject canvasButton = canvas.transform.GetChild(0).gameObject;
            canvasButton.GetComponent<Image>().color = canvasButton.GetComponent<Button>().colors.normalColor;
        }
    }
}
