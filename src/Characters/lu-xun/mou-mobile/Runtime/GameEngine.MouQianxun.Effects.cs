using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record NativeTrickTargetEffectReturn(long UseId, int TargetSeat, int TargetIndex,
    GroupCardEffect? GroupEffect = null, NullificationWindowFrame? IronChainReturn = null,
    long? WenheProgramFrameId = null, int HpAmount = 0);
public sealed partial record CardEffectBeforeApplyFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeTrickTargetEffectReturn? NativeTargetReturn { get; init; }
}
public sealed record NativeIronChainTargetProgress
{
    private readonly IReadOnlyList<string> _changes = Array.AsReadOnly(Array.Empty<string>());
    public IReadOnlyList<string> Changes { get => _changes; init => _changes = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public NativeIronChainTargetProgress? NativeIronChainTargets { get; init; }
}
