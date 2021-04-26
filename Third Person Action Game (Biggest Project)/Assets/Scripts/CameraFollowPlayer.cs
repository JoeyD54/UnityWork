using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollowPlayer : MonoBehaviour {

	GameObject target;
	public bool indoorMode = false;
	public bool firstPersonMode = false;
	GameObject closeRangeCamera, firstPersonCamera;

	// Use this for initialization
	void Start () {
		target = transform.parent.gameObject;
		closeRangeCamera = GameObject.Find("cameraIndoor");
		closeRangeCamera.SetActive(false);
		firstPersonCamera = GameObject.Find("cameraFirstPerson");
		firstPersonCamera.SetActive(false);	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(target.transform.position);

		if (Input.GetKeyDown(KeyCode.C))
		{
			indoorMode = !indoorMode;
			if (firstPersonMode)
				firstPersonMode = !firstPersonMode;
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			firstPersonMode = !firstPersonMode;
			if (indoorMode)
				indoorMode = !indoorMode;
		}

			if (indoorMode)
		{
			//print("zooming in camera");
			closeRangeCamera.SetActive(true);
			firstPersonCamera.SetActive(false);

		}
		else
		{
			if (closeRangeCamera.activeSelf)
			{
				//print("zooming out camera");
				closeRangeCamera.SetActive(false);
			}
		}
		if (firstPersonMode)
		{
			//print("zooming in camera");
			firstPersonCamera.SetActive(true);
			closeRangeCamera.SetActive(false);

		}
		else
		{
			if (firstPersonCamera.activeSelf)
			{
				//print("zooming out camera");
				firstPersonCamera.SetActive(false);
			}
		}
	}
}
