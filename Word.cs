using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace CrosswordSolver
{

	public struct DictionaryResult
	{
		public int confidence;
		public string word;


	}
	// each cell in the board

	// data structure for words, including pending answer list
	public class Word
	{
		public List<DictionaryResult> pendingWords = new List<DictionaryResult>();
		public string currentWord = "";
		public int length;
		public string clue = "";
		public int startX = -1;
		public int startY = -1;
		public bool horizontal = false;
		public string id = "1";
		public int totalCrossPoint = 0;
		public List<Word> children = new List<Word>();

		public void InitCell(Board b)
		{
			if (!horizontal)
			{
				for (int i = startX, j = 0; i < startX + length; i++, j++)
				{
					b.cells[i, startY].ids.Add(id);

				}
			}
			else {

				for (int i = startY, j = 0; i < startY + length; i++, j++)
				{
					b.cells[startX, i].ids.Add(id);

				}
			}

		}

		public void WriteToCell(Board b)
		{
			if (!horizontal)
			{
				for (int i = startX, j = 0; i < startX + length; i++, j++)
				{
					b.cells[i, startY].answer = currentWord[j];

				}
			}
			else {

				for (int i = startY, j = 0; i < startY + length; i++, j++)
				{
					b.cells[startX, i].answer = currentWord[j];

				}
			}
		}

		public void GetChildren(Board b)
		{
			List<Word> result = new List<Word>();
			if (!horizontal)
			{
				for (int i = startX, j = 0; i < startX + length; i++, j++)
				{
					b.cells[i, startY].answer = currentWord[j];
					result = result.Union(MainClass.words.FindAll(b.cells[i, startY].ids)).ToList();

				}
			}
			else {

				for (int i = startY, j = 0; i < startY + length; i++, j++)
				{
					b.cells[startX, i].answer = currentWord[j];
					result = result.Union(MainClass.words.FindAll(b.cells[startX, i].ids)).ToList();

				}
			}
			children = result;
			children.Remove(this);
			children.Sort((x, y) =>
			{
				if (x.totalCrossPoint < y.totalCrossPoint)
				{
					return 1;
				}
				else
				{
					return -1;
				}
			});

		}

		public void ReadFromCell(Board b)
		{
			StringBuilder builder = new StringBuilder();
			int count = 0;
			if (!horizontal)
			{
				for (int i = startX, j = 0; i < startX + length; i++, j++)
				{
					builder.Append(b.cells[i, startY].answer);
					count += b.cells[i, startY].Count();
				}
			}
			else
			{
				for (int i = startY, j = 0; i < startY + length; i++, j++)
				{
					builder.Append(b.cells[startX, i].answer);
					count += b.cells[startX, i].Count();
				}
			}
			currentWord = builder.ToString();
			totalCrossPoint = count - length;
		}



		public bool IsVisited(Board b)
		{
			return b.visited.Contains(id);
		}

		public void MarkAsVisited(Board b)
		{
			b.visited.Add(id);
		}

		private string getPattern(string s)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '?')
				{
					builder.Append("%3F");
				}
				else
				{
					builder.Append(s[i]);
				}
			}
			return builder.ToString();
		}

		private string getQuery(string s)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < s.Length; i++)
			{

				if (s[i] == ' ')
				{
					builder.Append("+");

				}
				else if (s[i] == 8217)
				{
					builder.Append("%27");
				}
				else if (s[i] == '-')
				{
					builder.Append("%2C");
				}
				else
				{
					builder.Append(s[i]);
				}
			}
			return builder.ToString();
		}


		public void LookUpDictionary()
		{
			string query = getQuery(clue);
			string pattern = getPattern(currentWord);
			List<DictionaryResult> result = new List<DictionaryResult>();
			// test
			HtmlDocument doc = new HtmlDocument();
			HtmlWeb web = new HtmlWeb();
			string url = String.Format("http://www.dictionary.com/fun/crosswordsolver?query={0}&pattern={1}&l={2}",
									 query, pattern, length);
			doc = web.Load(url);

			Console.WriteLine(url);
			var body = doc.DocumentNode.ChildNodes[2].ChildNodes[3].ChildNodes;

			var divs = body.Where((o) => (o.Name.Equals("div"))).ToList();
			var content = divs.Where((o) => (o.Attributes[0].Value.Equals("main-container"))).ToList();
			var answersContainer = content[0].ChildNodes[1].ChildNodes[1].ChildNodes[3].ChildNodes[1].ChildNodes[1].ChildNodes;

			var answersDivs = answersContainer.Where((o) =>
			{
				if (o.Attributes.Count != 0)
					return o.Attributes[0].Value.Contains("result-row");

				return false;
			}).ToList();
			for (int i = 1; i < answersDivs.Count; i++)
			{
				var confidence = answersDivs[i].ChildNodes[1].InnerText.Substring(2).Trim();
				var answer = answersDivs[i].ChildNodes[3].InnerText.Substring(2).Trim();
				DictionaryResult thisResult = new DictionaryResult();
				thisResult.confidence = int.Parse(confidence.Substring(0, confidence.Length - 1));
				thisResult.word = answer;
				result.Add(thisResult);
			}




			pendingWords = result;
		}

	}

	public class Words
	{
		public List<Word> words = new List<Word>();





		public Word Find(string id)
		{
			return words.Find((obj) => (obj.id.Equals(id)));
		}
		public List<Word> FindAll(List<string> ids)
		{
			List<Word> result = new List<Word>();
			foreach (string id in ids)
			{
				result.Add(Find(id));
			}

			return result;
		}





	}

}
