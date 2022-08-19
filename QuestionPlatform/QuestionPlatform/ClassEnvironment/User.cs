namespace QuestionPlatform.ClassEnvironment;

[Serializable]
internal class User
{
    public string? Name { get; set; }
    public List<Test>? PassedTest { get; set; }
}
