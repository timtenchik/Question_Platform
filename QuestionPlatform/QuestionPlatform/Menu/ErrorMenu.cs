namespace QuestionPlatform.Menu;

internal class ErrorMenu
{
    public string CheckAddAnswer(string messege, int maxSize)
    {
        Console.CursorVisible = true;
        Console.WriteLine(messege);
        string answerName;
        do
        {
            answerName = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(answerName) || answerName.Length > maxSize)
            {
                Console.Clear();
                Console.WriteLine("Please repeat");
                Console.WriteLine(messege);
            }
        } while (string.IsNullOrWhiteSpace(answerName) || answerName.Length > maxSize);
        return answerName;
    }
    public string CheckStringForError(string messege, int maxSize)
    {
        Console.CursorVisible = true;
        Console.WriteLine(messege);
        string? quizName;
        do
        {
            quizName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(quizName) || quizName.Length > maxSize)
            {
                Console.Clear();
                Console.WriteLine("Please repeat");
                Console.WriteLine(messege);
            }
            if (CheckExistQuiz(quizName))
            {
                Console.Clear();
                Console.WriteLine("This quiz already exist");
                Console.WriteLine("Please repeat");
                Console.WriteLine(messege);
            }
        } while (string.IsNullOrWhiteSpace(quizName) || quizName.Length > maxSize || CheckExistQuiz(quizName));
        return quizName;
    }
    public string? CheckAddQuestion(string messege, int maxSize, string quizName)
    {
        Console.CursorVisible = true;
        Console.WriteLine(messege);
        string? questionName;
        do
        {
            questionName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(questionName) || questionName.Length > maxSize)
            {
                Console.Clear();
                Console.WriteLine("Please repeat");
                Console.WriteLine(messege);
            }
            if (CheckExistQuestion(quizName, questionName))
            {
                Console.Clear();
                Console.WriteLine("This question already exist");
                Console.WriteLine("Please repeat");
                Console.WriteLine(messege);
            }
        } while (string.IsNullOrWhiteSpace(questionName) || questionName.Length > maxSize || CheckExistQuestion(quizName, questionName));
        return questionName;
    }
    private bool CheckExistQuiz(string? quizName)
    {
        var updateData = new UpdateData();
        updateData.GetManagerDatas();
        var repeated = updateData?.quizzes?.Quizzes?.Any(x => x.Name == quizName) ?? false;
        return repeated;
    }
    private bool CheckExistQuestion(string? quizName, string? questionName)
    {
        var updateData = new UpdateData();
        updateData.GetManagerDatas();
        var quiz = updateData?.quizzes?.Quizzes?.Where(x => x.Name == quizName).FirstOrDefault();
        var repeated = quiz?.Questions?.Any(x => x.Name == questionName) ?? false;
        return repeated;
    }
    public bool CheckForRightVariant(string? answer, List<string> variantList)
    {
        return !string.IsNullOrWhiteSpace(answer) && variantList is not null && !variantList.Any(x => x == answer);
    }
}
