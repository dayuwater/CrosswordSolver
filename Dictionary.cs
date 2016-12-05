﻿using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace CrosswordSolver
{
	partial class MainClass
	{
		// TODO: Add parameters for queries
		public static List<DictionaryResult> LookUpDictionary()
		{
			List<DictionaryResult> result = new List<DictionaryResult>();

			HtmlDocument doc = new HtmlDocument();
			HtmlWeb web = new HtmlWeb();
			doc = web.Load("http://www.dictionary.com/fun/crosswordsolver?query=pop+singers&pattern=&l=9");
			Console.WriteLine(doc);
			var body = doc.DocumentNode.ChildNodes[2].ChildNodes[3].ChildNodes;

			var divs = body.Where((o) => (o.Name.Equals("div"))).ToList();
			var content = divs.Where((o) => (o.Attributes[0].Value.Equals("main-container"))).ToList();
			var answersContainer = content[0].ChildNodes[1].ChildNodes[1].ChildNodes[3].ChildNodes[1].ChildNodes[1].ChildNodes;
			// TODO: fix the inner function so that this does not throw exceptions
			var answersDivs = answersContainer.Where((o) => {
				if (o.Attributes.Count != 0)
					return o.Attributes[0].Value.Contains("result-row");
				
				return false;
			}).ToList();
			for (int i = 1; i < answersDivs.Count; i++)
			{
				var confidence = answersDivs[i].ChildNodes[1].InnerText.Substring(2).Trim();
				var answer=answersDivs[i].ChildNodes[3].InnerText.Substring(2).Trim();
				DictionaryResult thisResult = new DictionaryResult();
				thisResult.confidence = int.Parse(confidence.Substring(0,confidence.Length-1));
				thisResult.word = answer;
				result.Add(thisResult);
			}




			return result;
		}


	}

	public struct DictionaryResult
	{
		public int confidence;
		public string word;


	}


}