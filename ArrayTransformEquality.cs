public class ArrayTransformEquality : IEqualityComparer<byte[,]>
{
    private readonly ArrayEquality _equality;

    public ArrayTransformEquality(ArrayEquality equality)
    {
        _equality = equality;
    }

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

        if (_equality.Equals(a, b)) return true;
        a = a.Rotate90();
        if (_equality.Equals(a, b)) return true;
        a = a.Rotate90();
        if (_equality.Equals(a, b)) return true;
        a = a.Rotate90();
        if (_equality.Equals(a, b)) return true;
        a = a.Flip();
        if (_equality.Equals(a, b)) return true;
        a = a.Rotate90();
        if (_equality.Equals(a, b)) return true;
        a = a.Rotate90();
        if (_equality.Equals(a, b)) return true;
        a = a.Rotate90();
        if (_equality.Equals(a, b)) return true;
        return false;
    }

    public int GetHashCode(byte[,] obj)
    {
        throw new NotImplementedException();
    }
}