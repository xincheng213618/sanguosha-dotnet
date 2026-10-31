namespace CardGame.Core;

public sealed partial class GameEngine
{

}

public sealed partial record CardUseFrame
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public bool GuanFirstRedDistance { get; init; }

}
