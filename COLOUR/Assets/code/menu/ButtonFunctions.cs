using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour {

	public void OpenNewScene(string scene){
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
	}
}
