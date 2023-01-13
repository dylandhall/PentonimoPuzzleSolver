using System.Collections;

public static class PentonimoWithTransformsExt
{
    public static byte[][][,] GetObjectGrids(this PentonimoWithTransforms[] pentObjs, int xSize, int ySize, byte[,]? gridMask = null)
    {
        var objGrids = new List<byte[,]>[pentObjs.Length];

        for (int i = 0; i < pentObjs.Length; i++)
        {
            var poL = new List<byte[,]>();
            for (int x = 0; x < xSize; x++)
            for (int y = 0; y < ySize; y++)
                poL.AddRange(pentObjs[i].GetAllAtOffset(x, y, xSize, ySize));

            
            objGrids[i] = gridMask == null ? poL : poL.Where(gridMask.IsCompatible).ToList();
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
}