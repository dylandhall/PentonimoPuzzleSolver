using System.Collections;
using System.Numerics;

public static class PentonimoWithTransformsExt
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

    public static BigInteger[,,] GenerateCompatibilityArray(this byte[][][,] objGrids, ref BigInteger[] bitValues)
    {
        var numObjs = objGrids.Count();
        int maxPerms = objGrids.Max(a => a.Length);

        var vl = new BigInteger[numObjs, maxPerms, numObjs];

        for (int pentObject = 0; pentObject < numObjs; pentObject++)
        {
            for (int grid = 0; grid < objGrids[pentObject].Length; grid++)
            {
                for (int otherObject = pentObject + 1; otherObject < numObjs; otherObject++)
                {
                    //bool[] tmp = new bool[objGrids[otherObject].Length];
                    BigInteger tmp = new BigInteger();
                    for (int otherGrid = 0; otherGrid < objGrids[otherObject].Length; otherGrid++)
                        if (objGrids[pentObject][grid].IsCompatible(objGrids[otherObject][otherGrid]))
                            tmp += bitValues[otherGrid];
                        //tmp[otherGrid] = objGrids[pentObject][grid].IsCompatible(objGrids[otherObject][otherGrid]);

                    vl[pentObject, grid, otherObject] = tmp;
                }
            }
        }

        return vl;
    }
}