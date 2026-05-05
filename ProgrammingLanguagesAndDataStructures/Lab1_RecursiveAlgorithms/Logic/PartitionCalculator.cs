using System.Collections.Generic;
using System;

namespace Lab1_RecursiveAlgorithms.Logic
{
    public static class PartitionCalculator
    {
        public static List<string> GetPartitions(int n)
        {
            List<string> results = new List<string>();
            FindPartitions(n, n - 1, new List<int>(), results);
            return results;
        }

        private static void FindPartitions(int remaining, int maxVal, List<int> current, List<string> results)
        {
            if (remaining == 0 && current.Count >= 2)
            {
                results.Add(string.Join(" + ", current));
            }
            else
            {
                int start = Math.Min(remaining, maxVal);
                for (int i = start; i >= 1; i--)
                {
                    current.Add(i);
                    FindPartitions(remaining - i, i, current, results);
                    current.RemoveAt(current.Count - 1);
                }
            }
        }
    }
}