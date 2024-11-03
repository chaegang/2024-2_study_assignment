using System;
using System.Linq;

namespace statistics
{
    class Program
    {
        static void Main(string[] args)
        {
            string[,] data = {
                {"StdNum", "Name", "Math", "Science", "English"},
                {"1001", "Alice", "85", "90", "78"},
                {"1002", "Bob", "92", "88", "84"},
                {"1003", "Charlie", "79", "85", "88"},
                {"1004", "David", "94", "76", "92"},
                {"1005", "Eve", "72", "95", "89"}
            };
            // You can convert string to double by
            // double.Parse(str)

            int stdCount = data.GetLength(0) - 1;
            
            double[] math = new double[stdCount];
            double[] science = new double[stdCount];
            double[] english = new double[stdCount];
            double[] total = new double[stdCount];
            string[] name = new string[stdCount];

            for (int i = 1; i <= stdCount; i++)
            {
                math[i - 1] = double.Parse(data[i, 2]);
                science[i - 1] = double.Parse(data[i, 3]);
                english[i - 1] = double.Parse(data[i, 4]);
                total[i - 1] = math[i - 1] + science[i - 1] + english[i - 1];
                name[i - 1] = data[i, 1];
            }

            double avgM = math.Average();
            double MaxM = math.Max();
            double MinM = math.Min();

            double avgS = science.Average();
            double MaxS = science.Max();
            double MinS = science.Min();

            double avgE = english.Average();
            double MaxE = english.Max();
            double MinE = english.Min();

            Console.WriteLine("Average Scores:");
            Console.WriteLine($"Math: {avgM:F2}");
            Console.WriteLine($"Science: {avgS:F2}");
            Console.WriteLine($"English: {avgE:F2}\n");
            Console.WriteLine("Max and min Scores:");
            Console.WriteLine($"Math: ({MaxM}, {MinM})");
            Console.WriteLine($"Science: ({MaxS}, {MinS})");
            Console.WriteLine($"English: ({MaxE}, {MinE})\n");

            var score = new (string Name, double totalScore)[stdCount];

            for (int i = 0; i < stdCount; i++)
            {
                score[i] = (name[i], total[i]);
            }

            Array.Sort(score, (x, y) => y.totalScore.CompareTo(x.totalScore));

            int[] ranks = new int[stdCount];
            for (int i = 0; i < stdCount; i++)
            {
                ranks[Array.IndexOf(name, score[i].Name)] = i + 1;
            }

            Console.WriteLine("Students rank by total scores:");
            for (int i = 0; i < name.Length; i++)
            {
                string end = "th"; 
                if (ranks[i] == 1)
                {
                    end = "st";
                }
                else if (ranks[i] == 2)
                {
                    end = "nd";
                }
                else if (ranks[i] == 3)
                {
                    end = "rd";
                }
                
                Console.WriteLine($"{name[i]}: {ranks[i]}{end}");
            }
        }
    }
}

/* example output

Average Scores: 
Math: 84.40
Science: 86.80
English: 86.20

Max and min Scores: 
Math: (94, 72)
Science: (95, 76)
English: (92, 78)

Students rank by total scores:
Alice: 4th
Bob: 1st
Charlie: 5th
David: 2nd
Eve: 3rd

*/
