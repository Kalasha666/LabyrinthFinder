using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LFData;
using LFInput;

public class LFSessionManager : MonoBehaviour {

	private LFSessionContainer _container;
	private LFGameSession _session;
	private static LFSessionManager _instance = null;

	public static LFSessionManager Instance
	{
		get {return  _instance;}
	}

	// Use this for initialization
	void Start () {
		LoadXml ();

		if(_session == null)
		{
			_session = _container.AddNewSession ();
		}
	}

	void Awake()
	{
		if (_instance) {
			DestroyImmediate (gameObject);
			return;
		}

		_instance = this;
		DontDestroyOnLoad (gameObject);
	}

	public void AddGameSession()
	{
		_session = _container.AddNewSession ();
	}

	public LFGameSession GetCurrentSession()
	{
		return _session;
	}

	public List<LFGameSession> GetSessions()
	{
		_container.SortedByCoinSessions ();
		return _container.GetSessions ();
	}

	public void Save()
	{
		string path = GetXmlPath ();
		SaveXml (path);
	}

	public void Clean()
	{
		_container.CleanSessions ();
	}
	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy() {
		
	}

	private void LoadXml()
	{
		string path = GetXmlPath ();

		if (!System.IO.File.Exists (path)) {
			path = GetTemplateXmlPath ();
			ReadXml (path);
		} 
		else 
		{
			_container = new LFSessionContainer();
		}
			
	}

	private string GetTemplateXmlPath()
	{
		return System.IO.Path.Combine (Application.dataPath, "XML/gameSession.xml");
	}

	private string GetXmlPath()
	{
		return System.IO.Path.Combine (Application.persistentDataPath, "gameSession.xml");
	}

	private void ReadXml(string path)
	{ 
		_container = LFSessionContainer.Load(path);
	}

	private void SaveXml(string path)
	{ 
		_container.Save(path);
	}
}
