using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCheck : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
	
	}

    // Update is called once per frame
    void Update()
    {

	}
	
	float GetObjectDis(Vector3 _origin, Vector3 _target)
	{
		return Mathf.Floor(Vector3.Distance(_origin, _target)); 
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.name.Length > 0 && !other.name.Equals("DisCheckBoxCol"))
		{
			Debug.Log("OnTriggerEnter -> " + other.name);
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.name.Length > 0 && !other.name.Equals("DisCheckBoxCol"))
		{
			Debug.Log("OnTriggerEnter -> " + other.name);
		}
	}
}
