namespace QuestionPlatform.ClassEnvironment;

[Serializable]
internal class Quiz
{
    public string? Name { get; set; }
    public List<Question>? Questions { get; set; }
}
