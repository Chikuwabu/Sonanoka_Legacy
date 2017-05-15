using System.Collections;
using UnityEngine;
public class MusicTimer : SingletonMonoBehaviour<MusicTimer>
{
	private int _measure;

	public float Bpm { get; set; } 
	public Beat Beat { get; set; }
	public bool IsRunning { get; private set; }
	public int Measure
	{
		get { return _measure; }
		set
		{
			_measure = value;
			MeasureChanged?.Invoke(this);
		}
	}

	public float TimePerMeasure => (float)Beat?.Rhythm / Beat?.Note * 240 / Bpm ?? 0;

	public float Time { get; private set; }

	public event System.Action<MusicTimer> MeasureChanged;

	public void Play()
	{
		if (IsRunning)
			return;
		if (Bpm == 0 || Beat == null)
		{
			Debug.LogError("Bpm or Beat are not set.");
			Debug.Break();
		}
		IsRunning = true;
		Measure = 0;
		StartCoroutine(StartAsync());

	}

	private IEnumerator StartAsync()
	{
		while (IsRunning)
		{
			Time += UnityEngine.Time.deltaTime;
			if (Time > TimePerMeasure)
			{
				Time = 0;
				Measure++;
			}
			yield return null;
		}
	}

	private void OnGUI()
	{

	}

	public void Stop()
	{
		if (!IsRunning)
			return;
		IsRunning = false;
	}

}
