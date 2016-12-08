using System;
using System.Collections.Generic;
using System.IO;


namespace CrosswordSolver
{
	partial class MainClass
	{

		public static Board board = new Board();
		public static Words words = new Words();
		static Stack<Board> history = new Stack<Board>();

		public static void loadFile()
		{
			// test cases:
			// Grid 1-1, pass with 1 hint. IDAS(12)
			// Grid 1-2, fail
			// Grid 1-4, pass with 0 hint. Another solution

			string[] s = File.ReadAllLines("/Users/tanwang/Projects/CrosswordSolver/CrosswordSolver/clue4.csv");
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

		static bool FillTheCell(Word w)
		{
			w.LookUpDictionary();
			int currentWordIndex = 0;
			if (w.pendingWords.Count != 0)
			{
				// success, update the board, push current state to history
				start: w.currentWord = w.pendingWords[currentWordIndex].word;
				w.MarkAsVisited(board);
				w.WriteToCell(board);
				history.Push(board);
				board.PrintBoard();
				// update the children
				foreach (Word w1 in w.children)
				{
					if (!w1.IsVisited(board))
						w1.ReadFromCell(board);

				}
				// traverse the children
				foreach (Word w1 in w.children)
				{
					if (!w1.IsVisited(board))
					{
						// if one of the children is failed, reset the board and try another word
						if (!FillTheCell(w1))
						{
							history.Pop();
							board = history.Peek();
							if (currentWordIndex < w.pendingWords.Count - 1)
							{
								currentWordIndex++;
								goto start;

							}
							// no pending word to try, so the search is failed
							else 
							{
								return false;
							}
						}
					}

				}
				return true;

			}
			else
			{
				// fail
				Console.WriteLine("Failed to find a possible word for this clue");
				Console.WriteLine(w.id + " " + w.clue + " " + w.currentWord);
			a: Console.WriteLine("1=Manual Input, 2=Admit failure");
				string input = Console.ReadLine();
				int inp = -1;
				if (int.TryParse(input, out inp))
				{
					if (inp == 1)
					{
						// manual correction
						Console.WriteLine("Enter the word now");
						start:w.currentWord = Console.ReadLine();
						w.MarkAsVisited(board);
						w.WriteToCell(board);
						history.Push(board);
						board.PrintBoard();
						// update the children
						foreach (Word w1 in w.children)
						{
							if (!w1.IsVisited(board))
								w1.ReadFromCell(board);

						}
						// traverse the children
						foreach (Word w1 in w.children)
						{
							if (!w1.IsVisited(board))
							{
								// if one of the children is failed, reset the board and try another word
								if (!FillTheCell(w1))
								{
									history.Pop();
									board = history.Peek();
									if (currentWordIndex < w.pendingWords.Count - 1)
									{
										currentWordIndex++;
										goto start;

									}
									// no pending word to try, so the search is failed
									else
									{
										return false;
									}
								}
							}

						}
						return true;

					}

					else if (inp == 2)
					{
						// retry using other possible words

						return false;
					}
					else
					{
						Console.WriteLine("You must enter either 1 or 2");
						goto a;
					}
				}
				else
				{
					Console.WriteLine("You must enter either 1 or 2");
					goto a;
				}

			}

		}


		public static void Main(string[] args)
		{

			loadFile();
			SetupBoard();

			foreach (Word w in words.words)
			{
				if (!w.IsVisited(board))
				{
					if (!FillTheCell(w))
					{
						history = new Stack<Board>();
						board = new Board();
						SetupBoard();



					}
				}
			}









		}
	}
}
