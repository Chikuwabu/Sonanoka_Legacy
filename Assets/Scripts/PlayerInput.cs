using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	[SerializeField] private float _dumpTime = 0.5f;
	Vector3 _pos;
	// Use this for initialization
	void Start () {
		_pos = transform.position;
	}
	Coroutine c;
	// Update is called once per frame
	void Update () {
		var key1 = Input.GetKeyDown(Config.Instance.Key1);
		var key2 = Input.GetKeyDown(Config.Instance.Key2);
		var key3 = Input.GetKeyDown(Config.Instance.Key3);
		if (key1) Move(GameController.Instance.XSou);
		if (key2) Move(GameController.Instance.XNano);
		if (key3) Move(GameController.Instance.XKa);
	}

	void Move(float x)
	{
		if (c != null)
			StopCoroutine(c);
		transform.position = new Vector3(x, transform.position.y, transform.position.z);
		_pos = new Vector3(x, _pos.y, _pos.z);
		c = StartCoroutine(Dump());
	}

	IEnumerator Dump()
	{
		
		var time = 0f;
		while (time < _dumpTime)
		{
			transform.position = new Vector3(_pos.x, MathHelper.EaseOut(time / _dumpTime, _pos.y - 1, _pos.y), _pos.z);
			time += Time.deltaTime;
			yield return null;
		}
	}


	
}

/// <summary>
/// 補完移動用のヘルパーメソッドを提供します。
/// </summary>
public static class MathHelper
{
	/// <summary>
	/// 加減速補完を行います。
	/// </summary>
	public static float EaseInOut(float time, float start, float end) =>
			(time /= 0.5f) < 1
			? (end - start) * 0.5f * time * time * time + start
			: (end - start) * 0.5f * ((time -= 2) * time * time + 2) + start;

	/// <summary>
	/// 加速補完を行います。
	/// </summary>
	public static float EaseIn(float time, float start, float end) => (end - start) * time * time * time + start;

	/// <summary>
	/// 減速補完を行います。
	/// </summary>
	public static float EaseOut(float time, float start, float end) => (end - start) * (--time * time * time + 1) + start;

	/// <summary>
	/// 線形補間を行います。
	/// </summary>
	public static float Linear(float time, float start, float end) => (end - start) * time + start;

	/// <summary>
	/// 線形補間を行います。
	/// </summary>
	public static float Linear(float time, float timeStart, float timeEnd, float start, float end) => Linear((time - timeStart) / (timeEnd - timeStart), start, end);


	/// <summary>
	/// 角度をラジアンに変換します。
	/// </summary>
	public static float ToRadian(float degree) => degree * 0.0055f * Mathf.PI;
}