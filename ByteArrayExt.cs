public static class ByteArrayExt
{
    public static byte[,] CopyOf(this byte[,] obj)
    {
        var lenX = obj.GetLength(0);
        var lenY = obj.GetLength(1);
		
        var res = new byte[lenX, lenY];

        for (int x = 0; x < lenX; x++)
        for (int y = 0; y < lenY; y++)
            res[x, y] = obj[x, y];

        return res;
    }


    public static string[,] ToRender(this byte[,] obj)
    {
        var lenX = obj.GetLength(0);
        var lenY = obj.GetLength(1);
        var res = new string[lenX,lenY];
        for (int x = 0; x < lenX; x++)
        for (int y = 0; y < lenY; y++)
            res[x, y] = (obj[x, y]==10? "▓▓":"  ");
			
        return res;
    }


    public static bool IsCompatible(this byte[,] grid, byte[,] update)
    {
        if (grid.GetLength(0) != update.GetLength(0) || grid.GetLength(0) != update.GetLength(0)) return false;

        for (int x = 0; x < grid.GetLength(0); x++)
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            byte newval = (byte)(grid[x, y] + update[x, y]);
            if (newval > SquareValues.Filled) return false;
        }

        return true;
    }

    public static bool AddObject(this byte[,] grid, byte[,] update)
    {
        if (grid.GetLength(0)!=update.GetLength(0)||grid.GetLength(0)!=update.GetLength(0)) return false;
		
        for (int x = 0; x < grid.GetLength(0); x++)
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            byte newval = (byte)(grid[x, y] + update[x, y]);
            if (newval > SquareValues.Filled) return false;
            grid[x, y] = newval;
        }

        return true;
    }

}