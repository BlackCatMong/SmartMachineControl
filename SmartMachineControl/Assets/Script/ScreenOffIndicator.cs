using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOffIndicator : MonoBehaviour
{
	public GameObject mScreenOffIndicatorPrefab;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		CreateScreenOffIndicator();
	}
	
	public bool IsTargetVisiable(Camera _camera, Transform _transform)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);//평면체를 가져옴..
		Vector3 point = _transform.position;
		foreach(var plane in planes)
		{
			//0보다 작으면 평면 안쪽, 0이면 평면위, 0보다 크면 평면 밖 
			if(plane.GetDistanceToPoint(point) < 0) //평면체와 오브젝트의 거리 확인..
			{
				return false;
			}
			
		}
		return true;
	}
	float GetGradien(float _x1, float _x2, float _y1, float _y2)
	{
		return (_y1 - _y2) / (_x1 - _x2);
	}
	public void CreateScreenOffIndicator()
	{
		GameObject[] gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

		for (int i = 0; i < gameObjects.Length; i++)
		{
			if (gameObjects[i].tag == "WorkArea" && gameObjects[i].name.Contains("Clone"))//맨처음 시작때 WorkArea가 생성됨 ... 어디서 생성되는거지..
			{
				Transform gameObjectTransform = gameObjects[i].transform;
				if(!IsTargetVisiable(Camera.main, gameObjectTransform)) //화면 밖에 있으므로 Indicator On ..
				{
					float objectXPos = gameObjectTransform.position.x;
					float objectYPos = gameObjectTransform.position.y;
					//	float gradien = GetGradien(Camera.main.transform.position.y, gameObjects[i].transform.position.y, Camera.main.transform.position.x, gameObjects[i].transform.position.x);
					float gradien = Camera.main.transform.position.y - gameObjects[i].transform.position.y;
					float screenX = Screen.width;
					float screenY = Screen.height;
					string log;

					float indicatorXPos = screenY / gradien / 2;
					float indicatorYPos = screenX * gradien / 2;
					log = "gradien -> " + gradien + "\n";
					log += "Indicator Y Pos -> " + indicatorYPos + "\n";
					indicatorYPos += screenY / 2;
					if (indicatorYPos > screenY)
						indicatorYPos = screenY;
					else if(indicatorYPos - 100 < 0)
						indicatorYPos = 100;


					if (!mScreenOffIndicatorPrefab.activeInHierarchy)
						mScreenOffIndicatorPrefab.SetActive(true);
//					float screenX = Screen.width / 2;
//					float tmp = (Camera.main.transform.position.y + gameObjectTransform.position.y);
//					float yPosition = Screen.height / 2 - Screen.height * tmp;

					Vector2 indicatorPosition = new Vector2(Screen.width - 100, indicatorYPos); //100은 Indicator크기
					Quaternion indicatorRotate = new Quaternion(0, 0, 0, 0);
					//Instantiate(mScreenOffIndicatorPrefab, indicatorPosition, indicatorRotate); //무한생성 
					//mScreenOffIndicatorPrefab.transform.position = indicatorPosition;

					mScreenOffIndicatorPrefab.transform.position = indicatorPosition;
					log += "Camera Position -> " + Camera.main.transform.position + "\n";
					log += "object Position -> " + gameObjects[i].transform.position;

					Debug.Log(log);
				}
				else //화면 안에 있으므로 Indicator Off..
				{
					mScreenOffIndicatorPrefab.SetActive(false);
				}
			}
		}
	}

}
