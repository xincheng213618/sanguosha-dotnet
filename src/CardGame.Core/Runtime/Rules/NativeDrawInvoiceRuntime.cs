namespace CardGame.Core;

internal sealed record NativeDrawInvocationDraft(long Id, int Owner, int Requested, int Turn, int TurnOwner,
    int PhaseActor, ActualDiscardRecoveryPhaseKey? Phase, long? Parent, CardConversionSource? Producer,
    CardMoveReason Reason, long Before)
{
    public NativeDrawProgramOrigin? ProgramOrigin { get; init; }
    public GameStartingDrawOrigin? GameStartingOrigin { get; init; }
    public GameStartingMovementDrawOrigin? GameStartingMovementOrigin { get; init; }
    private readonly IReadOnlyList<ZhanjiDrawEntitlement>? _zhanjiEntitlements;
    public IReadOnlyList<ZhanjiDrawEntitlement>? ZhanjiEntitlements
    { get => _zhanjiEntitlements; init => _zhanjiEntitlements = value is null ? null : Array.AsReadOnly(value.ToArray()); }
    private readonly IReadOnlyList<NativeDrawOrdinalEntitlement>? _ordinalEntitlements;
    public IReadOnlyList<NativeDrawOrdinalEntitlement>? OrdinalEntitlements
    { get => _ordinalEntitlements; init => _ordinalEntitlements = value is null ? null : Array.AsReadOnly(value.ToArray()); }
    private readonly IReadOnlyList<NativeDrawThresholdEntitlement>? _thresholdEntitlements;
    public IReadOnlyList<NativeDrawThresholdEntitlement>? ThresholdEntitlements
    { get => _thresholdEntitlements; init => _thresholdEntitlements = value is null ? null : Array.AsReadOnly(value.ToArray()); }
}

internal interface INativeDrawInvocationInvoices : IProgramRuntimeOperations
{
    NativeDrawInvocationDraft? Capture(int owner, int requested, CardMoveReason reason, CardConversionSource? directProducer);
    void Complete(NativeDrawInvocationDraft? draft, IReadOnlyList<int> actualIds);
}

internal interface INativeDrawInvoiceQueries : IProgramSelectionQuery
{
    bool IsValid(NativeDrawInvocationProof proof);
    NativeDrawProgramOrigin? CaptureProgramOrigin(ProgramSkillFrame frame);
}

