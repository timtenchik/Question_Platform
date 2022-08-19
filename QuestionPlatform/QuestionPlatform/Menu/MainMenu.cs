using QuestionPlatform.ClassEnvironment;
using QuestionPlatform.UserIntervace;

namespace QuestionPlatform.Menu;

internal class MainMenu
{
    public void StartMenu()
    {
        var message = "Welcome to the applications for creating and passing tests!\n" +
                      "Use the ↑ ↓ arrows to navigate through the menu.\n" +
                      "Use Enter to select choice and Escape to exit(or appropriate сhoice)\n" +
                      "Login as:";
        var selectMenu = new string[] { "Manager", "User" };
        while (true)
        {
            var menuSettings = new MenuDrawing(message, selectMenu);
            var selectedResult = menuSettings.ShowSelectMenu();
            switch (selectedResult)
            {
                case 0:
                    QuizMenu();
                    break;
                case 1:
                    var userMenu = new UserMenu();
                    userMenu.MenuUser();
                    break;
                default:
                    return;
            }
        }
    }
    public void QuizMenu()
    {
        var selectMenu = new string[] {
                                        "Add new test",
                                        "Edit available test",
                                        "Back"
                                      };
        while (true)
        {
            var menuSettings = new MenuDrawing(selectMenu);
            var selectedResult = menuSettings.ShowSelectMenu();
            switch (selectedResult)
            {
                case 0:
                    AddQuizName();
                    break;
                case 1:
                    EditAvailableTest();
                    break;
                default:
                    return;
            }
        }
    }
    public void AddQuizName()
    {
        var errorMenu = new ErrorMenu();
        var message = "Enter test name(max 50 symbols):";
        var quizName = errorMenu.CheckStringForError(message, 50);
        MainQuizMenu(quizName);
    }
    public void MainQuizMenu(string quizName)
    {
        var updateData = new UpdateData();
        updateData.GetManagerDatas();
        updateData?.AddNewQuiz(quizName);
        updateData?.SaveManagerDatas();
        var message = $"\t[ {quizName} ]";
        var selectMenu = new string[] {
                                        "Add new question",
                                        "Edit available question",
                                        "Show statistics",
                                        "Delete this test",
                                        "Back"
                                      };
        while (true)
        {
            var menuSettings = new MenuDrawing(message, selectMenu);
            var selectedResult = menuSettings.ShowSelectMenu();
            switch (selectedResult)
            {
                case 0:
                    AddQuestionsName(quizName);
                    break;
                case 1:
                    EditAvailableQuestions(quizName);
                    break;
                case 2:
                    break;
                case 3:
                    var quiz = updateData?.quizzes?.Quizzes?.FirstOrDefault() ?? new Quiz();
                    updateData?.DeleteQuiz(quiz);
                    updateData?.SaveManagerDatas();
                    return;
                default:
                    return;
            }
        }
    }
    public void BackQuizMenu(string quizName)
    {
        var updateData = new UpdateData();
        updateData.GetManagerDatas();
        var message = $"\t[ {quizName} ]";
        var selectMenu = new string[] {
                                        "Add new question",
                                        "Edit available question",
                                        "Show statistics",
                                        "Delete this test",
                                        "Back"
                                      };
        while (true)
        {
            var menuSettings = new MenuDrawing(message, selectMenu);
            var selectedResult = menuSettings.ShowSelectMenu();
            switch (selectedResult)
            {
                case 0:
                    AddQuestionsName(quizName);
                    break;
                case 1:
                    EditAvailableQuestions(quizName);
                    break;
                case 2:
                    ShowStatistic(quizName);
                    break;
                case 3:
                    var quiz = updateData?.quizzes?.Quizzes?.FirstOrDefault() ?? new Quiz();
                    updateData?.DeleteQuiz(quiz);
                    updateData?.SaveManagerDatas();
                    return;
                default:
                    return;
            }
        }
    }
    public void ShowStatistic(string quizName)
    {
        var updataUserData = new UpdateData();
        updataUserData.GetManagerDatas();
        updataUserData.GetUserDatas();
        if (updataUserData.users is null)
        {
            Console.WriteLine("Anybody has not complited tests");
            return;
        }
        Console.WriteLine($"Rating {updataUserData?.users?.Name} is {updataUserData?.users?.PassedTest?.Select(x => x.Rating).FirstOrDefault()} out of {updataUserData?.quizzes?.Quizzes?.Where(x => x.Name == quizName).FirstOrDefault()?.Questions?.Count}");
        Console.WriteLine("Enter any key");
        _ = Console.ReadLine();
    }
    public void AddQuestionsName(string quizName)
    {
        var errorMenu = new ErrorMenu();
        var message = "Write new question(max 100 symbols):";
        var questionName = errorMenu.CheckAddQuestion(message, 100, quizName);
        QuestionMenu(quizName, questionName);
    }
    public void QuestionMenu(string quizName, string? questionName)
    {
        var updateData = new UpdateData();
        updateData.GetManagerDatas();
        updateData.AddNewQuestion(quizName, questionName);
        updateData?.SaveManagerDatas();
        var messege = "You can add answer variant (A, B, C) or " +
                      "leave an empty field to Enter an answer (number or word)";
        var selectMenu = new string[] {
                                        "Add answer variant",
                                        "Leave an empty field to enter the answer",
                                        "Back"
                                      };
        while (true)
        {
            var menuSettings = new MenuDrawing(messege, selectMenu);
            var selectedResult = menuSettings.ShowSelectMenu();
            switch (selectedResult)
            {
                case 0:
                    AddAnswerVariant(quizName, questionName);
                    return;
                case 1:
                    AddAnswerForQuestion(quizName, questionName);
                    return;
                default:
                    return;
            }
        }
    }
    private void AddAnswerVariant(string quizName, string? questionName)
    {
        var selectMenu = new string[] {
                                        "Add new variant",
                                        "Finish"
                                      };
        var errorMenu = new ErrorMenu();
        var menuDrawing = new MenuDrawing(selectMenu);
        var userAnswer = new List<string>();
        var iterator = 0;
        do
        {
            Console.CursorVisible = true;
            var symbol = (char)('a' + iterator);
            Console.Write($"{symbol}: ");
            var answer = Console.ReadLine();
            if (errorMenu.CheckForRightVariant(answer, userAnswer))
            {
                userAnswer.Add(answer ?? string.Empty);
                var variantString = menuDrawing.DrawVariant(userAnswer);
                var newMenuDrawing = new MenuDrawing(variantString, selectMenu);
                var choice = newMenuDrawing.ShowSelectMenu();
                switch (choice)
                {
                    case 0:
                        break;
                    default:
                        var updateData = new UpdateData();
                        updateData.GetManagerDatas();
                        updateData.AddVariantsToQuestion(quizName, questionName, userAnswer);
                        updateData.SaveManagerDatas();
                        AddAnswerForQuestion(quizName, questionName);
                        return;
                }
                iterator++;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("This variant already exist or you write error variant!");
                Console.WriteLine("Write new variant.");
            }
        } while (true);
    }
    public void AddAnswerForQuestion(string quizName, string? questionName)
    {
        var errorMenu = new ErrorMenu();
        var message = "Enter answer for this question(max 50 symbols):";
        var answer = errorMenu.CheckAddAnswer(message, 50);
        var updateData = new UpdateData();
        updateData.GetManagerDatas();
        updateData.AddAnswerToQuestion(quizName, questionName, answer);
        updateData.SaveManagerDatas();
    }
    public void EditAvailableQuestions(string quizName)
    {
        var updateData = new UpdateData();
        updateData.GetManagerDatas();
        var messege = "This is a list of available questions";
        var selectMenu = updateData.GetAvailableQuestions(quizName);
        if (selectMenu?.Length is null || selectMenu.Length == 0)
            messege += "\nYou have not questions in this test";
        var menuSettings = new MenuDrawing(messege, selectMenu);
        var selectedResult = menuSettings.ShowSelectMenu();
        if (selectedResult == -1)
            return;
        var nameSelectedQuestion = selectMenu?[selectedResult];
        var question = updateData?.quizzes?.Quizzes?.Where(x => x.Name == quizName).FirstOrDefault()?.Questions?.Where(x => x.Name == nameSelectedQuestion).FirstOrDefault();
        EditQuestionMenu(question, quizName);
    }
    public void EditQuestionMenu(Question? question, string quizName)
    {
        var updateData = new UpdateData();
        updateData.GetManagerDatas();
        var messege = "This state you can change!";
        var selectMenu = new string[] {
                                        "Change question",
                                        "Change answer",
                                        "Delete this question",
                                        "Back"
                                      };
        while (true)
        {
            var menuSettings = new MenuDrawing(messege, selectMenu);
            var selectedResult = menuSettings.ShowSelectMenu();
            switch (selectedResult)
            {
                case 0:
                    var name = ChangeQuestionsName(question);
                    var quest = updateData?.quizzes?.Quizzes?.Where(x => x.Name == quizName).FirstOrDefault()?.Questions?.Where(x => x.Name == question?.Name).FirstOrDefault();
                    if (quest is not null)
                        quest.Name = name;
                    updateData?.SaveManagerDatas();
                    return;
                case 1:
                    var answer = ChangeQuestionsAnswer();
                    var quest1 = updateData?.quizzes?.Quizzes?.Where(x => x.Name == quizName).FirstOrDefault()?.Questions?.Where(x => x.Name == question?.Name).FirstOrDefault();
                    if (quest1 is not null)
                        quest1.Answer = answer;
                    updateData?.SaveManagerDatas();
                    return;
                case 2:
                    var quest2 = updateData?.quizzes?.Quizzes?.Where(x => x.Name == quizName).FirstOrDefault()?.Questions?.Where(x => x.Name == question?.Name).FirstOrDefault() ?? new Question();
                    updateData?.DeleteQuestion(quizName, quest2);
                    updateData?.SaveManagerDatas();
                    return;
                default:
                    return;
            }
        }
    }
    public string ChangeQuestionsName(Question? question)
    {
        var name = question?.Name ?? string.Empty;
        var errorMenu = new ErrorMenu();
        var message = "Write new question(max 100 symbols):";
        var questionName = errorMenu.CheckAddQuestion(message, 100, name);
        return questionName ?? string.Empty;
    }
    public string ChangeQuestionsAnswer()
    {
        var errorMenu = new ErrorMenu();
        var message = "Write new answer(max 50 symbols):";
        var answer = errorMenu.CheckStringForError(message, 50);
        return answer;
    }
    public void EditAvailableTest()
    {
        var updateData = new UpdateData();
        updateData.GetManagerDatas();
        var messege = "This is a list of available test";
        var selectMenu = updateData.GetAvailableQuiz();
        if (selectMenu?.Length is null || selectMenu.Length == 0)
            messege += "\nYou have not test. You need to make new test";
        var menuSettings = new MenuDrawing(messege, selectMenu);
        var selectedResult = menuSettings.ShowSelectMenu();
        if (selectedResult == -1)
            return;
        var nameSelectedQuiz = selectMenu?[selectedResult];
        //var question = updateData?.quizzes?.Quizzes?.Where(x => x.Name == nameSelectedQuiz).FirstOrDefault();
        if (nameSelectedQuiz is not null)
            BackQuizMenu(nameSelectedQuiz);
    }
}
