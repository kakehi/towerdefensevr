using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public bool isVR = false;

	public bool autoPopulateTowers = false;

	public GameObject DesktopCamera;
	public GameObject VRCamera;
	public GameObject InstantPreviewPrefab;
	public GameObject VREditorPrefab;

	// Use this for initialization
	void Start () {

		if(isVR){
			DesktopCamera.SetActive (false);
			DesktopCamera.GetComponent<Camera>().enabled = false;
			VRCamera.SetActive (true);
			VRCamera.GetComponent<Camera>().enabled = true;
			InstantPreviewPrefab.SetActive (true);
			VREditorPrefab.SetActive (true);
		}else{
			DesktopCamera.SetActive (true);
			DesktopCamera.GetComponent<Camera>().enabled = true;
			VRCamera.SetActive (false);
			VRCamera.GetComponent<Camera>().enabled = false;
			InstantPreviewPrefab.SetActive (false);
			VREditorPrefab.SetActive (false);
		}

	}

}
