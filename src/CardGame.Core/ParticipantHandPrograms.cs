namespace CardGame.Core;

public sealed record ProgramPrivateHandTakeDraft(int SourceSeat, int RequiredCount,
    IReadOnlyList<int> ViewedCardIds, IReadOnlyList<int> SelectedCardIds, bool Paid = false);
public sealed record ProgramHandRepaymentDraft(int SourceSeat, int InstructionIndex,
    bool Paid = false, int? PaidCardId = null, bool LostHp = false);
public sealed record ProgramBoundParticipantGiftReceipt(int InstructionIndex, string SourceBind, int RecipientSeat)
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public ProgramOwnedHandRankSumGiftReceipt? RankSum { get; init; }
}
public sealed record ProgramOwnedHandRankSumGiftEntity(int CardId, CardKind PrintedKind, int PrintedRank,
    CardLocation From, int EffectiveRank);
public sealed record ProgramOwnedHandRankSumGiftReceipt(long OwnerFrameId, int RequiredSum, long SequenceBefore, CardMoveReason Reason)
{
    private IReadOnlyList<ProgramOwnedHandRankSumGiftEntity> _entities = Array.AsReadOnly(Array.Empty<ProgramOwnedHandRankSumGiftEntity>());
    public IReadOnlyList<ProgramOwnedHandRankSumGiftEntity> Entities { get => _entities; init => _entities = Array.AsReadOnly((value ?? []).ToArray()); }
    public long BatchId { get; init; }
    public long SequenceAfter { get; init; }
}
public sealed record OwnedHandRankSumGiftIssuedEvent(long FrameId, CardConversionSource Source, string GameplayHash,
    string SourceBind, int RecipientSeat, int RequiredSum, int EntityCount, long SequenceBefore, CardMoveReason Reason) : IGameEvent;
public sealed record OwnedHandRankSumGiftEntityFrozenEvent(long FrameId, int Index, int CardId, CardKind PrintedKind,
    int PrintedRank, CardLocation From, int EffectiveRank) : IGameEvent;
public sealed record OwnedHandRankSumGiftPaidEvent(long FrameId, long BatchId, long SequenceAfter, int EntityCount) : IGameEvent;
public sealed record ProgramPrivateHandViewedEvent(long FrameId, string SkillId, int ViewerSeat, int SourceSeat, int Count) : IGameEvent;
public sealed record ProgramHandRepaymentResolvedEvent(long FrameId, string SkillId, int SourceSeat, int RecipientSeat,
    bool GaveCard, bool LostHp) : IGameEvent;

public interface IParticipantHandProgramHost
{
    SkillProgramStepOutcome ViewAndTakeHand(ProgramSkillFrame frame, int sourceSeat, int count);
    SkillProgramStepOutcome RequestHandRepayment(ProgramSkillFrame frame, int sourceSeat);
}

internal sealed class ViewAndTakeSelectedTargetHandDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ViewAndTakeSelectedTargetHand;
    public override ISkillProgramEffectHandler Handler { get; } = new ParticipantHandProgramHandler(SkillProgramEffectOp.ViewAndTakeSelectedTargetHand);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (effect, context) => context.TakeRandomHandCards(effect));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "amount", "condition");
        var target = r.RequiredEnum<SkillProgramEffectTarget>("target"); var count = r.RequiredInt("amount");
        if (target != SkillProgramEffectTarget.SelectedTarget || count is < 1 or > 2)
            throw new InvalidOperationException($"Invalid skill program at {r.Path}: private hand selection requires selectedTarget and amount 1..2.");
        return new(Op, target, count, r.Condition());
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.DrawPhaseEnded), new ReadSelectedTarget()];
}

internal sealed class RequestHandBySuitsOrLoseHpDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RequestHandBySuitsOrLoseHp;
    public override ISkillProgramEffectHandler Handler { get; } = new ParticipantHandProgramHandler(SkillProgramEffectOp.RequestHandBySuitsOrLoseHp);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.LoseHp,
        static (effect, context) => context.LoseHp(effect));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "suits", "condition");
        var target = r.RequiredEnum<SkillProgramEffectTarget>("target"); var suits = r.RequiredEnumArray<Suit>("suits");
        if (target != SkillProgramEffectTarget.SelectedTarget || suits.Count == 0 || suits.Distinct().Count() != suits.Count ||
            suits.Any(suit => suit == Suit.None))
            throw new InvalidOperationException($"Invalid skill program at {r.Path}: hand repayment requires selectedTarget and distinct ordinary suits.");
        var effect = new SkillProgramEffect(Op, target, 1, r.Condition(), suits: suits);
        RequireAlways(effect, r.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied), new ReadSelectedTarget()];
}

internal sealed class ParticipantHandProgramHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        op == SkillProgramEffectOp.ViewAndTakeSelectedTargetHand
            ? host.GetCapability<IParticipantHandProgramHost>().ViewAndTakeHand(frame, targetSeat, effect.Amount)
            : host.GetCapability<IParticipantHandProgramHost>().RequestHandRepayment(frame, targetSeat);
}

public sealed partial class GameEngine
{
}
