using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ShanheSourceMouStage { OwnChoice, OwnChildren, PeerChoice, PeerChildren, MaximumChildren,
    GiftChoice, GiftRecipient, GiftChildren, Wine, WineChildren, ThresholdChildren, HandDiscardChoice, Finishing }
public sealed record ShanheSourceMouCard(int Id, CardKind Kind, Suit PrintedSuit, int Rank, Suit Suit, CardLocation From);
public sealed record ShanheSourceMouReceipt(CardConversionSource Source, string Hash, SkillProgramEffectOp Operation,
    int Turn, int TurnOwner, long ParentId, int Instruction)
{
    public ShanheSourceMouStage Stage { get; init; }
    public bool Paid { get; init; }
    public bool GuanbianRemoved { get; init; }
    public int Cursor { get; init; }
    public int Payer { get; init; }
    public int Recipient { get; init; } = -1;
    public int Required { get; init; }
    public Suit Suit { get; init; }
    public long Before { get; init; }
    public long After { get; init; }
    public long? WineUseId { get; init; }
    public bool WineReturned { get; init; }
    public long ActionId { get; init; }
    public int Actor { get; init; } = -1;
    public int Ordinal { get; init; }
    private IReadOnlyList<int> _participants = [];
    private IReadOnlyList<ShanheSourceMouCard> _cards = [];
    public IReadOnlyList<int> Participants { get => _participants; init => _participants = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<ShanheSourceMouCard> Cards { get => _cards; init => _cards = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public ShanheSourceMouReceipt? ShanheSourceMou { get; init; }
}
public sealed record ShanheSourceMouWine(long ParentId, int Instruction, CardConversionSource Source, string Hash, bool SelfDying);
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public ShanheSourceMouWine? ShanheSourceMouWine { get; init; }
}
public sealed record ShanheSourceMouStartedEvent(long FrameId, CardConversionSource Source, string Hash,
    SkillProgramEffectOp Operation, int Turn, int TurnOwner, long ParentId, int Instruction) : IGameEvent;
public sealed record ShanheSourceMouPaidEvent(long FrameId, int Payer, int CardId, CardLocation From,
    CardKind Kind, Suit PrintedSuit, int Rank, Suit Suit, long Sequence) : IGameEvent;
public sealed record ShanheSourceMouGuanbianLostEvent(long FrameId, int Owner) : IGameEvent;
public sealed record ShanheSourceMouAllocationEvent(long FrameId, int Owner, int Recipient, int Count, Suit Suit, int Turn) : IGameEvent;
public sealed record ShanheSourceMouWineIssuedEvent(long FrameId, long UseId, long ActionId, bool SelfDying) : IGameEvent;
public sealed record ShanheSourceMouWineReturnedEvent(long FrameId, long UseId, long ActionId) : IGameEvent;
public sealed record ShanheSourceMouBlackUseEvent(long ActionId, long OriginFrameId, int Actor, int Turn, int TurnOwner,
    CardKind Kind, Suit Suit, bool Black) : IGameEvent;
public sealed record ShanheSourceMouThresholdEvent(long FrameId, int Owner, int Actor, int Turn, int Ordinal, long ActionId) : IGameEvent;

internal interface IShanheSourceMouHost { SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation); }
internal abstract class ShanheSourceMouDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramFactRequirements FactRequirements => new(
        Triggered: ProgramTrackedFact.WholeActualUseDeclaration | ProgramTrackedFact.NativeLogicalDiscard | ProgramTrackedFact.ActualDiscardRecoveryPhase | ProgramTrackedFact.NativeActionAppearance,
        Activated: ProgramTrackedFact.NativeLogicalDiscard | ProgramTrackedFact.ActualDiscardRecoveryPhase | ProgramTrackedFact.NativeActionAppearance);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(3));
    public override ISkillProgramEffectHandler Handler => new ShanheSourceMouHandler(Op);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => context.State.CanContinueAfterOwnerDeath(context.Frame);
}
internal sealed class ResolveShanheSourceXiongniDescriptor : ShanheSourceMouDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceXiongni; }
internal sealed class ResolveShanheSourceFengshangDescriptor : ShanheSourceMouDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceFengshang; }
internal sealed class ResolveShanheSourceZhibingDescriptor : ShanheSourceMouDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceZhibing; }
internal sealed class ShanheSourceMouHandler(SkillProgramEffectOp operation) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => operation;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheSourceMouHost>().Resolve(frame, operation);
}
