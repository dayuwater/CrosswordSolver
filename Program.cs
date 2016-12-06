using System;
using System.Collections.Generic;
using System.IO;


namespace CrosswordSolver
{
	partial class MainClass
	{

		static Board board = new Board();
		static Words words = new Words();
		static Stack<Board> history;

		public static void loadFile()
		{

			string[] s = File.ReadAllLines("/Users/tanwang/Projects/CrosswordSolver/CrosswordSolver/clue1.csv");
			int count = 0;
			foreach (string s1 in s)
			{
				string[] s2 = s1.Split(',');
				if (count > 0)
				{
					Word w = new Word();
					w.clue = s2[2];
					w.horizontal = s2[1].Equals("horizontal");
					w.id = s2[0];
					w.length = int.Parse(s2[3]);
					w.startX = int.Parse(s2[4]);
					w.startY = int.Parse(s2[5]);
					words.words.Add(w);

				}

				count++;


			}



		}

		public static void SetupBoard()
		{
			foreach (Word w in words.words)
			{
				w.InitCell(board);
			}
			foreach (Word w in words.words)
			{
				w.ReadFromCell(board);
			}
			words.words.Sort((x, y) =>
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


		public static void Main(string[] args)
		{

			loadFile();
			SetupBoard();







			board.PrintBoard();
		}
	}
}