// Invoice state travels with the original synchronous draw. This service only
// joins its physical movement journal, pending batch and committed/pending fact.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeDrawInvoiceRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramContentQueries content, IProgramTurnQueries turn, IProgramEventHistoryQueries history,
    ICardMovementHistoryQueries movements, ICardMovementBatchQueue batches,
    ICardMovementBatchNativeDrawInvoices batchInvoices, INativeDrawInvoiceFacts facts,
    IGameStartingDrawOrigins starting, IZhanjiDrawEntitlements zhanji, INativeDrawInvoiceCompletions native,
    INativeDrawOrdinalInvoices ordinals, INativeDrawThresholdInvoices thresholds,
    IActualDiscardRecoveryPhaseQueries actualPhases) : INativeDrawInvocationInvoices, INativeDrawInvoiceQueries
{
    private long MovementSequence
    {
        get
        {
            var records = movements.Read();
            return records.Count == 0 ? 0 : records[^1].Sequence;
        }
    }

    public bool TracksInvocations =>
        content.RequiresFact(ProgramTrackedFact.NativeDrawInvocation) ||
        facts.ModeTracksInvocations ||
        content.UsesTriggerCondition(SkillProgramTriggerConditionKind.NativeDrawActualTurnOrdinalIs) ||
        content.UsesDrawPhaseDrawCountHandLimit ||
        content.HasTriggerOperation(SkillProgramEffectOp.ResolveMouHuangYueyingLixian) ||
        facts.HasPangJiCapability || facts.HasLeMiHengJiguCapability || facts.HasLeZhouFeiLingkongCapability ||
        facts.HasTaoQianCapability ||
        facts.HasZhaoZhongCapability ||
        facts.HasHellZhiwangHolder ||
        facts.HasFengweiCapability ||
        facts.HasFeiYiHezhong ||
        content.HasTriggerOperation(SkillProgramEffectOp.ResolveGoudeActualTurnEnd) ||
        content.HasTriggerOperation(SkillProgramEffectOp.DrawAfterActualOutsideDraw) ||
        content.HasTriggerOperation(SkillProgramEffectOp.ExchangeFriendGainForDiscard) ||
        content.HasTriggerOperation(SkillProgramEffectOp.OfferNativeBatchHandGainOrCardLossBenefit) ||
        content.HasTriggerOperation(SkillProgramEffectOp.DrawAfterActualPlayDrawInvocation) ||
        content.HasTriggerOperation(SkillProgramEffectOp.BindOwnTurnEnemyHandGain) ||
        facts.HasTangTangZhiyi || facts.HasYuanJiCapability || facts.HasSanwenCapability;

    public NativeDrawInvocationDraft? Capture(int owner, int requested, CardMoveReason reason,
        CardConversionSource? directProducer)
    {
        if (!TracksInvocations || !facts.SetupComplete || requested <= 0 || reason == CardMoveReasons.InitialDeal ||
            turn.TurnNumber < 0 || !facts.IsValidSeat(owner) || !facts.IsValidSeat(turn.TurnOwnerSeat) ||
            facts.Winner != Winner.None || facts.Status == EngineStatus.Completed) return null;
        GameStartingMovementDrawOrigin? movementOrigin = null;
        var startupOrigin = turn.TurnNumber == 0 ? starting.Capture(owner, requested, reason, directProducer, out movementOrigin) : null;
        if (turn.TurnNumber == 0 && startupOrigin is null && movementOrigin is null) return null;
        var phase = actualPhases.Current();
        var draft = new NativeDrawInvocationDraft(frames.NextId(), owner, requested, turn.TurnNumber, turn.TurnOwnerSeat,
            phase?.ActorSeat ?? turn.CurrentSeat, phase, frames.Top?.Id, directProducer, reason,
            MovementSequence);
        if ((startupOrigin?.OutsideDrawChild ?? movementOrigin?.DrawChild) is { } child)
            events.AdvanceFact(new GameStartingDrawChildRequestedEvent(draft.Id, child));
        return draft with { ProgramOrigin = frames.Top is ProgramSkillFrame producer
                ? CaptureProgramOrigin(producer) : null,
            GameStartingOrigin = startupOrigin, GameStartingMovementOrigin = movementOrigin,
            ZhanjiEntitlements = zhanji.Capture(draft.Id, owner, draft.Before),
            OrdinalEntitlements = ordinals.Capture(draft.Id, owner, draft.Before),
            ThresholdEntitlements = thresholds.Capture(draft.Id, owner, draft.Before) };
    }

    public NativeDrawProgramOrigin? CaptureProgramOrigin(ProgramSkillFrame frame)
    {
        if (frame.InstructionIndex < 1) return null;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, content.Skill(frame.SkillId).Program!);
        return frame.InstructionIndex > plan.Instructions.Count ? null : new(
            new(frame.SkillId, RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId),
            frame.GameplayHash, frame.InstructionIndex, plan.Instructions[frame.InstructionIndex - 1].Op);
    }

    public void Complete(NativeDrawInvocationDraft? draft, IReadOnlyList<int> actualIds)
    {
        if (draft is null || actualIds.Count == 0) return;
        if (actualIds.Count > draft.Requested || actualIds.Distinct().Count() != actualIds.Count)
            throw new InvalidOperationException("A native draw invocation cannot invent or repeat physical draw materials.");
        var after = MovementSequence;
        var materials = movements.Read().Where(m => m.Sequence > draft.Before && m.Sequence <= after &&
                m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(draft.Owner) && m.Reason == draft.Reason)
            .OrderBy(m => m.Sequence).Select(m => new NativeDrawMaterial(m.CardId, m.CardKind, m.Sequence)).ToArray();
        if (!materials.Select(m => m.CardId).SequenceEqual(actualIds))
            throw new InvalidOperationException("A native draw invocation lost its exact real DrawCards entities.");
        var last = materials[^1];
        var index = batches.PendingBatchCount - 1;
        while (index >= 0)
        {
            var batch = batches.PendingBatchAt(index);
            if (batch.ParentFrameId == draft.Parent &&
                batch.Movements.Any(m => m.Sequence == last.MovementSequence && m.CardId == last.CardId)) break;
            index--;
        }
        if (index < 0 || batches.PendingBatchAt(index).NativeDrawInvocation is not null)
            throw new InvalidOperationException("A native draw invocation lost its last unclaimed physical draw batch.");
        var proof = new NativeDrawInvocationProof { InvocationId = draft.Id, OwnerSeat = draft.Owner,
            RequestedCount = draft.Requested, ActualTurnNumber = draft.Turn, ActualTurnOwnerSeat = draft.TurnOwner,
            PhaseActorSeat = draft.PhaseActor, Phase = draft.Phase, ParentFrameId = draft.Parent,
            DirectProducer = draft.Producer, ProgramOrigin = draft.ProgramOrigin,
            Reason = draft.Reason, SequenceBefore = draft.Before, SequenceAfter = after,
            LastBatchId = batches.PendingBatchAt(index).Id, Materials = materials, ZhanjiEntitlements = draft.ZhanjiEntitlements,
            GameStartingOrigin = draft.GameStartingOrigin, GameStartingMovementOrigin = draft.GameStartingMovementOrigin,
            ActualTurnDrawOrdinal = ordinals.Freeze(draft), OrdinalEntitlements = draft.OrdinalEntitlements,
            ThresholdProgress = thresholds.Freeze(draft, materials.Length) };
        batchInvoices.AssignNativeDrawInvoice(index, proof);
        events.AdvanceFact(new NativeDrawInvocationRecordedEvent(proof.InvocationId, proof.OwnerSeat,
            proof.RequestedCount, proof.ActualCount, proof.ActualTurnNumber, proof.ActualTurnOwnerSeat, proof.PhaseActorSeat,
            proof.Phase, proof.ParentFrameId, proof.DirectProducer, proof.Reason, proof.SequenceBefore, proof.SequenceAfter,
            proof.LastBatchId) { GameStartingOrigin = proof.GameStartingOrigin, GameStartingMovementOrigin = proof.GameStartingMovementOrigin,
                ActualTurnDrawOrdinal = proof.ActualTurnDrawOrdinal, ProgramOrigin = proof.ProgramOrigin });
        thresholds.Complete(proof);
        native.CompleteSanwenNativeDrawQualification(proof);
        native.CompleteTangTangNativeDrawQualification(proof);
        native.CompleteLingkongNativeDrawQualification(proof);
    }

    public bool IsValid(NativeDrawInvocationProof p)
    {
        if (p.InvocationId <= 0 || !facts.IsValidSeat(p.OwnerSeat) || !facts.IsValidSeat(p.ActualTurnOwnerSeat) ||
            !facts.IsValidSeat(p.PhaseActorSeat) || p.ActualTurnNumber < 0 || p.ActualCount < 1 ||
            (p.ActualTurnNumber == 0 ? !starting.IsValidProvenance(p) : p.GameStartingOrigin is not null || p.GameStartingMovementOrigin is not null) ||
            p.RequestedCount < p.ActualCount || p.Reason == CardMoveReasons.InitialDeal || p.LastBatchId <= 0 ||
            p.SequenceBefore < 0 || p.SequenceAfter <= p.SequenceBefore || p.SequenceAfter > MovementSequence ||
            p.Materials.Select(m => m.CardId).Distinct().Count() != p.ActualCount ||
            p.Materials.Select(m => m.MovementSequence).Distinct().Count() != p.ActualCount ||
            !p.Materials.Select(m => m.MovementSequence).SequenceEqual(p.Materials.Select(m => m.MovementSequence).Order()) ||
            p.Phase is { } phase && (!actualPhases.IsRecorded(phase) ||
                phase.TurnNumber != p.ActualTurnNumber || phase.ActualTurnOwnerSeat != p.ActualTurnOwnerSeat || phase.ActorSeat != p.PhaseActorSeat)) return false;
        var recordedFacts = history.Read<NativeDrawInvocationRecordedEvent>()
            .Where(e => e.InvocationId == p.InvocationId).ToArray();
        if (recordedFacts is not [var fact] || fact.OwnerSeat != p.OwnerSeat || fact.Requested != p.RequestedCount ||
            fact.ActualCount != p.ActualCount || fact.TurnNumber != p.ActualTurnNumber ||
            fact.ActualTurnOwnerSeat != p.ActualTurnOwnerSeat || fact.PhaseActorSeat != p.PhaseActorSeat || fact.Phase != p.Phase ||
            fact.ParentFrameId != p.ParentFrameId || fact.DirectProducer != p.DirectProducer || fact.Reason != p.Reason ||
            fact.ProgramOrigin != p.ProgramOrigin ||
            fact.ActualTurnDrawOrdinal != p.ActualTurnDrawOrdinal ||
            fact.GameStartingOrigin != p.GameStartingOrigin ||
            !starting.SameMovementDrawOrigin(fact.GameStartingMovementOrigin, p.GameStartingMovementOrigin) ||
            fact.SequenceBefore != p.SequenceBefore || fact.SequenceAfter != p.SequenceAfter || fact.LastBatchId != p.LastBatchId) return false;
        var actual = movements.Read().Where(m => m.Sequence > p.SequenceBefore && m.Sequence <= p.SequenceAfter &&
            m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(p.OwnerSeat) && m.Reason == p.Reason)
            .OrderBy(m => m.Sequence).Select(m => new NativeDrawMaterial(m.CardId, m.CardKind, m.Sequence));
        return p.Materials.SequenceEqual(actual) && zhanji.IsValid(p) && ordinals.IsValid(p) && thresholds.IsValid(p);
    }
}
