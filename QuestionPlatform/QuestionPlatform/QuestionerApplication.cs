using QuestionPlatform.Menu;

namespace QuestionPlatform;

internal class QuestionerApplication
{
    public int Run()
    {
        MainMenu menu = new();
        menu.StartMenu();
        return 0;
    }
}
