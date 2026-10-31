#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuKangWumiaoRuntime
{
    internal static bool IsWuLuKangSelectedPindianSource(SkillProgramActivation a) => RuntimeSkills.RuntimeRules_lu_kang_wumiao_WuLuKangPindianEntry.IsWuLuKangSelectedPindianSource(a);
    internal bool WuLuKangCanOmitPindianSource(int owner, SkillProgramActivation activation) => IsWuLuKangSelectedPindianSource(activation) && CanWuLuKangDiscardForPindian(_coreMatchParticipantsPlayersAccess._players[owner]);
    internal bool WuLuKangActivationTargetHasPindianSource(ProgramExecutionPlan plan, CharacterState target) => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0 || (plan.Features.HasOperation(SkillProgramEffectOp.Pindian) || plan.Features.HasOperation(SkillProgramEffectOp.StartPindian) || plan.Features.HasOperation(SkillProgramEffectOp.StartSimultaneousPindian) || plan.Features.HasOperation(SkillProgramEffectOp.ResolveYouqueMinimumBaitPindian)) && CanWuLuKangDiscardForPindian(target);
    internal static bool IsWuLuKangPindianTargetInstruction(IReadOnlyList<SkillProgramEffect> instructions, int index) => RuntimeSkills.RuntimeRules_lu_kang_wumiao_WuLuKangPindianEntry.IsWuLuKangPindianTargetInstruction(instructions, index);
    internal IReadOnlyList<int> WuLuKangProgramSelectionSeats(int owner, SkillProgramEffect selection, IReadOnlyList<SkillProgramEffect> instructions, ProgramSkillWindowContext? context = null) => _programTargetSelectionOperations.Get().SelectionSeats(owner, selection, instructions, context);
    internal IReadOnlyList<int> WuLuKangProgramSelectionSeats(ProgramSkillFrame frame, SkillProgramTargetKind kind, IReadOnlyList<CardZoneKind> zones, PlayerMarkerKind? marker, ProgramParticipantReference? actor) => _programTargetSelectionOperations.Get().SelectionSeats(frame, kind, zones, marker, actor);
    internal bool WuLuKangProgramSelectionTargetEligible(ProgramSkillFrame frame, SkillProgramTargetKind kind, IReadOnlyList<CardZoneKind> zones, int seat, PlayerMarkerKind? marker, ProgramParticipantReference? actor) => _programTargetSelectionOperations.Get().SelectionTargetEligible(frame, kind, zones, seat, marker, actor);
    internal ProgramSkillFrame CaptureWuLuKangPindianSelection(ProgramSkillFrame f, SkillProgramActivation activation)
    {
        if (f.SelectedCardIds.Count != 0 || !IsWuLuKangSelectedPindianSource(activation))
            return f;
        if (!WuLuKangCanOmitPindianSource(f.OwnerSeat, activation) || f.TriggerId is not null || f.InstructionIndex != 0 || f.WuLuKangPindianSelection is not null || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
            throw new InvalidOperationException("A cardless Pindian source requires its original eligible maximum-deck policy.");
        var source = _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], SkillProgramCardPolicyKind.PindianDiscardForMaximumDeck).First(item => item.Source.SkillId == WuLuKangComposition.Shenduan && WuLuKangComposition.IsShenduan(item.Source.Program));
        var admission = new WuLuKangPindianSelectionAdmission(f.Id, new(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, new(source.Source.SkillId, source.Policy.Id, f.OwnerSeat, source.Source.SkillInstanceId), source.Source.Program.GameplayHash, phase);
        return f with
        {
            WuLuKangPindianSelection = admission
        };
    }

    internal void IssueWuLuKangPindianSelection(ProgramSkillFrame f)
    {
        if (f.WuLuKangPindianSelection is not { } admission)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame current || current.Id != f.Id || current.WuLuKangPindianSelection != admission || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuLuKangPindianSelectionAdmittedEvent>().Any(e => e.Admission.FrameId == f.Id))
            throw new InvalidOperationException("Pindian source admission belongs to its one original native activation.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangPindianSelectionAdmittedEvent(admission));
    }

    internal bool HasWuLuKangPindianSelection(ProgramSkillFrame f, SkillProgramActivation a) => f.WuLuKangPindianSelection is { } admission && f.TriggerId is null && f.SelectedCardIds.Count == 0 && IsWuLuKangSelectedPindianSource(a) && admission.FrameId == f.Id && admission.ProgramSource == new CardConversionSource(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId) && admission.ProgramHash == f.GameplayHash && admission.CapabilitySource.OwnerSeat == f.OwnerSeat && admission.CapabilitySource.SkillId == WuLuKangComposition.Shenduan && admission.CapabilitySource.BindingId == "maximum-deck-source" && admission.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && admission.Phase.ActorSeat == f.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(admission.Phase) && _contentRegistryOperations._contentRegistry.GetSkill(WuLuKangComposition.Shenduan).Program is { } p && p.GameplayHash == admission.CapabilityHash && WuLuKangComposition.IsShenduan(p) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuLuKangPindianSelectionAdmittedEvent>().Count(e => e.Admission == admission) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) == 1;
}
