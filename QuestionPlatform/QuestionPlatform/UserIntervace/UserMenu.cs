using QuestionPlatform.ClassEnvironment;
using QuestionPlatform.Menu;

namespace QuestionPlatform.UserIntervace;

internal class UserMenu
{
    public void MenuUser()
    {
        var menuError = new ErrorMenu();
        var surname = menuError.CheckAddAnswer("Your name and surname: ", 50);
        var updateData = new UpdateData();
        updateData.GetManagerDatas();
        var messege = "This is a list of available test";
        var selectMenu = updateData.GetAvailableQuiz();
        if (selectMenu?.Length is null || selectMenu.Length == 0)
            messege += "\nYou have no available test";
        var menuSettings = new MenuDrawing(messege, selectMenu);
        var selectedResult = menuSettings.ShowSelectMenu();
        if (selectedResult == -1)
            return;
        var nameSelectedQuiz = selectMenu?[selectedResult];
        var quiz = updateData?.quizzes?.Quizzes?.Where(x => x.Name == nameSelectedQuiz).FirstOrDefault();
        var i = 0;
        var answers = new List<string?>();
        var rating = 0;
        if (quiz is not null && quiz?.Questions is not null)
        {
            foreach (var question in quiz.Questions)
            {
                if (question.HasAnswerOptions)
                {
                    var updateDataUser = new UpdateData();
                    var messegeUserAnswer = question.Name;
                    var selectMenuUserAnswer = updateDataUser.GetAvailableAnswer(question);
                    var menuSettingsUserAnswer = new MenuDrawing(messegeUserAnswer, selectMenuUserAnswer);
                    var selectedResultUserAnswer = menuSettingsUserAnswer.ShowSelectMenu();
                    var nameSeleted = selectMenuUserAnswer?[selectedResultUserAnswer];
                    answers.Add(nameSeleted);
                    if (question.Answer == nameSeleted)
                    {
                        rating++;
                    }
                }
                else
                {
                    Console.Clear();
                    if (question.Name is not null && question.Answer is not null)
                    {
                        var answer = menuError.CheckAddAnswer(question.Name, 100);
                        i++;
                        answers.Add(answer);
                        if (question.Answer == answer)
                        {
                            rating++;
                        }
                    }
                }
            }
            Console.WriteLine($"Your rating is {rating} out of {quiz.Questions.Count}");
        }
        var test = new Test
        {
            Answers = answers,
            Rating = rating
        };
        var listTest = new List<Test>
        {
            test
        };
        var user = new User
        {
            Name = surname,
            PassedTest = listTest
        };
        var updateDataUserSave = new UpdateData
        {
            users = user
        };
        updateDataUserSave.SaveUserDatas();
        Console.WriteLine($"\nPress any key for exit");
        var key = Console.ReadKey().Key;
    }
}
