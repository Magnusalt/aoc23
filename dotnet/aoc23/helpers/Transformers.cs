public static class Transformers
{
    public static char[][] CreateCharMatrix(string[] current)
    {
        var nbrOfRows = current.Length;
        var result = new char[nbrOfRows][];
        for (int y = 0; y < nbrOfRows; y++)
        {
            result[y] = current[y].ToCharArray();
        }
        return result;
    }
}