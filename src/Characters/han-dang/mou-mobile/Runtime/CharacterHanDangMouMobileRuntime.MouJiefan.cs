#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanDangMouMobileRuntime
{
    internal int MouHanDangPhaseUses(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouHanDangStartedEvent>().Count(e => !e.Gongqi && e.Source.OwnerSeat == owner && e.Phase == phase);
    internal bool MouHanDangJiefanDisabled(int owner)
    {
        if (!HasMouHanDangCapability)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var index = Array.FindLastIndex(history, e => e is MouHanDangJiefanDisabledEvent d && d.OwnerSeat == owner);
        return index >= 0 && !history.Skip(index + 1).OfType<PlayerDiedEvent>().Any(e => e.KillerSeat == owner && e.VictimSeat != owner);
    }

    [ProgramPhaseUsedProjection(50)]
    internal int MouHanDangProjectedUses(int owner, string skill, int fallback) => skill == MouHanDangJiefan && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p ? MouHanDangPhaseUses(owner, p) : fallback;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 30700, SkillProgramEffectOp.ResolveMouHanDangJiefan)]
    internal bool CanStartMouHanDangJiefan(CharacterState owner) => !MouHanDangJiefanDisabled(owner.Seat) && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == owner.Seat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) && MouHanDangPhaseUses(owner.Seat, p) == 0;
    internal IReadOnlyList<int> MouHanDangRangeSeats(int target, int first) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != target && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(p.Seat, target)).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(first, p.Seat)).Select(p => p.Seat).ToArray());
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouHanDangJiefan)]
    internal void ReturnMouHanDangDraw(ProgramSkillFrame f, NativeDrawOutcome d)
    {
        if (!ValidMouHanDang(f) || f.MouHanDang is not { Stage: MouHanDangStage.Drawing, Draw: null, Gongqi: false } r || d.RecipientSeat != r.TargetSeat || d.RequestedCount != r.RequestedDraw)
            throw new InvalidOperationException("Jiefan requires its exact one issued native Draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHanDang = r with { Stage = MouHanDangStage.DrawChildren, Draw = d, Before = d.SequenceBefore, After = d.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHanDangDrawReturnedEvent(f.Id, r.TargetSeat, d.RequestedCount, d.ActualDrawIds.Count, d.ReplacementGainedIds.Count, d.AttemptFrameId));
        SealMouHanDangMovements(f.Id, d.SequenceBefore);
        ContinueMouHanDang(f.Id);
    }
}
