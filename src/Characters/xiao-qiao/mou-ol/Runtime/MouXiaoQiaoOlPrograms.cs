using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum MouXiaoQiaoOlStage { GiftCards, GiftRecipient, RevealChildren, GiftChildren, EndChoice, HealthChildren,
    UseChoice, NativeIssuing, NativeUse, NativeChildren }
public sealed record MouXiaoQiaoOlGift(int CardId, CardKind PrintedKind, CardLocation From, int RecipientSeat);
public sealed record MouXiaoQiaoOlUsePlan(CardKind Kind, LegalActionKind ActionKind, int? TargetCardId,
    CardKind? RequiredKind, string Description, bool Adjusted = false)
{
    private readonly IReadOnlyList<int> _targets = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Targets { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
}
public sealed record MouXiaoQiaoOlReceipt(CardConversionSource Source, string GameplayHash,
    SkillProgramEffectOp Operation, int Round, long ParentFrameId)
{
    public MouXiaoQiaoOlStage Stage { get; init; }
    public bool Paid { get; init; }
    public int SelectedCard { get; init; }
    public int Cursor { get; init; }
    public int TargetSeat { get; init; } = -1;
    public bool Recover { get; init; }
    public long Before { get; init; }
    public long After { get; init; }
    public CardKind OutputKind { get; init; }
    public Suit ShownSuit { get; init; }
    public bool RedBasic { get; init; }
    public MouXiaoQiaoOlUsePlan? Plan { get; init; }
    public long UseId { get; init; }
    public long ActionId { get; init; }
    public bool NativeReturned { get; init; }
    private readonly IReadOnlyList<MouXiaoQiaoOlGift> _gifts = Array.AsReadOnly(Array.Empty<MouXiaoQiaoOlGift>());
    public IReadOnlyList<MouXiaoQiaoOlGift> Gifts { get => _gifts; init => _gifts = Array.AsReadOnly(value.ToArray()); }
}
public sealed record MouXiaoQiaoOlNativeOrigin(long ProgramFrameId, CardConversionSource Source, string GameplayHash,
    long UseId, long ActionId, int ActorSeat, CardKind Kind);
public sealed record MouXiaoQiaoJueyanResponseRequest(long ParentFrameId, DecisionKind DecisionKind, CardKind OutputKind,
    int ActorSeat, int TargetSeat, long OriginalResponseFrameId,
    SkillProgramEffectOp? RequestOperation=null,int RequestInstruction=0,CardConversionSource? RequestSource=null,
    string? RequestHash=null,string? ResultBind=null)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public ProgramSlashRequestAcceptance? RequestAcceptance { get; init; }
}
public sealed record MouXiaoQiaoJueyanResponseGrant(long ProgramFrameId, CardConversionSource Source, string GameplayHash,
    int Round, int ShownCardId, MouXiaoQiaoJueyanResponseRequest Request, long ActionId = 0,bool Returned=false);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MouXiaoQiaoOlReceipt? MouXiaoQiaoOl { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MouXiaoQiaoJueyanResponseRequest? MouXiaoQiaoJueyanResponse { get; init; }
}
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MouXiaoQiaoOlNativeOrigin? MouXiaoQiaoOlOrigin { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MouXiaoQiaoJueyanResponseGrant? MouXiaoQiaoJueyanRescueOrigin { get; init; }
}
public abstract partial record ResolutionFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MouXiaoQiaoJueyanResponseGrant? MouXiaoQiaoJueyanResponseGrant { get; init; }
}
public sealed record MouXiaoQiaoOlStartedEvent(long FrameId, CardConversionSource Source, string GameplayHash,
    SkillProgramEffectOp Operation, int Round, long ParentFrameId) : IGameEvent;
public sealed record MouXiaoQiaoMiluoAcceptedEvent(long FrameId, int OwnerSeat, int Round, int Count) : IGameEvent;
public sealed record MouXiaoQiaoMiluoGiftEvent(long FrameId, int OwnerSeat, int Round, int Index, int CardId,
    CardKind Kind, int RecipientSeat, CardLocation From, CardLocation To, long Sequence) : IGameEvent;
public sealed record MouXiaoQiaoMiluoConsequenceEvent(long FrameId, int OwnerSeat, int Round, int TargetSeat, bool Recover) : IGameEvent;
public sealed record MouXiaoQiaoJueyanUsedEvent(long FrameId, CardConversionSource Source, string GameplayHash,
    int Round, int CardId, CardKind OutputKind, Suit ShownSuit, bool RedBasic) : IGameEvent;
public sealed record MouXiaoQiaoJueyanNativeIssuedEvent(long FrameId, long UseId, long ActionId, int ActorSeat, CardKind Kind) : IGameEvent;
public sealed record MouXiaoQiaoJueyanNativeReturnedEvent(long FrameId, long UseId, long ActionId) : IGameEvent;
public sealed record MouXiaoQiaoJueyanBatchSignedEvent(long FrameId,long UseId,long BatchId,long? ParentBatchId,int Count):IGameEvent;
public sealed record MouXiaoQiaoJueyanMovementSignedEvent(long FrameId,long UseId,long BatchId,int CardId,CardKind Kind,CardLocation From,CardLocation To,CardMoveReason Reason,long Sequence):IGameEvent;
public sealed record MouXiaoQiaoJueyanResponseGrantedEvent(MouXiaoQiaoJueyanResponseGrant Grant) : IGameEvent;
public sealed record MouXiaoQiaoJueyanResponseIssuedEvent(long ParentFrameId, long ProgramFrameId, long ActionId, int ActorSeat, CardKind Kind) : IGameEvent;
public sealed record MouXiaoQiaoOlCompletedEvent(long FrameId, SkillProgramEffectOp Operation) : IGameEvent;

internal interface IMouXiaoQiaoOlRuntime : IProgramRuntimeOperations
{ SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation); }
internal abstract class MouXiaoQiaoOlDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ISkillProgramEffectHandler Handler => new MouXiaoQiaoOlHandler(Op);
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext c) => c.State.CanContinueAfterOwnerDeath(c.Frame);
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeActionAppearance | ProgramTrackedFact.ActualRound,
        Activated: ProgramTrackedFact.NativeActionAppearance | ProgramTrackedFact.ActualRound);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(2));
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => Op switch
    {
        SkillProgramEffectOp.ResolveMouXiaoQiaoMiluoRoundEnd => [new RequireTriggerWindow(SkillProgramTriggerWindow.RoundEnding)],
        SkillProgramEffectOp.ResolveMouXiaoQiaoJueyan => [new CaptureActivationCards("jueyan-hand"), new RetainOwnedCardSet("jueyan-hand")],
        _ => []
    };
}
internal sealed class ResolveMouXiaoQiaoMiluoDescriptor : MouXiaoQiaoOlDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMouXiaoQiaoMiluo; }
internal sealed class ResolveMouXiaoQiaoMiluoRoundEndDescriptor : MouXiaoQiaoOlDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMouXiaoQiaoMiluoRoundEnd; }
internal sealed class ResolveMouXiaoQiaoJueyanDescriptor : MouXiaoQiaoOlDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMouXiaoQiaoJueyan; }
internal sealed class MouXiaoQiaoOlHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost h) =>
        h.GetCapability<IMouXiaoQiaoOlRuntime>().Resolve(f, op);
}
