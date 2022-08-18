public static class ArrayTransformExts
{
    
    public static byte[,] Rotate90(this byte[,] obj) 
    {
        var lenX = obj.GetLength(0);
        var lenY = obj.GetLength(1);
        int indX = lenX-1;
        var res = new byte[lenY,lenX];
        for (int x = 0; x < lenY; x++)
        for (int y = 0; y < lenX; y++)
            res[x, y] = obj[indX - y, x];

        return res;
    }
	
    public static byte[,] Flip(this byte[,] obj)
    {
        var lenX = obj.GetLength(0);
        var lenY = obj.GetLength(1);
        int indX = lenX-1;
		
        var res = new byte[lenX, lenY];

        for (int x = 0; x < lenX; x++)
        for (int y = 0; y < lenY; y++)
            res[x, y] = obj[indX-x, y];
				
        return res;
    }

}