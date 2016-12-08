using System;
using System.Collections.Generic;
using System.IO;


namespace CrosswordSolver
{
	partial class MainClass
	{

		public static Board board = new Board();
		public static Words words = new Words();
		static Stack<Board> history= new Stack<Board>();

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
			foreach (Word w in words.words)
			{
				w.GetChildren(board);
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

		static void FillTheCell(Word w)
		{
			w.LookUpDictionary();
			if (w.pendingWords.Count != 0)
			{
				// success, update the board, push current state to history
				w.currentWord = w.pendingWords[0].word;
				w.MarkAsVisited(board);
				w.WriteToCell(board);
				history.Push(board);
				board.PrintBoard();
				// update the children
				foreach (Word w1 in w.children)
				{
					if(!w1.IsVisited(board))
						w1.ReadFromCell(board);

				}
				// traverse the children
				foreach (Word w1 in w.children)
				{
					if(!w1.IsVisited(board))
						FillTheCell(w1);

				}

			}
			else 
			{
				// fail
			}

		}


		public static void Main(string[] args)
		{

			loadFile();
			SetupBoard();

			foreach(Word w in words.words)
			{
				if(!w.IsVisited(board))
					FillTheCell(w);
			}









		}
	}
}
