using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace AClassyProblem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var currentCell = new Cell();
            var cellByPosition = new Dictionary<(int x, int y), Cell>();
            var currentX = 0;
            var currentY = 0;
            cellByPosition.Add((0, 0), currentCell);
            while (true)
            {
                Direction move = Direction.None;
                if (currentCell.MovesLeft.Count > 0)
                {
                    foreach (var m in currentCell.MovesLeft)
                    {
                        var delta = GetDelta(m);
                        var guessX = currentX + delta.x;
                        var guessY = currentY + delta.y;
                        if (!cellByPosition.ContainsKey((guessX, guessY)))
                        {
                            move = m;
                            break;
                        }
                    }

                    if (move == Direction.None)
                    {
                        move = currentCell.MovesLeft[0];
                    }
                    
                    currentCell.MovesLeft.Remove(move);
                }
                else if (currentCell.BackMoves.Count > 0)
                {
                    move = currentCell.BackMoves.Last();
                    currentCell.BackMoves.Remove(move);
                }
                else
                {
                    Console.WriteLine("no way out");
                    return;
                }

                Console.WriteLine(move.ToString().ToLower());
                var answer = Console.ReadLine();
                if (answer == "wall")
                {
                    continue;
                }
                else if (answer == "ok")
                {
                    var delta = GetDelta(move);
                    currentX += delta.x;
                    currentY += delta.y;
                    if (currentCell.Cells.TryGetValue(move, out var newCell))
                    {
                        currentCell = newCell;
                    }
                    else if (cellByPosition.TryGetValue((currentX, currentY), out var otherCell))
                    {
                        currentCell.Cells.Add((move), otherCell);
                        otherCell.Cells.Add((Reverse(move)), currentCell);
                        currentCell = otherCell;
                        //if (otherCell.MovesLeft.Remove(Reverse(move))
                        otherCell.MovesLeft.Remove(Reverse(move));
                        otherCell.BackMoves.Add(Reverse(move));
                    }
                    else
                    {
                        var cell = new Cell();
                        var backMove = Reverse(move);
                        cell.Cells.Add(backMove, currentCell);
                        currentCell.Cells.Add(move, cell);
                        cell.BackMoves.Add(Reverse(move));
                        cell.MovesLeft.Remove(backMove);
                        currentCell = cell;
                        cellByPosition.Add((currentX, currentY), cell);
                    }
                }
                else
                {
                    return;
                }
            }
        }

        static Direction Reverse(Direction dir) => dir switch
        {
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up
        };

        static (int x, int y) GetDelta(Direction dir) => dir switch
        {
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            Direction.Down => (0, 1),
            Direction.Up => (0, -1),
        };

        private class Cell
        {
            public Dictionary<Direction, Cell> Cells = new Dictionary<Direction, Cell>();

            public List<Direction> MovesLeft = new List<Direction>() { Direction.Left, Direction.Right, Direction.Down, Direction.Up};

            public List<Direction> BackMoves { get; set; } = new List<Direction>();
        }

        private enum Direction
        {
            Left,
            Right,
            Up,
            Down,
            None
        }
    }
}