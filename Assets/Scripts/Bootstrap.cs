using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour {

	[SerializeField] private Score _score;

	// Use this for initialization
	IEnumerator Start () {
		yield return SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive).Await();
		GameController.Instance.CurrentScore = _score;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}

