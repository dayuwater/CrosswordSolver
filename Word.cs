using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace CrosswordSolver
{
	// each cell in the board

	// data structure for words, including pending answer list
	public class Word
	{
		public List<DictionaryResult> pendingWords=new List<DictionaryResult>();
		public string currentWord="";
		public int length;
		public string clue = "";
		public int startX=-1;
		public int startY=-1;
		public bool horizontal = false;
		public string id = "1";
		public int totalCrossPoint = 0;

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
				for (int i = startX, j=0; i < startX+length; i++, j++)
				{
					b.cells[i,startY].answer = currentWord[j];

				}
			}
			else {

				for (int i = startY, j=0; i < startY + length; i++, j++)
				{
					b.cells[startX, i].answer = currentWord[j];

				}
			}
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

		public void LookUpDictionary()
		{
		    string query = clue;
			string pattern = currentWord;
			List<DictionaryResult> result = new List<DictionaryResult>();
			// test
			HtmlDocument doc = new HtmlDocument();
			HtmlWeb web = new HtmlWeb();
			doc = web.Load(String.Format("http://www.dictionary.com/fun/crosswordsolver?query={0}&pattern={1}&l={2}",
										 query, pattern, length));
			Console.WriteLine(doc);
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
		public List<Word> words=new List<Word>();

		public Word Find(string id)
		{
			return words.Find((obj) => (obj.id.Equals(id)));
		}


	}

}
