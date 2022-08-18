using System.Collections;

public static class BitArrayExt
{
    
    public static bool AllFalse(this BitArray ba) {
        for (int i = 0; i < ba.Length; i++)
        {
            if (ba[i]) return false;
        }
        return true;
    }
}