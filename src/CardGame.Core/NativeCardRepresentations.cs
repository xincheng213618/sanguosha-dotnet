using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed partial record CardUseFrame
{
    private readonly Card? _nativeRepresentation;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Card? NativeRepresentation
    {
        get => _nativeRepresentation;
        init => _nativeRepresentation = value is null ? null : value with
        { GrantedSkillIds = Array.AsReadOnly(value.GrantedSkillIds.ToArray()) };
    }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool NativeRepresentationIsCarrier { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long NativeRepresentationIssuedAfterMovementSequence { get; init; }
}

internal interface INativeCardRepresentations : IProgramRuntimeOperations
{
    Card Get(long useId, int cardId, bool requireProcessing = false);
    Card ReadAppearance(long useId, CardAppearanceReference appearance);
}
