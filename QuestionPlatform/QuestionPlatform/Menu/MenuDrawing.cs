namespace QuestionPlatform.Menu;

internal class MenuDrawing
{
    private readonly string?[]? _selectArray;
    private readonly string? _message;
    public MenuDrawing(string? message, string?[]? selectMenu)
    {
        _selectArray = selectMenu;
        _message = message;
    }
    public MenuDrawing(string[] selectMenu)
    {
        _message = string.Empty;
        _selectArray = selectMenu;
    }
    public MenuDrawing()
    {
        _message = string.Empty;
        _selectArray = Array.Empty<string>();
    }
    private void DwawResults(int pointer)
    {
        Console.Clear();
        Console.WriteLine($"{_message}\n");
        for (var k = 0; k < _selectArray?.Length; k++)
        {
            if (k == pointer)
                Console.WriteLine($" ->  {_selectArray[k]}");
            else
                Console.WriteLine($"    {_selectArray[k]}");
        }
    }
    public int ShowSelectMenu()
    {
        var pointer = 0;
        Console.CursorVisible = false;
        ConsoleKey key;
        DwawResults(pointer);
        do
        {
            key = Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    pointer--;
                    break;
                case ConsoleKey.DownArrow:
                    pointer++;
                    break;
                default:
                    break;
            }
            if (pointer > _selectArray?.Length - 1)
                pointer = 0;
            else if (pointer < 0)
                pointer = _selectArray?.Length - 1 ?? 0;
            for (var t = 0; t < _selectArray?.Length; t++)
            {
                if (t == pointer)
                {
                    DwawResults(pointer);
                    if (key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        return pointer;
                    }
                }
            }
        } while (key != ConsoleKey.Escape);
        Console.Clear();
        return -1;
    }
    public string DrawVariant(List<string> variantList)
    {
        var iterator = 0;
        var variantsString = string.Empty;
        foreach (var variant in variantList)
        {
            var symbol = (char)('a' + iterator);
            variantsString += $"{symbol}: {variant}\n";
            iterator++;
        }
        return variantsString;
    }
}
