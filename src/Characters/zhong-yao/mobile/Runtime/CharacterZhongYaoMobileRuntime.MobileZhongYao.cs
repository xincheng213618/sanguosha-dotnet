#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhongYaoMobileRuntime
{
    internal IReadOnlyList<int> MobileZhongYaoZuodingTargets(int ownerSeat, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } })
            return[];
        var damaged = _corePlayCardHistoryOperations.EventsSinceLastBoundary(item => item is TurnStartedEvent or PhaseChangedEvent { Phase: TurnPhase.Play }).OfType<DamageAppliedEvent>().Where(item => item.Amount > 0).Select(item => item.TargetSeat).ToHashSet();
        return Array.AsReadOnly(_coreProgramLifecycleOperations.GetProgramTargetSeats(ownerSeat, SkillProgramTargetKind.CurrentCardUseTargets, context).Where(seat => !damaged.Contains(ownerSeat) || !damaged.Contains(seat)).ToArray());
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 77700, SkillProgramEffectOp.SelectTarget)]
    internal bool CanOfferMobileZhongYaoZuoding(ProgramTriggerLegalityContext gate) => gate.SkillId != "mobile:zuoding" || MobileZhongYaoZuodingTargets(gate.OwnerSeat, gate.Context).Count > 0;
    IReadOnlyList<int> global::CardGame.Core.IMobileZhongYaoSelectionQueries.EligibleTargets(int ownerSeat) => MobileZhongYaoZuodingTargets(ownerSeat, _resolutionStackOperations._resolutionStack.LastOrDefault() switch
    {
        ProgramSkillFrame frame => frame.WindowContext,
        ProgramCardTriggerWindowFrame window when window.CandidateIndex < window.Candidates.Count => _coreCardActionsOperations.CreateCardActionProgramContext(window, window.Candidates[window.CandidateIndex]),
        _ => null
    });
}
