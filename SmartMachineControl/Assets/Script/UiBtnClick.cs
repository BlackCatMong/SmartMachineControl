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
    //���� ��ġ�� Cube ����� ... (CreateCube ���� ��� ���� ��ġ ǥ�� �� Ŭ���� �� ��ġ�� ���� .. )
    void CreateCubeClick()
    {
        indicatorOn = !indicatorOn;
    }
    //�ʿ��� ��� ������ġ ǥ���� .. 
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
