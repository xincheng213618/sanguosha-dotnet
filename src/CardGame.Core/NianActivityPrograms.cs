namespace CardGame.Core;

internal interface INianActivityProgramHost
{
    SkillProgramStepOutcome ExecuteNianActivity(ProgramSkillFrame frame, SkillProgramEffect effect);
}

internal abstract class NianActivityOperationDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new NianActivityEffectHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
}
internal sealed class ApplyNianMengtaiDiscardDescriptor : NianActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ApplyNianMengtaiDiscard;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.DiscardPhaseStarting)];
}
internal sealed class DrawNianMengtaiEndingDescriptor : NianActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawNianMengtaiEnding;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding)];
}
internal sealed class DiscardNianRuizhiRemainderDescriptor : NianActivityOperationDescriptor
{
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeLogicalDiscard);

    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DiscardNianRuizhiRemainder;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow), new ReadCardSet("ruizhi-hand"), new ReadCardSet("ruizhi-equipment")];
}
internal sealed class RecoverNianJingjueLossDescriptor : NianActivityOperationDescriptor
{
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeLogicalDiscard);

    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RecoverNianJingjueLoss;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CardsMoved)];
}
internal sealed class DrawNianRenxingDamageDescriptor : NianActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawNianRenxingDamage;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
}
internal sealed class DrawNianRenxingRecoveryDescriptor : NianActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawNianRenxingRecovery;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterHpRecovered)];
}
internal sealed class DrawNianBaonuReplacementDescriptor : NianActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawNianBaonuReplacement;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.DrawPhaseStarting)];
}
internal sealed class NianActivityEffectHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<INianActivityProgramHost>().ExecuteNianActivity(frame, effect);
}

/// <summary>Only an actually skipped phase earns Mengtai, never a scheduled future skip.</summary>
public sealed record NianActualPhaseSkippedEvent(int ActualTurnNumber, int OwnerSeat, SkillProgramTurnPhase Phase) : IGameEvent;
public sealed record NianJingjueLossRecoveredEvent(long MovementBatchId, int OwnerSeat) : IGameEvent;
public sealed record NianBaonuDrawRolledEvent(long ProgramFrameId, int OwnerSeat, int HpAtRoll, int DrawCount) : IGameEvent;
public sealed record NianOwnedTransferProof(int CardId, int OwnerSeat, CardLocation OriginalFrom, int RecipientSeat,
    long EntryBatchId, int EntrySequence, int ExitSequence, long ParentFrameId, CardConversionSource? Source,
    string? GameplayHash, int? InstructionIndex, bool NativeSnatch = false);
public sealed partial record CardMovementBatchContext
{
    private IReadOnlyList<NianOwnedTransferProof>? _nianOwnedTransfers;
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<NianOwnedTransferProof>? NianOwnedTransfers
    { get => _nianOwnedTransfers; init => _nianOwnedTransfers = value is null ? null : Array.AsReadOnly(value.ToArray()); }
}
