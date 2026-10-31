namespace CardGame.Core;

public sealed partial record ProgramLifecycleTriggerWindowFrame
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public long? HellAppearanceId { get; init; }
}
