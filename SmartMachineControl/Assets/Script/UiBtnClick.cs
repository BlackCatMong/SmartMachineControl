using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class UiBtnClick : MonoBehaviour
{
    public GameObject indicator;
    public GameObject workAreaPrefabs;

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
    }
    //���� ��ġ�� Cube ����� ... (CreateCube ���� ��� ���� ��ġ ǥ�� �� Ŭ���� �� ��ġ�� ���� .. )
    public void CreateCubeClick()
    {
        indicatorOn = !indicatorOn;
    }
    //�ʿ��� ��� ������ġ ǥ���� .. 
    void CreateLocation()
    {
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        List<ARRaycastHit> aRRaycastHits = new List<ARRaycastHit>();

        if(arRaycastManager.Raycast(screenSize, aRRaycastHits, TrackableType.Planes) && indicatorOn)
        {
            indicator.SetActive(true);

            indicator.transform.position = aRRaycastHits[0].pose.position;
            indicator.transform.rotation = aRRaycastHits[0].pose.rotation;

            indicator.transform.position += indicator.transform.up * 0.1f; // ��ġ�Ƿ� �÷���

        }
        else
        {
            indicator.SetActive(false);
        }
    }
    void CreateWorkArea()
    {
        if(indicator.activeInHierarchy && indicatorOn && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                Instantiate(workAreaPrefabs, indicator.transform.position, indicator.transform.rotation);
                indicatorOn = false;
            }
        }
    }

}
