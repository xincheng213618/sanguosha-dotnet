#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaTuoHappyGodRuntime
{
    internal bool HappyHuaOriginalCandidate(ProgramSkillFrame f, SkillProgramTriggerWindow window, long parentId) => f.WindowContext is { } context && context.Window == window && context.ParentFrameId == parentId && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == parentId)is { } parent && parent.Window == window && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.OwnerSeat == f.OwnerSeat && e.SkillInstanceId == f.SkillInstanceId && e.Window == window) == 1;
    internal bool HappyHuaPayment(ProgramSkillFrame f, CardConversionSource source, int before, int after, string purpose) => source == new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) && before is >= 1 and <= 3 && after >= 0 && after < before && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyHuaMedicinePaidEvent>().Count(e => e == new HappyHuaMedicinePaidEvent(f.Id, source, f.GameplayHash, before, after, purpose)) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PlayerMarkerChangedEvent>().Count(e => e.ResolutionId == f.Id && e.PlayerSeat == f.OwnerSeat && e.Marker == PlayerMarkerKind.HappyMedicine && e.Delta == after - before && e.Count == after && e.SkillOwnerSeat == f.OwnerSeat) == 1;
    internal bool ValidHappyHuaRescue(ProgramSkillFrame f)
    {
        if (f.HappyHuaRescue is not { } r || f.SkillId != "happy:jishi" || f.TriggerId != "medicine-rescue" || f.InstructionIndex is < 1 or > 2 || f.ActivationId != f.TriggerId || r.GameplayHash != f.GameplayHash || r.HpBefore > 0 || r.MedicineAfter != r.MedicineBefore - 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.VictimSeat) || !f.SelectedTargetSeats.SequenceEqual([r.VictimSeat]) || f.WindowContext?.TargetSeat != r.VictimSeat || !HappyHuaOriginalCandidate(f, SkillProgramTriggerWindow.DyingEntering, r.EntryId) || _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().SingleOrDefault(d => d.Id == r.DyingId)is not { } dying || dying.VictimSeat != r.VictimSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(w => w.Id == r.EntryId).ResumeDyingFrameId != dying.Id || !HappyHuaPayment(f, r.Source, r.MedicineBefore, r.MedicineAfter, "rescue"))
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyHuaRescueIssuedEvent>().Count(e => e == new HappyHuaRescueIssuedEvent(f.Id, r.EntryId, r.DyingId, r.VictimSeat, r.HpBefore)) == 1;
    }

    internal bool ValidHappyHuaNeedle(ProgramSkillFrame f)
    {
        if (f.HappyHuaNeedle is not { } r || f.SkillId != "happy:shenzhen" || f.TriggerId != "medicine-needles" || f.ActivationId != f.TriggerId || f.InstructionIndex is < 1 or > 3 || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressTurnNumberAccess._turnNumber || !Enum.IsDefined(r.Stage) || r.Source != new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || !HappyHuaOriginalCandidate(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, r.ParentId) || r.Amount is < 0 or > 3 || r.Before is < 0 or > 3 || r.After < 0 || r.Stage == HappyHuaNeedleStage.AmountChoice && (r.Amount != 0 || r.After != r.Before || f.InstructionIndex != 1 || f.SelectedTargetSeats.Count != 0) || r.Stage == HappyHuaNeedleStage.TargetChoice && (r.Amount == 0 || r.After != r.Before - r.Amount || f.InstructionIndex != 1 || f.SelectedTargetSeats.Count != 0) || r.Amount > 0 && !HappyHuaPayment(f, r.Source, r.Before, r.After, "needle"))
            return false;
        var targets = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyHuaNeedleTargetEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage != HappyHuaNeedleStage.Committed)
            return targets.Length == 0 && !f.ChoiceBindings.Any(b => b.Name == "needle-mode");
        var mode = f.ChoiceBindings.SingleOrDefault(b => b.Name == "needle-mode");
        if (mode is null || mode.ChooserSeat != f.OwnerSeat || mode.OptionId is not ("zero" or "recover" or "lose") || f.SelectedTargetSeats.Count != r.Amount || f.SelectedTargetSeats.Distinct().Count() != r.Amount || f.SelectedTargetSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || mode.OptionId == "zero" && r.Amount != 0 || mode.OptionId != "zero" && r.Amount == 0 || targets.Length != r.Amount)
            return false;
        return targets.Select((e, i) => e == new HappyHuaNeedleTargetEvent(f.Id, i, f.SelectedTargetSeats[i], mode.OptionId)).All(x => x);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41780)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41780)]
    internal bool ContinuePaidHappyHuaTuo(ProgramSkillFrame f) => f.HappyHuaRescue is not null && ValidHappyHuaRescue(f) || f.HappyHuaNeedle is { Amount: > 0 } && ValidHappyHuaNeedle(f);
}
