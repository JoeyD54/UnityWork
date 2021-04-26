using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButton : MonoBehaviour
{
	public void StartGame()
	{
		SceneManager.LoadScene("level1");
	}
}
