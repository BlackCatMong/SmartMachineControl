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
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);//���ü�� ������..
		Vector3 point = _transform.position;
		foreach(var plane in planes)
		{
			//0���� ������ ��� ����, 0�̸� �����, 0���� ũ�� ��� �� 
			if(plane.GetDistanceToPoint(point) < 0) //���ü�� ������Ʈ�� �Ÿ� Ȯ��..
			{
				return false;
			}
			
		}
		return true;
	}
	public void CreateScreenOffIndicator()
	{
		GameObject[] gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

		for (int i = 0; i < gameObjects.Length; i++)
		{
			if (gameObjects[i].tag == "WorkArea" && gameObjects[i].name.Contains("Clone"))//��ó�� ���۶� WorkArea�� ������ ... ��� �����Ǵ°���..
			{
				Transform gameObjectTransform = gameObjects[i].transform;
				if(!IsTargetVisiable(Camera.main, gameObjectTransform)) //ȭ�� �ۿ� �����Ƿ� Indicator On ..
				{
					if(!mScreenOffIndicatorPrefab.activeInHierarchy)
						mScreenOffIndicatorPrefab.SetActive(true);

					float screenX = Screen.width / 2;
					float tmp = (Camera.main.transform.position.y + gameObjectTransform.position.y);
					float yPosition = Screen.height / 2 - Screen.height * tmp;

					Vector2 indicatorPosition = new Vector2(Screen.width - 100, yPosition); //100�� Indicatorũ��
					Quaternion indicatorRotate = new Quaternion(0, 0, 0, 0);
					//Instantiate(mScreenOffIndicatorPrefab, indicatorPosition, indicatorRotate); //���ѻ��� 
					//mScreenOffIndicatorPrefab.transform.position = indicatorPosition;

					string log = "Camera Position -> " + Camera.main.transform.position + "\n";
					log += "object Position -> " + gameObjects[i].transform.position;

					Debug.Log(log);
				}
				else //ȭ�� �ȿ� �����Ƿ� Indicator Off..
				{
					mScreenOffIndicatorPrefab.SetActive(false);
				}
			}
		}
	}

}
