public static class Pentonimos
{

    private static byte m = SquareValues.Filled;
    private static byte o = SquareValues.Empty;
    public static byte[][,] GetPentonimos() => new []{
        new[,]
        {
            {o,m,m},
            {m,m,o},
            {o,m,o}
        },

        new[,]
        {
            {m},
            {m},
            {m},
            {m},
            {m}
        },

        new[,]
        {
            {m,o},
            {m,o},
            {m,o},
            {m,m}
        },

        new[,]
        {
            {m,m},
            {m,m},
            {m,o}
        },

        new[,]
        {
            {o,m},
            {m,m},
            {m,o},
            {m,o}
        },

        new[,]
        {
            {m,m,m},
            {o,m,o},
            {o,m,o}
        },

        new[,]
        {
            {m,o,m},
            {m,m,m}
        },

        new[,]
        {
            {m,o,o},
            {m,o,o},
            {m,m,m}
        },

        new[,]
        {
            {m,o,o},
            {m,m,o},
            {o,m,m}
        },

        new[,]
        {
            {o,m,o},
            {m,m,m},
            {o,m,o}
        },

        new[,]
        {
            {o,m},
            {m,m},
            {o,m},
            {o,m}
        },

        new[,]
        {
            {o,m,m},
            {o,m,o},
            {m,m,o}
        }
    };
}
