using System;
using System.Collections.Generic;
using System.Linq;

namespace MaxSumOfDeepestPathInPyramid
{
    class Program
    {
        static void Main(string[] args)
        {
            /* The priority for this result is to find the deepest path with the maximum sum among the deepest paths. */

            string pyramid = @"215
                               193 124
                               117 237 442
                               218 935 347 235
                               320 804 522 417 345
                               229 601 723 835 133 124
                               248 202 277 433 207 263 257
                               359 464 504 528 516 716 871 182
                               461 441 426 656 863 560 380 171 923
                               381 348 573 533 447 632 387 176 975 449
                               223 711 445 645 245 543 931 532 937 541 444
                               330 131 333 928 377 733 017 778 839 168 197 197
                               131 171 522 137 217 224 291 413 528 520 227 229 928
                               223 626 034 683 839 053 627 310 713 999 629 817 410 121
                               924 622 911 233 325 139 721 218 253 223 107 233 230 124 233";

            var pyramidMatrix = FormattingPyramidIntoMatrixWithZeroValuesMapping(pyramid);
            var matrixOfPathsWithSums = MatrixOfPathsWithSums(pyramidMatrix);
            var path = FindThePathThatGivesMaxSum(matrixOfPathsWithSums);

            if (path != null)
            {
                Console.WriteLine($" Maximum sum: {path.First()} \n");
                DrawThePath(pyramid, path);
            }
            else
            {
                Console.WriteLine("Failed! Check Again!");
            }

        }

        static int[,] FormattingPyramidIntoMatrixWithZeroValuesMapping(string pyramid)
        {
            var formattedPyramid = pyramid.Split("\n").Select(p => p.Trim()).Select(p => p.Split()).ToArray();
            int len = formattedPyramid.Length;
            var pyramidMatrix = new int[len, len];

            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    if (j < formattedPyramid[i].Length && !CheckPrime(int.Parse(formattedPyramid[i][j])))
                    {
                        pyramidMatrix[i, j] = int.Parse(formattedPyramid[i][j]);
                    }
                }
            }
            return pyramidMatrix;
        }

        static List<List<int>>[,] MatrixOfPathsWithSums(int[,] matrix)
        {
            int len = matrix.GetLength(0);
            List<List<int>>[,] matrixOfPathsWithSums = new List<List<int>>[len, len];

            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    matrixOfPathsWithSums[i, j] = new List<List<int>>();
                }
            }

            matrixOfPathsWithSums[0, 0].Add(new List<int> { matrix[0, 0] });
            matrixOfPathsWithSums[0, 0].First().Add(0);

            for (int i = 0; i < len - 1; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    int parent = matrix[i, j], leftChild = matrix[i + 1, j];
                    int rightChild = j == len - 1 ? 0 : matrix[i + 1, j + 1];

                    if (matrixOfPathsWithSums[i, j].Count > 0)
                    {
                        foreach (var OldPath in matrixOfPathsWithSums[i, j])
                        {
                            if (parent != 0 && OldPath.Count > 0)
                            {
                                if (leftChild != 0)
                                {
                                    matrixOfPathsWithSums[i + 1, j].Add(new List<int>(OldPath));
                                    matrixOfPathsWithSums[i + 1, j][matrixOfPathsWithSums[i + 1, j].Count - 1][0] += leftChild;
                                    matrixOfPathsWithSums[i + 1, j][matrixOfPathsWithSums[i + 1, j].Count - 1].Add(j);
                                }

                                if (rightChild != 0)
                                {
                                    matrixOfPathsWithSums[i + 1, j + 1].Add(new List<int>(OldPath));
                                    matrixOfPathsWithSums[i + 1, j + 1][matrixOfPathsWithSums[i + 1, j + 1].Count - 1][0] += rightChild;
                                    matrixOfPathsWithSums[i + 1, j + 1][matrixOfPathsWithSums[i + 1, j + 1].Count - 1].Add(j + 1);
                                }
                            }
                        }
                    }
                }
            }
            return matrixOfPathsWithSums;
        }

        static List<int> FindThePathThatGivesMaxSum(List<List<int>>[,] matrixOfPathsWithSums)
        {
            int len = matrixOfPathsWithSums.GetLength(0), max = 0;
            for (int i = len - 1; i >= 0; i--)
            {
                for (int j = 0; j < len; j++)
                {
                    if (matrixOfPathsWithSums[i, j].Count > 0)
                    {
                        for (int k = 0; k < matrixOfPathsWithSums[i, j].Count; k++)
                        {
                            max = Math.Max(max, matrixOfPathsWithSums[i, j][k][0]);
                        }
                    }
                }

                if (max != 0)
                {
                    break;
                }
            }

            for (int i = len - 1; i >= 0; i--)
            {
                for (int j = 0; j < len; j++)
                {
                    foreach (var path in matrixOfPathsWithSums[i, j])
                    {
                        if (path[0] == max)
                        {
                            return path;
                        }
                    }
                }
            }
            return null;
        }

        static void DrawThePath(string pyramid, List<int> path)
        {
            var formattedPyramid = pyramid.Split("\n").Select(p => p.Trim()).Select(p => p.Split()).ToArray();

            for (int i = 0; i < path.Count - 1; i++)
            {
                formattedPyramid[i][path[i + 1]] += "*";
            }

            Console.WriteLine(" The deepest path that gives maximum sum: \n");
            for (int i = 0; i < formattedPyramid.Length; i++)
            {
                for (int j = 0; j < formattedPyramid[i].Length; j++)
                {
                    Console.Write(" " + formattedPyramid[i][j] + "\t");
                }
                Console.WriteLine();
            }
        }

        static bool CheckPrime(int n)
        {
            if (n <= 1)
            {
                return false;
            }
            else if (n == 2)
            {
                return true;
            }
            else
            {
                for (int i = 2; i < n / 2; i++)
                {
                    if (n % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
