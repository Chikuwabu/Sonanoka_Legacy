using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using System.Text;

public class GameController : SingletonMonoBehaviour<GameController> {

	[SerializeField] private TextMesh _key1;
	[SerializeField] private TextMesh _key2;
	[SerializeField] private TextMesh _key3;
	[SerializeField] private GameObject _measureBar;
	[SerializeField] private AudioClip _sou;
	[SerializeField] private AudioClip _nano;
	[SerializeField] private AudioClip _ka;
	[SerializeField] private AudioClip _yo;
	[SerializeField] private GameObject[] _foods;
	[SerializeField] private float _xSou = -2;
	[SerializeField] private float _xNano = 0;
	[SerializeField] private float _xKa = 2;
	[SerializeField] private Score _score;
	[SerializeField] private Range _excellentRange;
	[SerializeField] private Range _greatRange;
	[SerializeField] private Range _okRange;


	private bool isInitialized = false;
	private AudioSource bgm;
	private string[] _lines;
	private Queue<char> _line;
	private float _timePerLineElement => MusicTimer.Instance ? MusicTimer.Instance.TimePerMeasure / (_lines[MusicTimer.Instance.Measure].Length) : 0;
	private float _time;
	private bool _end;
	private readonly List<Func<string>> _debugCallbacks = new List<Func<string>>();
	private string _debug;

	/// <summary>
	/// 小節線のプレハブを取得します。
	/// </summary>
	public GameObject MeasureBar => _measureBar;
	public float XSou => _xSou;
	public float XNano => _xNano;
	public float XKa => _xKa;
	/// <summary>
	/// 現在の譜面を取得または設定します。
	/// </summary>
	public Score CurrentScore
	{
		get { return _score; }
		set { _score = !_score ? value : _score; }
	}

	// Use this for initialization
	void Start () {
		Check(_key1, "UI Key1 is not attached to GameContoller");
		Check(_key2, "UI Key2 is not attached to GameContoller");
		Check(_key3, "UI Key3 is not attached to GameContoller");
		Config cfg = Config.Instance;
		Check(cfg, "GameSettingManager is not attached to any gameObject.");
		_key1.text = cfg.Key1.ToString();
		_key2.text = cfg.Key2.ToString();
		_key3.text = cfg.Key3.ToString();
		Check(_foods.Length > 0, "Food registry has no element of gameObject.");
		if (!(bgm = gameObject.GetComponent<AudioSource>()))
		{
			bgm = gameObject.AddComponent<AudioSource>();
		}
	}

	/// <summary>
	/// 判定式が正しくなければエラーを表示します。
	/// </summary>
	void Check(bool logic, string message)
	{
		
		if (logic) return;
		if (Debug.isDebugBuild)
		{
			Debug.LogError(message);
			Debug.Break();
		}
		else
		{
			// TODO: Show error to display
		}
	}

	

	private void SpawnFood()
	{
		if (_line.Count == 0)
			return;
		switch (_line.Dequeue())
		{
			case 's':
				StartCoroutine(ControlFood(-2));
				break;
			case 'n':
				StartCoroutine(ControlFood(0));
				break;
			case 'k':
				StartCoroutine(ControlFood(2));
				break;
			case '-':
				// なにもしない
				break;

		}
	}
	
	public void AddDebugCallback(Func<string> callback)
	{
		if (callback == null)
			throw new ArgumentNullException(nameof(callback));
		_debugCallbacks?.Add(callback);
	}

	// Update is called once per frame
	void Update () {
		if (!_score)
			return;
		if (!isInitialized)
		{
			isInitialized = true;
			MusicTimer.Instance.Beat = _score.Beat;
			MusicTimer.Instance.Bpm = _score.Bpm;
			MusicTimer.Instance.MeasureChanged += s =>
			{
				if (_lines.Length < MusicTimer.Instance.Measure)
				{
					// TODO: 終了処理書く
				}
				if (_lines.Length <= MusicTimer.Instance.Measure)
				{
					_end = true;
				}

				if (MusicTimer.Instance.Measure == 1)
				{
					bgm.Play();
				}
				StartCoroutine(ControlBar());

				if (!_end)
				{
					_line = new Queue<char>(_lines[MusicTimer.Instance.Measure]);
					_time = _timePerLineElement;
				}
			};
			_lines = SplitScore(_score.Script);
			bgm.clip = _score.Music;
			MusicTimer.Instance.Play();
			SpawnFood();
			
		}
		if (!_end)
		{
			if (_time >= _timePerLineElement)
			{
				_time %= _timePerLineElement;
				SpawnFood();
			}

			_time += Time.deltaTime;
		}

		if (Input.GetKeyDown(Config.Instance.Key1))
		{
			bgm.PlayOneShot(_sou);
		}
		else if (Input.GetKeyDown(Config.Instance.Key2))
		{
			bgm.PlayOneShot(_nano);
		}
		else if (Input.GetKeyDown(Config.Instance.Key3))
		{
			bgm.PlayOneShot(_ka);
		}

		var sb = new StringBuilder();
		foreach (Func<string> a in _debugCallbacks)
			sb.AppendLine(a());
		_debug = sb.ToString();

	}

	private string[] SplitScore(string score) => score.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n').Select(a => Regex.Replace(a, "#.+$", "")).Where(a => !string.IsNullOrEmpty(a)).ToArray();

	private IEnumerator ControlBar()
	{
		GameObject go = Instantiate(MeasureBar);
		while (go.transform.position.z < 10)
		{
			go.transform.position += new Vector3(0, 0, 10 / MusicTimer.Instance.TimePerMeasure * Time.deltaTime);
			yield return null;
		}
		Destroy(go);
	}

	private IEnumerator ControlFood(float x)
	{
		var f = _foods.Random();
		GameObject go = Instantiate(f, new Vector3(x, 0, 0), f.transform.rotation);
		while (go.transform.position.z < 20)
		{
			if (x == XSou && Input.GetKeyDown(Config.Instance.Key1))
				Judge(go);

			go.transform.position += new Vector3(0, 0, 10 / MusicTimer.Instance.TimePerMeasure * Time.deltaTime);
			yield return null;
		}
		Destroy(go);
	}

	private void Judge(GameObject go)
	{
		var z = go.transform.position.z;
		if (_excellentRange.Contains(z))
		{

		}
		else if (_greatRange.Contains(z))
		{

		}
		else if (_okRange.Contains(z))
		{

		}
		else
		{

		}

	}

	private void OnGUI()
	{
		GUI.Label(new Rect(0, 0, Screen.width, Screen.height), _debug ?? "");
	}

}

[Serializable]
public struct Range
{
	[SerializeField] private float _a;
	[SerializeField] private float _b;

	public float A => _a;
	public float B => _b;
	public Range(float a, float b)
	{
		_a = a; _b = b;
	}

	public bool ContainsExcludeAB(float target) => A < target && target < B;
	public bool Contains(float target) => A <= target && target <= B;
}

static class Extensions
{
	internal static T Random<T>(this IEnumerable<T> source)
	{
		var count = source.Count();
		if (count == 0)
			return default(T);
		return source.ElementAtOrDefault(UnityEngine.Random.Range(0, count - 1));
	}

	/// <summary>
	/// Async Operation を待機します。
	/// </summary>
	/// <param name="a"></param>
	/// <param name="progressAction">進捗報告を行う処理のデリゲート。</param>
	/// <returns></returns>
	internal static IEnumerator Await(this AsyncOperation a, Action<float> progressAction = null)
	{
		while (!a.isDone)
		{
			progressAction?.Invoke(a.progress);
			yield return null;
		}
	}

}
