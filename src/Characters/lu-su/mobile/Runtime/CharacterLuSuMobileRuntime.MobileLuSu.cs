#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuMobileRuntime
{
    internal bool MobileLuSuDimengPaid(ProgramSkillFrame f) => f.SkillId == "mobile:dimeng" && f.TriggerId is null && f.ActivationId == "alliance-pact" && f.InstructionIndex is 2 or 3 && f.SelectedTargetSeats is [var first, var second] && first != second && f.CardSetBindings.SingleOrDefault(b => b.Name == "dimeng-cost")is { CardIds.Count: > 0 } cost && cost.CardIds.Select((id, i) => (Id: id, From: cost.SourceLocations[i])).All(c => c.From.OwnerSeat == f.OwnerSeat && c.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().SkipWhile(e => e is not ProgramSkillStartedEvent started || started.FrameId != f.Id).OfType<CardMovedEvent>().Count(m => m.CardId == c.Id && m.From == c.From && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.mobile:dimeng.MoveBoundCards") == 1) && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) == 1;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 71500)]
    internal bool KeepMobileLuSuDimengPaid(ProgramSkillFrame f) => MobileLuSuDimengPaid(f);
    [ProgramContinuation(ProgramContinuationKind.Movement, 71500)]
    internal bool ReturnMobileLuSuDimengPaid(ProgramSkillFrame f)
    {
        if (!MobileLuSuDimengPaid(f) || f.PendingMovementContinuation is not { SubjectSeat: var seat, BeforeCount: 0, CoverageResultBind: null } || seat != f.OwnerSeat)
            return false;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
}
