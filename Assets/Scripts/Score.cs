using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : ScriptableObject {
	[SerializeField] private string _title;
	[SerializeField] private string _subTitle;
	[SerializeField] private AudioClip _music;
	[SerializeField] private float _bpm = 120;
	[SerializeField] private Beat _beat = new Beat();
	[SerializeField] private float _pointExcellent = 300;
	[SerializeField] private float _pointGreat = 180;
	[SerializeField] private float _pointOk = 80;
	[SerializeField] private float _speedPerSecond = 1;
	[TextArea(3, 10)]
	[Tooltip(@"aa")]
	[SerializeField] private string _script;
	public string Title { get { return _title; } }
	public string SubTitle { get { return _subTitle; } }
	public AudioClip Music { get { return _music; } }
	public float PointExcellent { get { return _pointExcellent; } }
	public float PointGreat { get { return _pointGreat; } }
	public float PointOk { get { return _pointOk; } }
	public float PointBad { get { return _pointBad; } }
	public float SpeedPerSecond { get { return _speedPerSecond; } }
	public float Bpm { get { return _bpm; } }
	public Beat Beat  { get { return _beat; } }
	public string Script  { get { return _script; } }
}

[System.Serializable]
public class Beat
{
	[SerializeField] [Range(1, 32)] private int _note = 4;
	[SerializeField] [Range(1, 32)] private int _rhythm = 4;
	public int Note { get { return _note; } }
	public int Rhythm { get { return _rhythm; } }
}
