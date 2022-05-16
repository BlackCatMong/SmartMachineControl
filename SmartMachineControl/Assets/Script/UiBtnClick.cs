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
    //�۾� ���� ����..
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
    //Button �������� Ȯ��
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
