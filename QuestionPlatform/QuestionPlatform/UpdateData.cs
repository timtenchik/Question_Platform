using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using QuestionPlatform.ClassEnvironment;

namespace QuestionPlatform;

internal class UpdateData
{
    public AllQuizzes? quizzes;
    public User? users;
    private readonly string _pathToManageJson = Path.GetFullPath(@"..\..\..\Hash\manager_hash.json");
    private readonly string _pathToUserJson = Path.GetFullPath(@"..\..\..\Hash\users_hash.json");
    public void GetUserDatas()
    {
        var json = File.ReadAllText(_pathToUserJson);
        if (!string.IsNullOrWhiteSpace(json))
        {
            var deserializeManageData = JsonSerializer.Deserialize<User>(json);
            if (deserializeManageData is null)
                throw new ArgumentException();
            users = deserializeManageData;
        }
    }
    public void SaveUserDatas()
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(users, options);
        File.WriteAllText(_pathToUserJson, json);
    }
    public void GetManagerDatas()
    {
        var json = File.ReadAllText(_pathToManageJson);
        if (!string.IsNullOrWhiteSpace(json))
        {
            var deserializeManageData = JsonSerializer.Deserialize<AllQuizzes>(json);
            if (deserializeManageData is null)
                throw new ArgumentException();
            quizzes = deserializeManageData;
        }
    }
    public void SaveManagerDatas()
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(quizzes, options);
        File.WriteAllText(_pathToManageJson, json);
    }
    public void AddNewQuiz(string quizName)
    {
        var quizItem = new Quiz
        {
            Name = quizName,
            Questions = null
        };
        if (quizzes is null)
        {
            var allQuizzes = new AllQuizzes
            {
                Quizzes = new List<Quiz>()
            };
            allQuizzes.Quizzes.Add(quizItem);
            quizzes = allQuizzes;
        }
        else
        {
            quizzes?.Quizzes?.Add(quizItem);
        }
    }
    public void AddNewQuestion(string quizName, string? questionName)
    {
        var questionItem = new Question
        {
            Name = questionName
        };
        var quiz = quizzes?.Quizzes?.Where(x => x.Name == quizName).FirstOrDefault();
        if (quiz is not null && quiz.Questions is null)
        {
            var questions = new List<Question>
            {
                questionItem
            };
            quiz.Questions = questions;
        }
        else
        {
            quiz?.Questions?.Add(questionItem);
        }
    }
    public void AddVariantsToQuestion(string quizName, string? questionName, List<string> variants)
    {
        var quiz = quizzes?.Quizzes?.Where(x => x.Name == quizName)?.FirstOrDefault();
        var question = quiz?.Questions?.Where(x => x.Name == questionName).FirstOrDefault();
        if (question is not null && question.AnswerOptions is null)
        {
            question.HasAnswerOptions = true;
            question.AnswerOptions = variants;
        }
    }

    internal void AddAnswerToQuestion(string quizName, string? questionName, string answer)
    {
        var quiz = quizzes?.Quizzes?.Where(x => x.Name == quizName)?.FirstOrDefault();
        var question = quiz?.Questions?.Where(x => x.Name == questionName).FirstOrDefault();
        if (question is not null && question.Answer is null)
            question.Answer = answer;
    }
    public string?[]? GetAvailableQuestions(string quizName)
    {
        GetManagerDatas();
        var quiz = quizzes?.Quizzes?.Where(x => x.Name == quizName)?.FirstOrDefault();
        var questions = quiz?.Questions?.Select(x => x.Name)?.ToArray();
        return questions;
    }
    public string[]? GetAvailableAnswer(Question question)
    {
        var answers = question?.AnswerOptions?.ToArray();
        return answers;
    }
    public string?[]? GetAvailableQuiz()
    {
        GetManagerDatas();
        var quiz = quizzes?.Quizzes?.Select(x => x.Name)?.ToArray();
        return quiz;
    }
    public void DeleteQuestion(string quizName, Question question)
    {
        var removed = quizzes?.Quizzes?.Where(x => x.Name == quizName).FirstOrDefault()?.Questions?.Remove(question);
        var listQuestion = quizzes?.Quizzes?.Where(x => x.Name == quizName).FirstOrDefault()?.Questions;
        if (listQuestion?.Count == 0)
        {

        }
    }
    public void DeleteQuiz(Quiz quiz)
    {
        _ = (quizzes?.Quizzes?.Remove(quiz));
    }
}


