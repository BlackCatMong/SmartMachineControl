using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class UiBtnClick : MonoBehaviour
{
    public GameObject indicator;

    ARRaycastManager arRaycastManager;
    bool indicatorOn = false;
    // Start is called before the first frame update
    void Start()
    {
        indicator.SetActive(false);

        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CreateLocation();
    }
    //지정 위치에 Cube 만들기 ... (CreateCube 누를 경우 생성 위치 표시 후 클릭시 그 위치에 생성 .. )
    void CreateCubeClick()
    {
        indicatorOn = !indicatorOn;
    }
    //필요한 경우 생성위치 표현용 .. 
    void CreateLocation()
    {
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        List<ARRaycastHit> aRRaycastHits = new List<ARRaycastHit>();

        if(arRaycastManager.Raycast(screenSize, aRRaycastHits, TrackableType.Planes) && AndroidActivityIndicatorStyle)
        {
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }
    }

}
