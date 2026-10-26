namespace CardGame.Core;

public sealed record ShanheLuanxinNextDrawIssuedEvent(long ProgramFrameId, int InstructionIndex,
    CardConversionSource Source, string GameplayHash, long MovementBatchId, int TargetSeat) : IGameEvent;
public sealed record ShanheLuanxinNextDrawAssignedEvent(long ProgramFrameId, int InstructionIndex,
    ActualDiscardRecoveryPhaseKey Phase) : IGameEvent;

internal static class ShanheLuanxinProgramContract
{
    [ProgramCatalogValidation(81600)]
    internal static void Validate(string path, SkillProgram program)
    {
        var operation = SkillProgramEffectOp.QueueShanheLuanxinNextDraw;
        var triggered = program.Triggers.Where(trigger => trigger.Effects.Any(effect => effect.Op == operation)).ToArray();
        if (triggered.Length == 0 && !program.Activations.Any(activation => activation.Effects.Any(effect => effect.Op == operation))) return;
        if (program.Id != "shanhe:companion-luanxin" || program.Activations.Any(activation => activation.Effects.Any(effect => effect.Op == operation)) ||
            triggered is not [{ Id: "other-hand-gain", Window: SkillProgramTriggerWindow.CardsGained,
                Subject: SkillProgramTriggerSubject.Owner, MovementOccurrence: SkillProgramMovementOccurrence.PerBatch,
                Optional: false, Effects: [{ Op: SkillProgramEffectOp.QueueShanheLuanxinNextDraw }] } trigger] ||
            !trigger.DestinationZones.SequenceEqual([CardZoneKind.Hand]))
            throw new InvalidOperationException(path + ": Luanxin's future Draw benefit requires its mandatory owner hand-gain batch trigger.");
    }
}

internal interface IShanheLuanxinProgramHost { void QueueNextDraw(ProgramSkillFrame frame); }
internal sealed class QueueShanheLuanxinNextDrawDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.QueueShanheLuanxinNextDraw;
    public override ISkillProgramEffectHandler Handler { get; } = new QueueShanheLuanxinNextDrawHandler();
    public override ProgramFactRequirements FactRequirements => new(Triggered:
        ProgramTrackedFact.NativeLogicalDiscard | ProgramTrackedFact.ActualDiscardRecoveryPhase);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier,
        static (_, context) => context.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 1, reader.Condition());
        RequireAlways(effect, reader.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
}
internal sealed class QueueShanheLuanxinNextDrawHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.QueueShanheLuanxinNextDraw;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    { host.GetCapability<IShanheLuanxinProgramHost>().QueueNextDraw(frame); return SkillProgramStepOutcome.Continue; }
}

public sealed partial class GameEngine
{
    private const string ShanheLuanxinSkillId = "shanhe:companion-luanxin";
    [ProgramCapability]
    private sealed class ShanheLuanxinProgramHost(GameEngine engine) : IShanheLuanxinProgramHost
    { public void QueueNextDraw(ProgramSkillFrame frame) => engine.QueueShanheLuanxinNextDraw(frame); }

    private bool HasShanheLuanxinForeignHandDiscard(int owner, CardMovementBatchContext batch)
    {
        var sequences = batch.Movements.Select(movement => (long)movement.Sequence).ToHashSet();
        var invoices = ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>()
            .Where(invoice => invoice.ExecutorSeat != owner).Select(invoice => invoice.InvocationId).ToHashSet();
        return ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Any(material =>
            invoices.Contains(material.InvocationId) && material.From == CardLocation.Hand(owner) &&
            material.To == CardLocation.DiscardPile && sequences.Contains(material.CompletionSequence));
    }
    private bool HasShanheLuanxinOtherHandGain(int owner, CardMovementBatchContext batch) =>
        batch.Movements.Any(movement => movement.To == CardLocation.Hand(owner) &&
            ActualHandGainOriginalSource(movement, batch) is { Zone: CardZoneKind.Hand, OwnerSeat: { } source } && source != owner);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 81600)]
    private bool CanOfferShanheLuanxinMovement(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != ShanheLuanxinSkillId) return true;
        if (trigger.Id is not ("foreign-hand-discard" or "other-hand-gain")) return true;
        if (context.MovementBatch is not { } batch) return false;
        return trigger.Id == "foreign-hand-discard" ? HasShanheLuanxinForeignHandDiscard(candidate.OwnerSeat, batch)
            : HasShanheLuanxinOtherHandGain(candidate.OwnerSeat, batch);
    }

    private void QueueShanheLuanxinNextDraw(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != ShanheLuanxinSkillId || frame.TriggerId != "other-hand-gain" ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardsGained, MovementBatch: { } batch } ||
            !HasShanheLuanxinOtherHandGain(frame.OwnerSeat, batch) ||
            ProgramEventHistory<ShanheLuanxinNextDrawIssuedEvent>().Any(issued => issued.ProgramFrameId == frame.Id && issued.InstructionIndex == frame.InstructionIndex))
            throw new InvalidOperationException("Luanxin requires its original completed other-player hand gain batch.");
        AdvanceEventRulesAndQueueFact(new ShanheLuanxinNextDrawIssuedEvent(frame.Id, frame.InstructionIndex,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, batch.Id, frame.OwnerSeat));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 81600)]
    private void AssignShanheLuanxinNextDraw(ActualDiscardRecoveryPhaseStartedEvent started)
    {
        if (started.Phase.Kind != ActualDiscardRecoveryPhaseKind.Draw || !_players[started.Phase.ActorSeat].IsAlive ||
            !_contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.QueueShanheLuanxinNextDraw)) return;
        var assigned = ProgramEventHistory<ShanheLuanxinNextDrawAssignedEvent>().Select(item => (item.ProgramFrameId, item.InstructionIndex)).ToHashSet();
        foreach (var issued in ProgramEventHistory<ShanheLuanxinNextDrawIssuedEvent>().Where(item =>
            item.TargetSeat == started.Phase.ActorSeat && !assigned.Contains((item.ProgramFrameId, item.InstructionIndex))).ToArray())
            AdvanceEventRulesAndQueueFact(new ShanheLuanxinNextDrawAssignedEvent(issued.ProgramFrameId, issued.InstructionIndex, started.Phase));
    }

    private int ShanheLuanxinNextDrawBonus(CharacterState player)
    {
        if (!_contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.QueueShanheLuanxinNextDraw) ||
            CurrentActualDiscardRecoveryPhase() is not { Kind: ActualDiscardRecoveryPhaseKind.Draw } phase || phase.ActorSeat != player.Seat) return 0;
        var assigned = ProgramEventHistory<ShanheLuanxinNextDrawAssignedEvent>().Where(item => item.Phase == phase)
            .Select(item => (item.ProgramFrameId, item.InstructionIndex)).ToHashSet();
        return ProgramEventHistory<ShanheLuanxinNextDrawIssuedEvent>().Count(item => item.TargetSeat == player.Seat &&
            assigned.Contains((item.ProgramFrameId, item.InstructionIndex)));
    }
}
