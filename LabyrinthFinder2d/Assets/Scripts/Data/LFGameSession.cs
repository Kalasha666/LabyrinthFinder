using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using System;

namespace LFData
{
	public enum LFExitType {
		[XmlEnum("0")] none = 0,
		[XmlEnum("1")] exit = 1, 
		[XmlEnum("2")] gameOver = 2
	}

	public class LFGameSession: IComparable<LFGameSession> {
		[XmlIgnore]
		protected int _idSession;
		[XmlIgnore]
		protected string _playerName;
		[XmlIgnore]
		protected int _coinCount;
		[XmlIgnore]
		protected float _gameTime;
		[XmlIgnore]
		protected string _startDate;
		[XmlIgnore]
		protected LFExitType _exitType;
		
		[XmlAttribute("IdSession")]
		public int IdSession
		{
			get{ return _idSession;}
			set{ _idSession = value;}
		}
			
		public string PlayerName
		{
			get{ return _playerName;}
			set{ _playerName = value;}
		}
			
		public int CoinCount
		{
			get{ return _coinCount;}
			set{ _coinCount = value;}
		}
			
		public float GameTime
		{
			get{ return _gameTime;}
			set{ _gameTime = value;}
		}
			
		public string StartDate
		{
			get{ return _startDate;}
			set{ _startDate = value;}
		}
			
		public LFExitType ExitType
		{
			get{ return _exitType;}
			set{ _exitType = value;}
		}

		public LFGameSession()
		{
			_idSession = 0;
			_playerName = "Player";
			_coinCount = 0;
			_gameTime = 0;
			_startDate = System.DateTime.Now.ToString ("dd.MM.yyyy");
			_exitType = LFExitType.none;
		}

		public LFGameSession(int idSession, string playerName)
		{
			_idSession = idSession;
			_playerName = playerName;
			_coinCount = 0;
			_gameTime = 0;
			_startDate = System.DateTime.Now.ToString ("dd.MM.yyyy");
			_exitType = LFExitType.none;
		}

		public int CompareTo(LFGameSession other) {
			return _coinCount.CompareTo(other.CoinCount);
		}
	}
}
