namespace CardGame.Core;
public sealed partial record CardUseFrame
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public bool TurnRedSlashSkipsDodge { get; init; }
}

public sealed partial class GameEngine
{



}
