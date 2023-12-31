﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenith
{
    internal class Program
    {
        static void Main()
        {
            Zenith.debugMessLog = true;
            Zenith.debugErrorLog = true;
            Zenith.debugSuccessLog = true;

            //string[] s = Zenith.Procesing.ArrayRandomizer(Zenith.Input.Strings());
            int[] s = Zenith.Procesing.OrderBy(Zenith.Input.Parameters());
            Zenith.Output.ArrayInLine(s);

            Console.ReadKey();
        }
    }
    public static class Zenith
    {
        #region Debug Enables
        public static bool debugMessLog = false;
        public static bool debugErrorLog = false;
        public static bool debugSuccessLog = false;
        public static bool debugWarningLog = false;
        #endregion
        #region Dictionaries

        private static readonly Dictionary<string, int> Directions = new Dictionary<string, int>()
        {
            { "UP", 1 },
            { "DOWN", 2 },
            { "RIGHT", 3 },
            { "LEFT", 4 },

            { "U_LEFT", 5 },
            { "D_LEFT", 6 },
            { "U_RIGHT", 7 },
            { "D_RIGHT", 8 },

            { "NONE", -1 }
        };
        private static readonly Dictionary<int, string> ReversedDirections = new Dictionary<int, string>()
        {
            { 1, "UP" },
            { 2, "DOWN" },
            { 3, "RIGHT" },
            { 4, "LEFT" },
               
            { 5, "U_LEFT" },
            { 6, "D_LEFT" },
            { 7, "U_RIGHT" },
            { 8, "D_RIGHT" },

            { -1, "NONE" },
        };
        
        #endregion

        public static class Input
        {
            public static bool TrySplit(string s, out int[] output, char spliter = ' ')
            {
                output = null;

                s = s.Trim();
                string[] split = s.Split(spliter);

                if (split.Length > 0)
                {
                    output = new int[split.Length];
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (!int.TryParse(split[i], out output[i]))
                            return false;
                    }
                }
                else
                    return false;

                return true;
            }
            public static char[,] CharArray(int x, int y, char spliter)
            {
                char[,] array = new char[x,y];

                for (int i = 0; i < y; i++)
                {
                    string vst = Console.ReadLine();
                    string[] splitedVst = vst.Split(spliter);

                    for (int j = 0; j < x; j++)
                        array[i, j] = splitedVst[j].ToArray()[0];
                }

                return array;
            }
            public static char[,] CharArray(int x, int y)
            {
                char[,] array = new char[x, y];

                for (int i = 0; i < y; i++)
                {
                    string vst = Console.ReadLine();
                    char[] splitedVst = vst.ToCharArray();

                    for (int j = 0; j < x; j++)
                        array[i, j] = splitedVst[j];
                }

                return array;
            }
            public static int[] Parameters(int limeter = -1, char spliter = ' ')
            {
                string vst = Console.ReadLine();
                try
                {
                    string[] numbers = vst.Trim().Split(spliter);

                    if (limeter == -1)
                        limeter = numbers.Length;

                    int[] result = new int[limeter];

                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = int.Parse(numbers[i].Trim());
                        DebugLog.Message("Parameters", "sucsfully added " + numbers[i]);
                    }
                    DebugLog.Success("Parameters entered: " + vst);
                    return result;
                }
                catch 
                { 
                    DebugLog.Error("Parameters failed to [Parse] value to [INTs]: " + vst);
                    return null;
                }
            }
            public static string[] Strings(char spliter = ' ')
            {
                return Console.ReadLine().Split(spliter);
            }
            public static int Intiger()
            {
                string s = Console.ReadLine();
                try
                { return int.Parse(s); }
                catch
                {
                    DebugLog.Error("Unable to parse: {" + s + "} to int");
                    return 0; 
                }                
            }
        }
        public static class Procesing
        {
            public static int[] OrderBy(int[] original, bool desc = false)
            {
                int[] result = new int[original.Length];
                List<int> list = original.ToList();

                for (int i = 0; i < result.Length; i++)
                {
                    int change;

                    if (desc)
                        change = list.Max();
                    else
                        change = list.Min();

                    result[i] = change;
                    list.RemoveAt(list.IndexOf(change));
                }
                DebugLog.Success("Ordred descending = " + desc);
                return result;
            }
            public static string[] ArrayInvertor(string[] original)
            {
                int until = (original.Length - 1) / 2;
                for (int i = 0, j = original.Length - 1; i < until; i++, j--)
                {
                    string temp = original[i];
                    original[i] = original[j];
                    original[j] = temp;
                    DebugLog.Message("ArrayInvertor", $"Switched strings: {original[i]} <=> {original[j]}\n\t[i,j] = [{i},{j}]");
                }
                return original;
            }
            public static int[] ArrayInvertor(int[] original)
            {
                if (original == null) 
                    return null;
                try
                {
                    if (original.Length <= 1)
                        return original;

                    int until = (original.Length - 1) / 2;
                    for (int i = 0, j = original.Length - 1; i < until; i++, j--)
                    {
                        int temp = original[i];
                        original[i] = original[j];
                        original[j] = temp;
                        DebugLog.Message("ArrayInvertor", $"Switched ints: {original[i]} <=> {original[j]}\n\t[i,j] = [{i},{j}]");
                    }
                    return original;
                }
                catch
                {
                    DebugLog.Error("Array cannot be inverted! ");
                    return null; 
                }
            }
            public static int CountChar(string input, char ch)
            {
                int ret = 0;

                char[] temp = input.ToCharArray();
                foreach (char c in temp)
                    if (c.Equals(ch))
                        ret++;

                return ret;
            }
            public static string[] ArrayRandomizer(string[] original)
            {
                List<int> used = new List<int>();
                Random r = new Random();
                string[] result = new string[original.Length];
                int i = 0;

                while (used.Count < original.Length)
                {
                    int j = r.Next(0, original.Length);
                    if (!used.Contains(j))
                    {
                        result[j] = original[i];
                        used.Add(j);
                        DebugLog.Message("ArrayRand(String)", $"Places switched {original[i]}[{i}] <=> {result[j]}[{j}]");
                        i++;
                    }
                }
                DebugLog.Success("String field was randomized");
                return result;
            }
            public static int[] ArrayRandomizer(int[] original)
            {
                List<int> used = new List<int>();
                Random r = new Random();
                int[] result = new int[original.Length];
                int i = 0;

                while (used.Count < original.Length)
                {
                    int j = r.Next(0, original.Length);
                    if (!used.Contains(j))
                    {
                        result[j] = original[i];
                        used.Add(j);
                        DebugLog.Message("ArrayRand(String)", $"Places switched {original[i]}[{i}] <=> {result[j]}[{j}]");
                        i++;
                    }
                }

                DebugLog.Success("Itiger field was randomized");
                return result;
            }
        }
        public static class Output
        {
            public static void OutBoolCase(bool b, string caseTrue, string caseFalse)
            {
                if (b)
                {
                    Console.WriteLine(caseTrue);
                }
                else
                {
                    Console.WriteLine(caseFalse);
                }
            }
            public static void ArrayInLine(string[] output, string between = " ")
            {
                foreach (string o in output) 
                { 
                    Console.Write(o + between);
                }
                Console.WriteLine();
            }            
            public static void ArrayInLine(int[] output, string between = " ")
            {
                if (output != null)
                    foreach (int o in output) 
                    { 
                        Console.Write(o + between);
                    }
                Console.WriteLine();
            }
        }
        public class Vector2
        {
            float X;
            float Y;
            public Vector2(float x, float y)
            {
                X = x;
                Y = y;
            }
        }
        public class Vector3
        {
            float X;
            float Y;
            float Z;
            public Vector3(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }
        public class CharArray
        {
            /// [x,y]
            /// x => horizontal axis
            /// y => vertical   axis
            char[,] pole;

            public CharArray(char[,] ch)
            {
                pole = ch;
            }
            public override string ToString()
            {
                string s = "";
                foreach (char c in pole)
                    s += c;
                return s;
            }

            public bool Find4(string s, bool diagonal)
            {
                char[] c = s.ToCharArray();
                string vs = ToString();
                int cIndex = 0;
                bool b = true;

                for (int i = 0; i < c.Length && b; i++)
                {
                    b = vs.Contains(c[i]);
                }

                DebugLog.Message("Find4", $"MODE: {DebugLog.BoolCase(diagonal, "Diagonal", "Strait")} SEARCHING FOR: {s}");

                if (b)
                {
                    DebugLog.Message("Find4", "Pole: " + vs + "\n\tContains symbols: " + s);
                    for (int y = 0; y < pole.GetLength(1); y++)
                        for (int x = 0; x < pole.GetLength(0); x++)
                            if (pole[x, y] == c[0])
                            {
                                DebugLog.Message("Find4", $"Target aquired [{x},{y}]");
                                cIndex = 1;
                                int[,] indexes = new int[c.Length, 2];
                                indexes[0, 0] = x;
                                indexes[0, 1] = y;

                                if (cIndex < c.Length)
                                {
                                    int[] surroundings;

                                    if (diagonal)
                                        surroundings = CheckForDirectionDia(x, y, c[cIndex]);
                                    else
                                        surroundings = CheckForDirection4(x, y, c[cIndex]);

                                    foreach (int neighbour in surroundings)
                                    {
                                        bool sucsess = true;
                                        for (int i = cIndex; i < c.Length; i++)
                                        {
                                            if (CheckInDirection(ref x, ref y, c[i], neighbour))
                                            {
                                                indexes[i, 0] = x;
                                                indexes[i, 1] = y;
                                            }
                                            else
                                            {
                                                sucsess = false;
                                                break;
                                            }
                                        }
                                        if (!sucsess)
                                        {
                                            x = indexes[0, 0];
                                            y = indexes[0, 1];
                                            continue;
                                        }
                                        else
                                        {
                                            string cor = "";

                                            for (int i = 0; i < indexes.GetLength(0); i++)
                                                cor += "\n\t\t[" + indexes[i, 0] + "," + indexes[i, 1] + "]";

                                            DebugLog.Message("Find4 " + DebugLog.BoolCase(diagonal, "Diagonal", "Strait"), $"Found: {s}\n\tOn cordinates: " + cor);
                                            return true;
                                        }
                                    }
                                }
                                else
                                    return true;
                            }
                }
                DebugLog.Message("Find4", "Pole: " + vs + "\n\tDoes not contain: " + s);
                return false;
            }
            public bool Find8(string s)
            {
                return Find4(s, false) || Find4(s, true);
            }

            private int[] CheckForDirection4(int x, int y, char ch)
            {
                List<int> result = new List<int>();

                if (y > 0)
                    if (pole[x, y - 1] == ch)
                        result.Add(Directions["UP"]);
                if (y < pole.GetLength(1) - 1)
                    if (pole[x, y + 1] == ch)
                        result.Add(Directions["DOWN"]);
                if (x > 0)
                    if (pole[x - 1, y] == ch)
                        result.Add(Directions["LEFT"]);
                if (x < pole.GetLength(0) - 1)
                    if (pole[x + 1, y] == ch)
                        result.Add(Directions["RIGHT"]);

                DebugLog.Message("CheckForDirection4", "Number of possible ways: " + result.Count);
                return result.ToArray();
            }
            private int[] CheckForDirectionDia(int x,int y, char ch)
            {
                List<int> result = new List<int>();

                if (x > 0 && y > 0)
                    if (pole[x - 1, y - 1] == ch)
                        result.Add(Directions["U_LEFT"]);
                if (x < pole.GetLength(0) - 1 && y > 0)
                    if (pole[x + 1, y - 1] == ch)
                        result.Add(Directions["U_RIGHT"]);
                if (x > 0 && y < pole.GetLength(1) - 1)
                    if (pole[x - 1, y + 1] == ch)
                        result.Add(Directions["D_LEFT"]);
                if (x < pole.GetLength(0) - 1 && y < pole.GetLength(1) - 1)
                    if (pole[x + 1, y + 1] == ch)
                        result.Add(Directions["D_RIGHT"]);

                DebugLog.Message("CheckForDirectionDia", "Number of possible ways: " + result.Count);
                return result.ToArray();
            }
            private bool CheckInDirection(ref int x, ref int y, char ch, int dir)
            {
                bool possible = false;
                int oldX = x, oldY = y;

                if (dir == Directions["UP"] && y - 1 >= 0)
                {
                    possible = true;
                    y--;
                }
                else if (dir == Directions["DOWN"] && y + 1 < pole.GetLength(1))
                {
                    possible = true;
                    y++;
                }
                else if (dir == Directions["LEFT"] && x - 1 >= 0)
                {
                    possible = true;
                    x--;
                }
                else if (dir == Directions["RIGHT"] && x + 1 < pole.GetLength(0))
                {
                    possible = true;
                    x++;
                }

                else if (dir == Directions["U_LEFT"] && x > 0 && y > 0)
                {
                    possible = true;
                    x--;
                    y--;
                }
                else if (dir == Directions["D_LEFT"] && x > 0 && y + 1 < pole.GetLength(1))
                {
                    possible = true;
                    x--;
                    y++;
                }
                else if (dir == Directions["U_RIGHT"] && x + 1 < pole.GetLength(0) && y > 0)
                {
                    possible = true;
                    x++;
                    y--;
                }
                else if (dir == Directions["D_RIGHT"] && x + 1 < pole.GetLength(0) && y + 1 < pole.GetLength(1))
                {
                    possible = true;
                    x++;
                    y++;
                }
                if (possible)
                    if (pole[x, y] == ch)
                    {
                        DebugLog.Message("CheckInDirection", $"Start position: [{oldX},{oldY}]\n\tChecked direction: {ReversedDirections[dir]}\n\tContains: {ch}\n\tNew position [{x},{y}]");
                        return true;
                    }

                bool border = x + 1 < pole.GetLength(0) && x - 1 >= 0 && y + 1 < pole.GetLength(1) && y - 1 >= 0;

                DebugLog.Message("CheckInDirection", $"Start position: [{oldX},{oldY}]\n\tChecked direction: {ReversedDirections[dir]}\n\tOn the edge: {!border}\n\tDoes not contain: {ch}\n\tActual position [{x},{y}]");

                return false;
            }
        }
        private static class DebugLog
        {
            public static void Error(string mess)
            {
                if (debugErrorLog)
                {
                    Breackets("ERROR", ConsoleColor.Red);
                    Console.WriteLine(mess);
                }
            }
            public static void Warning(string mess)
            {
                if (debugWarningLog)
                {
                    Breackets("WARNING", ConsoleColor.Yellow);
                    Console.WriteLine(mess);
                }
            }
            public static void Success(string mess)
            {
                if (debugSuccessLog)
                {
                    Breackets("SUCSESS", ConsoleColor.Green);
                    Console.WriteLine(mess);
                }
            }
            public static void Message(string source, string mess)
            {
                if (debugMessLog)
                {
                    Breackets(source, ConsoleColor.Blue);
                    Console.WriteLine(mess);
                }
            }
            public static string BoolCase(bool b, string caseTrue, string caseFalse)
            {
                if (b)
                    return caseTrue;
                else
                    return caseFalse;
            }

            private static void Breackets(string log, ConsoleColor color)
            {
                Console.Write("[ ");
                ConsoleColor c = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.Write(log);
                Console.ForegroundColor = c;
                Console.Write(" ]  ");
            }
        }
    }
}