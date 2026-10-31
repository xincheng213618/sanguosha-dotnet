#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoXiOlRuntime
{
    internal bool ValidCaoXiGangshu(ProgramSkillFrame f)
    {
        if (f.CaoXiGangshu is not { } r || f.SkillId != "ol:gangshu" || f.InstructionIndex != 1 || r.InstructionIndex != 1 || r.Source != CaoXiSource(f) || r.GameplayHash != f.GameplayHash || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } use } c || c.ParentFrameId != r.ParentWindowId || use.CardActionId != r.ActionId || use.ParentCardUseFrameId != r.CardUseFrameId || use.ActorSeat != f.OwnerSeat || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(use.EffectiveKind, [SkillProgramCardCategory.Basic]) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveGangshuAttributeIncrement || r.Numbers != CurrentCaoXiNumbers(f.OwnerSeat) || !global::CardGame.Core.EngineOwners.CharacterCaoXiOlDefinitions.CaoXiQueries.Any(q => r.Numbers.Value(q) < 5) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CaoXiGangshuStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.ParentWindowId == r.ParentWindowId && e.CardUseFrameId == r.CardUseFrameId && e.ActionId == r.ActionId && e.Numbers == r.Numbers) != 1)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return i > 0 && _resolutionStackOperations._resolutionStack[i - 1] is ProgramCardTriggerWindowFrame w && w.Id == r.ParentWindowId && w.Action.ActionId == r.ActionId && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && w.Candidates[w.CandidateIndex] is { } candidate && candidate.OwnerSeat == f.OwnerSeat && candidate.SkillId == f.SkillId && candidate.TriggerId == f.TriggerId && candidate.SkillInstanceId == f.SkillInstanceId && candidate.GameplayHash == f.GameplayHash;
    }
}
