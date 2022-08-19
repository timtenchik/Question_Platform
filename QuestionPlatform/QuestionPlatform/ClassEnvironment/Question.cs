namespace QuestionPlatform.ClassEnvironment;

[Serializable]
internal class Question
{
    public string? Name { get; set; }
    public string? Answer { get; set; }
    public List<string>? AnswerOptions { get; set; }
    public bool HasAnswerOptions { get; set; }
}
