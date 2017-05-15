using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : SingletonMonoBehaviour<Config> {

	[SerializeField] private KeyCode _key1 = KeyCode.A;
	[SerializeField] private KeyCode _key2 = KeyCode.S;
	[SerializeField] private KeyCode _key3 = KeyCode.D;

	public KeyCode Key1
	{
		get { return _key1; }
		set { _key1 = value; }
	}

	public KeyCode Key2
	{
		get { return _key2; }
		set { _key2 = value; }
	}

	public KeyCode Key3
	{
		get { return _key3; }
		set { _key3 = value; }
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
