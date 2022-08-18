using System.Collections;
using System.Text;

namespace PentonimoSolver;

public static class Helpers
{
    
    public static byte[][][,] GetObjectGrids(this PentonimoWithTransforms[] pentObjs, int xSize, int ySize)
    {
        var objGrids = new List<byte[,]>[pentObjs.Length];

        for (int i = 0; i < pentObjs.Length; i++)
        {
            var poL = new List<byte[,]>();
            for (int x = 0; x < xSize; x++)
            for (int y = 0; y < ySize; y++)
                poL.AddRange(pentObjs[i].GetAllAtOffset(x, y, xSize, ySize));

            objGrids[i] = poL;
        }
        
        return objGrids.Select(g => g.ToArray()).ToArray();
    }

    public static BitArray[,,] GenerateCompatibilityArray(this byte[][][,] objGrids)
    {
        var numObjs = objGrids.Count();
        int maxPerms = objGrids.Max(a => a.Length);
	
        var vl = new BitArray[numObjs, maxPerms, numObjs];

        for (int pentObject = 0; pentObject < numObjs; pentObject++)
        {
            for (int grid = 0; grid < objGrids[pentObject].Length; grid++)
            {
                for (int otherObject = pentObject + 1; otherObject < numObjs; otherObject++)
                {
                    bool[] tmp = new bool[objGrids[otherObject].Length];

                    for (int otherGrid = 0; otherGrid < objGrids[otherObject].Length; otherGrid++)
                        tmp[otherGrid] = objGrids[pentObject][grid].IsCompatible(objGrids[otherObject][otherGrid]);


                    vl[pentObject, grid, otherObject] = new BitArray(tmp);
                }
            }
        }
        return vl;
    }
    
    
    private static bool AllFalse(BitArray ba) {
        for (int i = 0; i < ba.Length; i++)
        {
            if (ba[i]) return false;
        }
        return true;
    }
    
    public static void PrintGrid(this string[,] rArr)
    {

        var stringBuilder = new StringBuilder();
        for (int j = 0; j < rArr.GetLength(0); j++)
        {
            for (int k = 0; k < rArr.GetLength(1); k++)
            {
                stringBuilder.Append(rArr[j, k]);
            }

            stringBuilder.AppendLine();
        }
        
        Console.WriteLine(stringBuilder.ToString());
    }

// compatibility map indexes:
// level/pentomino, grid/permutation index, other pentomino, bitarray index value: compatibility with other pentomino's grid/permutation at this index
    public static byte[,] CheckRoute(ref BitArray[,,] cMap, ref byte[][][,] objGrids, int currentLevel, int gridIndex, BitArray[] currentLevels)
    {
        //Console.WriteLine($"Starting level {currentLevel} index {gridIndex}");

        var nextLevelsLength = objGrids.Length-currentLevel-1;
        
        if (nextLevelsLength == 0)
            return objGrids[currentLevel][gridIndex].CopyOf();
        
        BitArray[] nextLevels;

        nextLevels = new BitArray[nextLevelsLength];
        
        for (int level = 0; level < nextLevelsLength; level++)
        {
            var newArr = new BitArray(currentLevels[level+1]);
            newArr.And(cMap[currentLevel,gridIndex,currentLevel+level+1]);
            if (AllFalse(newArr)) return null!;
            nextLevels[level]=newArr;
        }

        for (int i = 0; i < nextLevels[0].Count; i++)
        {
            if (!nextLevels[0][i]) continue;
            
            var result = CheckRoute(ref cMap, ref objGrids, currentLevel + 1, i, nextLevels);
            if (result == null!) continue;
                
            result.AddObject(objGrids[currentLevel][gridIndex]);
            return result;
        }

        return null!;
    }
    
    
    
    public static void UpdatePercentageAndPrint(int nom, int denom, ref double lastPercentage, TimeSpan timeElapsed)
    {
        var percentage = Math.Round((nom / (double) denom), 5);
        if (percentage > lastPercentage)
        {
            Interlocked.Exchange(ref lastPercentage, percentage);
            Console.WriteLine($"{Math.Round(percentage * 100, 3)}% complete at {timeElapsed}");
        }
    }
}