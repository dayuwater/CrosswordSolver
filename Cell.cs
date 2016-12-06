using System;
using System.Collections.Generic;
using System.Text;

namespace CrosswordSolver
{
	// each cell in the board
	public class Cell
	{
		public List<string> ids; // the ids for all the words containing this cell
		public char answer; // the character this cell currently have
		public Cell()
		{
			ids = new List<string>();
			answer = '?';
		}

		public string PrintIds()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("(");
			foreach (string s in ids)
			{
				builder.Append(s);
				builder.Append(",");
			}
			builder.Append(")");
			return builder.ToString();
		}

		public int Count()
		{
			return ids.Count;
		}


	}

	// data structure for words, including pending answer list
	

	public class Board
	{
		public Cell[,] cells;
		public List<string> visited;

		public Board()
		{
			cells = new Cell[9, 9];
			visited = new List<string>();

			for (int i = 0; i < cells.GetLength(0); i++)
			{
				for (int j = 0; j < cells.GetLength(1); j++)
				{
					cells[i, j] = new Cell();
				}
			}
		}

		public void PrintBoard()
		{
			Console.WriteLine("Word ids");
			for (int i = 0; i < cells.GetLength(0); i++)
			{
				for (int j = 0; j < cells.GetLength(1); j++)
				{
					Console.Write(cells[i, j].Count() + " ");
				}
				Console.WriteLine();
			}
			Console.WriteLine("Current char");
			for (int i = 0; i < cells.GetLength(0); i++)
			{
				for (int j = 0; j < cells.GetLength(1); j++)
				{
					Console.Write(cells[i, j].answer + " ");
				}
				Console.WriteLine();
			}
			Console.WriteLine("Current Visited");
			foreach (string s in visited)
			{
				Console.Write(s + "->");
			}
		}
	}


}
