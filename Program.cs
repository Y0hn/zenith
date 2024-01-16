using System;
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
            Zenith.debugWarningLog = true;

            int pocet = Zenith.Input.Intiger();
            Zenith.LineInts tricka = new Zenith.LineInts(Zenith.Input.Parameters(pocet));

            int[] distc = tricka.GetDistinct();

            int[] maxes = { 0, 0};

            for (int i = 0; i < distc.Length;  i++) 
            {
                int max = Zenith.Procesing.HighestNearCount(tricka.IndexsOf(distc[i]));

                if      (max > maxes[1])
                    maxes[1] = max;
                else if (max > maxes[0])
                    maxes[0] = max;
            }

            Console.WriteLine(maxes[0] + maxes[1]);

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
        public static readonly Dictionary<int, char> Numbers = new Dictionary<int, char>()
        {
            { 0, '0' },
            { 1, '1' },
            { 2, '2' },
            { 3, '3' },
            { 4, '4' },
            { 5, '5' },
            { 6, '6' },
            { 7, '7' },
            { 8, '8' },
            { 9, '9' },
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
                char[,] array = new char[x, y];

                for (int i = 0; i < y; i++)
                {
                    string vst = Console.ReadLine();
                    string[] splitedVst = vst.Split(spliter);

                    for (int j = 0; j < x; j++)
                        array[j, i] = splitedVst[j].ToArray()[0];
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
                        array[j, i] = splitedVst[j];

                    DebugLog.Message("CharArray", $"Added new line to array: {vst} from [0,{i}] to [{x - 1},{i}]");
                }

                return array;
            }
            public static int[] Parameters(int limeter = -1, int modifier = 0, char spliter = ' ')
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
                        result[i] = int.Parse(numbers[i].Trim()) + modifier;
                        DebugLog.Message("Parameters", "sucsfully added " + numbers[i] + " modified by: " + modifier);
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
            public static int[,] MultiParam(int limitVert, int limitHoriz = -1, char spliter = ' ')
            {
                int[,] input = null;

                if (limitHoriz > 0)
                    input = new int[limitHoriz,limitVert];

                for (int y = 0; y < limitVert; y++)
                {
                    int[] temp = Parameters(limitHoriz, spliter);

                    if (limitHoriz < 0)
                        limitHoriz = temp.Length;

                    for (int x = 0; x < limitHoriz; x++)
                    {
                        input[x,y] = temp[x];
                    }
                }

                return input;
            }
            public static string[] Strings(char spliter = ' ')
            {
                return Console.ReadLine().Split(spliter);
            }
            public static char[] Chars(char spliter = ' ')
            {
                string input = Console.ReadLine();
                string[] vst = input.Split(spliter);

                List<char> ch = new List<char>();

                foreach (string s in vst)
                {
                    ch.Add(s[0]);
                    DebugLog.Message("Chars", $"{s[0]} added to array from {s} from {input}");
                }
                char[] result = ch.ToArray();

                string debug = "";
                foreach (char res in result)
                {
                    debug += res.ToString() + spliter.ToString();
                }

                DebugLog.Success("Input recieved: " + debug);

                return result;
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
            public static int GetMaxInRangeOfArray(int[] original, int start, int lenght)
            {
                int max = int.MinValue;

                for (int i = start; i < lenght; i++)
                {
                    if (original.Length - 1 > i)
                    {
                        if (original[i] > max)
                            max = original[i];
                    }
                    else
                        DebugLog.Error($"Index {i} outside of bounds {original.Length-1} of array");
                }

                return max;
            }
            public static bool TryGetMaxInRangeOfArray(int[] original, int start, int lenght, out int max)
            {
                max = int.MinValue;

                if (start > original.Length - 1)
                {
                    DebugLog.Warning($"Start {start} is out of bounds [0] <=> [{original.Length - 1}]");
                    return false;
                }

                for (int i = start; i < start + lenght; i++)
                {
                    if (i < original.Length)
                    {
                        if (original[i] > max)
                        {
                            max = original[i];
                        }
                    }
                    else
                    {
                        DebugLog.Warning($"Index {i} outside of bounds {original.Length - 1} of array");
                        return false;
                    }
                }              
                
                DebugLog.Message("TryGetMaxInRangeOfArray", $"{max} is max in range from {start} to {start + lenght - 1}");
                
                return true;
            }
            public static int[] MultiplyEveryElse(int[] original)
            {
                int[] result = new int[original.Length];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = 1;
                    for (int j = 0; j < original.Length; j++)
                    {
                        if (i == j)
                            continue;
                        else
                        {
                            result[i] *= original[j];
                        }
                    }
                }
                return result;
            }
            public static int HighestNearCount(int[] indexes)
            {
                int tempMax = 1;
                int maxNear = 1;
                int last = indexes[0];

                indexes = OrderBy(indexes);

                for (int i = 1; i <= indexes.Length; i++)
                {
                    if (i < indexes.Length)
                    {
                        if (last + 1 == indexes[i])
                            tempMax++;
                        else
                        {
                            DebugLog.Message("HighestNearCount", $"{last} + 1 != {indexes[i]}");
                            if (tempMax > maxNear)
                                maxNear = tempMax;
                            tempMax = 0;
                        }
                        last = indexes[i];
                    }
                    else if (tempMax > maxNear)
                        maxNear = tempMax;
                }
                string debug = "";
                foreach (int i in indexes)
                {
                    debug += i + " ";
                }
                DebugLog.Message("HighestNearCount", $"From indexes: {debug} MaxNear = {maxNear}");
                return maxNear;
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
                string outist = "";

                if (output != null)
                    for (int i = 0; i < output.Length; i++) 
                    {
                        outist += output[i];

                        if (i < output.Length -1)
                            outist += between;
                    }

                Console.WriteLine(outist);
            }
            public static void ArrayInLiner(int[,] output, string between = " ", string onLineEnd = "\n")
            {
                string outist = "";

                for (int y = 0; y < output.GetLength(1); y++) 
                {
                    for (int x = 0; x < output.GetLength(0); x++)
                    {
                        outist += output[y, x];
                        if (x < output.GetLength(0) - 1)
                            outist += between;
                    }
                    outist += onLineEnd;
                }

                Console.Write(outist);
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
                DebugLog.Success($"Created new CharArray {ToString()}");
            }
            public override string ToString()
            {
                string s = "";
                foreach (char c in pole)
                    s += c;
                return s;
            }
            public string ToVericalString(string between = "")
            {
                string s = "";
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    for (int x = 0; x < pole.GetLength(0); x++)
                    {
                        s += pole[x, y];

                        if (x < pole.GetLength(0) - 1)
                            s += between;
                    }
                    s += "\n";
                }
                return s;
            }
            public int GetLeght(int axis = -1)
            {
                if (axis == -1)
                    return pole.Length;
                else
                    return pole.GetLength(axis);
            }
            public bool Replace(char original, char newOrg)
            {
                bool result = false;
                for (int y = 0; y < pole.GetLength(1); y++) 
                { 
                    for (int x = 0; x < pole.GetLength(0); x++)
                    {
                        if (pole[x, y].Equals(original))
                        {
                            pole[x,y] = newOrg;
                            DebugLog.Message("Replace", $"Charakter {original} replaced with {newOrg} at [{x},{y}]");
                            result = true;
                        }
                        else
                        {
                            DebugLog.Message("Replace", $"On [{x},{y}] {original} != {pole[x, y]}");
                        }
                    }
                }
                return result;
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
        public class LineInts
        {
            int[] ints;
            public LineInts(int[] newInts) 
            { 
                ints = newInts;
            }

            public int[] IndexsOf(int i)
            {
                List<int> result = new List<int>();

                for (int j = 0; j < ints.Length; j++) 
                { 
                    if (ints[j] == i)
                        result.Add(j);
                }

                string debug = "";
                foreach (int r in result)
                    debug += r + " ";

                DebugLog.Message("IndexsOf", $"Searched for: {i} Found indexes: " + debug);

                return result.ToArray();
            }
            public int[] GetDistinct()
            {
                List<int> unique = new List<int>();

                foreach (int i in ints)
                    if (!unique.Contains(i))
                        unique.Add(i);

                string debug = "";
                foreach (int i in unique)
                    debug += i + " ";

                DebugLog.Message("GetDistinct", debug);
                return unique.ToArray();
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