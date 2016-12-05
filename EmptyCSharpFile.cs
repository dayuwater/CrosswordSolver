using System;
using HtmlAgilityPack;
using CsvHelper;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace WebWrangle
{
	class MainClass
	{
		static List<HtmlNode> nodes = new List<HtmlNode>();
		static List<HtmlNode> filtered = new List<HtmlNode>();
		public static void main(string[] args)
		{
			// url is the test file 
			HtmlDocument doc = new HtmlDocument();


			doc.LoadHtml(File.ReadAllText("/Users/tanwang/Downloads/testSingleWrangle2.html"));

			var childnotes = doc.DocumentNode.ChildNodes;
			// find all c3/c6/c5 with the content is COUNSEL:
			// the whole c6 block is the content of COUNSEL
			foreach (HtmlNode node in childnotes)
			{
				foreach (HtmlNode n2 in node.ChildNodes)
				{
					if (n2.Name.Equals("body"))
					{
						foreach (HtmlNode n3 in n2.ChildNodes)
						{
							nodes.Add(n3);
						}
					}
				}
				Console.WriteLine();
			}

			// filtering goes here
			foreach (HtmlNode node in nodes)
			{
				if (node.OuterHtml.Contains("COUNSEL:"))
				{

					filtered.Add(node);

				}

			}



			// print the filtered result

			foreach (HtmlNode node in filtered)
			{
				Console.WriteLine(node.OuterHtml);
				Console.WriteLine("-----");
			}






		}
	}
}
