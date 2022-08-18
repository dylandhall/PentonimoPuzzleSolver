public class PentonimoWithTransforms
{

    public PentonimoWithTransforms(byte[,] obj)
    {
        var eq = new ArrayEquality();
        var x = new List<byte[,]>();

        x.Add(obj);

        var toAdd = obj.Rotate90();
        if (!x.Any(a => eq.Equals(a, toAdd))) x.Add(toAdd);

        toAdd = x.Last().Rotate90();
        if (!x.Any(a => eq.Equals(a, toAdd))) x.Add(toAdd);

        toAdd = x.Last().Rotate90();
        if (!x.Any(a => eq.Equals(a, toAdd))) x.Add(toAdd);

        toAdd = obj.Flip();
        if (!x.Any(a => eq.Equals(a, toAdd))) x.Add(toAdd);

        toAdd = x.Last().Rotate90();
        if (!x.Any(a => eq.Equals(a, toAdd))) x.Add(toAdd);

        toAdd = x.Last().Rotate90();
        if (!x.Any(a => eq.Equals(a, toAdd))) x.Add(toAdd);

        toAdd = x.Last().Rotate90();
        if (!x.Any(a => eq.Equals(a, toAdd))) x.Add(toAdd);

        _transforms = x.ToArray();
    }

    private static readonly int[][] AdjOffsets = { new [] { -1, 0 }, new [] { 1, 0 }, new [] { 0, -1 }, new [] { 0, 1} };
	
    public byte[][,] GetAllAtOffset(int offsetX, int offsetY, int gridX, int gridY)
    {
        var res = new List<byte[,]>();
        foreach (var t in _transforms)
        {
            var grid = new byte[gridX, gridY];
			
            if (t.GetLength(0)+offsetX>grid.GetLength(0)) continue;
            if (t.GetLength(1)+offsetY>grid.GetLength(1)) continue;

            for (int x = 0; x < t.GetLength(0); x++)
            {
                for (int y = 0; y < t.GetLength(1); y++)
                {
                    if (t[x, y] != SquareValues.Filled) continue;

                    grid[offsetX + x, offsetY + y] = SquareValues.Filled;
                    foreach (var aOffset in AdjOffsets)
                    {
                        int aX = offsetX + x + aOffset[0];
                        int aY = offsetY + y + aOffset[1];
                        if (aX < 0 || aX >= gridX) continue;
                        if (aY < 0 || aY >= gridY) continue;
                        if (grid[aX, aY] != SquareValues.Empty) continue;
                        grid[aX, aY] = SquareValues.Adjacent;
                    }
                }
            }
            res.Add(grid);
        }
        return res.ToArray();
    }

    private readonly byte[][,] _transforms;

}