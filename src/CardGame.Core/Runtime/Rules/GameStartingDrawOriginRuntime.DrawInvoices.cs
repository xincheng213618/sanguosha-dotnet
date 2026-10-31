namespace CardGame.Core;

// Startup draw authority follows the original frames, movement batches and
// complete fact journal. Every paid child stays on its issuing program frame.
[ProgramRuntimeModule, ProgramCapability]
internal sealed partial class GameStartingDrawOriginRuntime(IProgramRuntimeFrames frames,
    INativeFrameRoutingFacts nativeFrames, IProgramRuntimeEvents events, IProgramContentQueries content,
    IProgramEventJournalQueries journal, ICardMovementHistoryQueries movements,
    ICardMovementBatchQueue batches, IGameStartingMovementBatchOrigins batchOrigins,
    IProgramTurnQueries turn, IProgramPhaseQueries phase, ISelectionParticipantQueries participants,
    IGameStartingDrawNativeFacts facts, IGameStartingDrawNativeValidation nativeValidation,
    IGameStartingDrawNativeReceipts receipts, IDingShangWanFrameProofs dingShangWan) : IGameStartingDrawOrigins, IGameStartingDrawOriginQueries
{
    private const string FudaoGiftReason = "skill-program.ding-shang-wan.fudao.give";
    private const string ZhiyiDrawReason = "skill-program.tang-tang.zhiyi.draw";

    private long MovementSequence
    {
        get
        {
            var records = movements.Read();
            return records.Count == 0 ? 0 : records[^1].Sequence;
        }
    }

    // Preserve the original first-match lookup when testing a supplied frame.
    private int FrameIndex(long id)
    {
        for (var index = 0; index < nativeFrames.FrameCount; index++)
            if (nativeFrames.FrameAt(index).Id == id) return index;
        return -1;
    }

    public GameStartingDrawOrigin? Capture(int recipient, int requested, CardMoveReason reason,
        CardConversionSource? producer, out GameStartingMovementDrawOrigin? movementOrigin) =>
        CaptureGameStartingDrawOrigin(recipient, requested, reason, producer, out movementOrigin);

    public bool IsValidProvenance(NativeDrawInvocationProof proof) => ValidGameStartingDrawProvenance(proof);

    public bool SameMovementDrawOrigin(GameStartingMovementDrawOrigin? left, GameStartingMovementDrawOrigin? right) =>
        SameGameStartingMovementDrawOrigin(left, right);

    public bool IsExactGameStartingDrawProgram(ProgramSkillFrame program) =>
        TryGetGameStartingDrawProgram(program, out _);

    internal bool TryGetGameStartingDrawProgram(ProgramSkillFrame program, out ProgramLifecycleTriggerWindowFrame? window)
    {
        window = null;
        if (!facts.SetupComplete || turn.TurnNumber != 0 || phase.Phase != TurnPhase.NotStarted ||
            program.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting } context ||
            context.OwnerSeat != program.OwnerSeat || program.TriggerId is null ||
            content.FindSkill(program.SkillId)?.Program is not { } definition ||
            definition.GameplayHash != program.GameplayHash || program.InstructionIndex < 1) return false;
        var index = FrameIndex(program.Id);
        if (index < 1 || nativeFrames.FrameAt(index - 1) is not ProgramLifecycleTriggerWindowFrame parent ||
            parent.Id != context.ParentFrameId || parent.Window != SkillProgramTriggerWindow.GameStarting ||
            parent.Continuation != ProgramLifecycleContinuation.CompleteGameStarting || parent.OwnerSeat != turn.CurrentSeat ||
            parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count ||
            !RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(program, parent.Candidates[parent.CandidateIndex])) return false;
        var plan = ProgramInstructionResolver.Default.Resolve(program, definition);
        if (program.InstructionIndex > plan.Instructions.Count) return false;
        window = parent;
        return true;
    }

    internal GameStartingDrawOrigin? CaptureGameStartingDrawOrigin(int recipient, int requested, CardMoveReason reason,
        CardConversionSource? directProducer, out GameStartingMovementDrawOrigin? movementOrigin)
    {
        var derived = CaptureGameStartingGainDrawOrigin(recipient, requested, reason, directProducer, out movementOrigin);
        if (derived is not null || movementOrigin is not null) return derived;
        movementOrigin = CaptureGameStartingNativeMovementDrawOrigin(recipient, requested, reason, directProducer);
        if (movementOrigin is not null) return null;
        if (frames.Top is not ProgramSkillFrame { NativeDraw: { } paid } program ||
            !TryGetGameStartingDrawProgram(program, out var window) || paid.InstructionIndex != program.InstructionIndex ||
            paid.RequestedCount != requested || paid.Reason != reason || paid.RecipientApplied || paid.Replacement is not null ||
            paid.RecipientIndex < 0 || paid.RecipientIndex >= paid.Recipients.Count || paid.Recipients[paid.RecipientIndex] != recipient)
            return null;
        var origin = new GameStartingDrawOrigin(window!.Id, program.Id, program.InstructionIndex, program.GameplayHash,
            new(program.SkillId, RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(program), program.OwnerSeat, program.SkillInstanceId), paid.ReturnKind, paid.Operation);
        return GameStartingDrawOriginProgramMatches(origin) ? origin : null;
    }

    internal bool GameStartingDrawOriginProgramMatches(GameStartingDrawOrigin origin)
    {
        if (origin.WindowFrameId <= 0 || origin.ProgramFrameId <= origin.WindowFrameId || origin.InstructionIndex < 1 ||
            !participants.IsValidSeat(origin.Source.OwnerSeat) ||
            content.FindSkill(origin.Source.SkillId)?.Program is not { } definition ||
            definition.GameplayHash != origin.GameplayHash) return false;
        var trigger = definition.Triggers.SingleOrDefault(item => item.Id == origin.Source.BindingId);
        if (trigger?.Window != SkillProgramTriggerWindow.GameStarting) return false;
        var plan = ProgramInstructionResolver.Default.Resolve(definition, ProgramInstructionSourceKind.Trigger, trigger.Id);
        if (origin.InstructionIndex > plan.Instructions.Count) return false;
        var instruction = plan.Instructions[origin.InstructionIndex - 1];
        return origin.ReturnKind == NativeDrawAttemptReturnKind.ProgramInstruction
            ? origin.Operation is null && instruction.CompiledInstruction is DrawProgramInstruction
            : origin.ReturnKind == NativeDrawAttemptReturnKind.ProgramOperation && origin.Operation == instruction.Op &&
                origin.Operation is { } operation && facts.HasOperationReturn(operation);
    }

    internal bool ValidGameStartingDrawOrigin(NativeDrawInvocationProof proof)
    {
        if (proof.GameStartingOrigin?.OutsideDrawChild is not null)
            return ValidGameStartingGainDrawChain(proof);
        if (proof.GameStartingOrigin is not { } origin || proof.ActualTurnNumber != 0 || proof.Phase is not null ||
            proof.ParentFrameId != origin.ProgramFrameId || proof.InvocationId <= origin.ProgramFrameId ||
            !GameStartingDrawOriginProgramMatches(origin)) return false;
        return journal.Complete().OfType<ProgramBindingStartedEvent>().Count(started =>
            started.FrameId == origin.ProgramFrameId && started.SkillId == origin.Source.SkillId &&
            started.BindingId == origin.Source.BindingId && started.SkillInstanceId == origin.Source.SkillInstanceId &&
            started.OwnerSeat == origin.Source.OwnerSeat && started.Window == SkillProgramTriggerWindow.GameStarting) == 1;
    }
}
