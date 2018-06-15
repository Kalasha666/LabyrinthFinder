using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace LFData
{
	[XmlRoot("GameSessions")]
	public class LFSessionContainer {
		[XmlArray("Sessions"),XmlArrayItem("Session")]
		public List<LFGameSession> _sessions;

		public void Save(string path)
		{
			var serializer = new XmlSerializer(typeof(LFSessionContainer));
			using(var stream = new FileStream(path, FileMode.Create))
			{
				serializer.Serialize(stream, this);
			}
		}

		public LFSessionContainer()
		{
			_sessions = new List<LFGameSession>();
		}

		public LFGameSession AddNewSession(string playerName = "Player")
		{
			LFGameSession newGameSession = new LFGameSession (_sessions.Count, playerName);
			_sessions.Insert(0, newGameSession);
			return newGameSession;
		}

		public List<LFGameSession> GetSessions()
		{
			return _sessions;
		}

		public void SortedByCoinSessions()
		{
			_sessions.Sort((a, b) => -1* a.CompareTo(b));
		}

		public void CleanSessions()
		{
			_sessions.Clear ();
		}

		public static LFSessionContainer Load(string path)
		{
			var serializer = new XmlSerializer(typeof(LFSessionContainer));
			using(var stream = new FileStream(path, FileMode.Open))
			{
				return serializer.Deserialize(stream) as LFSessionContainer;
			}
		}
			
		public static LFSessionContainer LoadFromText(string text) 
		{
			var serializer = new XmlSerializer(typeof(LFSessionContainer));
			return serializer.Deserialize(new StringReader(text)) as LFSessionContainer;
		}
	}
}
