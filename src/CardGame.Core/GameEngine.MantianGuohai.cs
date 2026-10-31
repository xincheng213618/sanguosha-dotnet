using System.Text.Json.Serialization;

namespace CardGame.Core;

internal static class MantianGuohaiGiftContract
{
    internal static bool Matches(IReadOnlyList<SkillProgramEffect> effects) => effects is
        [{ Op: SkillProgramEffectOp.SelectOwnedCards, Target: SkillProgramEffectTarget.Owner, Amount: 1,
            ResultBind: "given", Zones: [CardZoneKind.Hand], MinimumCards: 0, MaximumCards: 0,
            Condition.Kind: SkillProgramConditionKind.Always },
         { Op: SkillProgramEffectOp.MoveBoundCards, Target: SkillProgramEffectTarget.Owner, SourceBind: "given",
            ExceptBind: null, Destination: SkillProgramCardDestination.SelectedTargetHand, AwaitMovementTriggers: true,
            Condition.Kind: SkillProgramConditionKind.Always }];
}

public sealed record MantianGuohaiGiftState
{
    private readonly IReadOnlyList<int> _targets = [];
    public IReadOnlyList<int> GiftTargets { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
    public int GiftIndex { get; init; }
    public int EffectRound { get; init; }
    public long? ProgramFrameId { get; init; }
    public bool Complete { get; init; }
}
public sealed record MantianGuohaiGiftReturn(long CardUseFrameId, long ActionId, int GiftIndex, int RecipientSeat);
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MantianGuohaiGiftState? MantianGuohaiGift { get; init; }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MantianGuohaiGiftReturn? MantianGuohaiReturn { get; init; }
}
