public class ArrayEquality : IEqualityComparer<byte[,]>
{
    public bool Equals(byte[,]? x, byte[,]? y)
    {
        if (x == null || y == null) return false;
        return Eq(x, y);
    }

    private bool Eq(byte[,] a, byte[,] b)
    {
        int lenX = a.GetLength(0);
        int lenY = a.GetLength(1);
        if (lenX!=b.GetLength(0)||lenY!=b.GetLength(1)) return false;
		
        for (int i = 0; i < lenX; i++)
        for (int j = 0; j < lenY; j++)
            if (a[i, j] != b[i, j]) return false;
        return true;
    }

    public int GetHashCode(byte[,] obj)
    {
        throw new NotImplementedException();
    }
}