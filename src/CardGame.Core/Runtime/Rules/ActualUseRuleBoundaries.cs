namespace CardGame.Core;

/// <summary>Expires declared lifetimes at the original actual-use history point.</summary>
[ProgramRuntimeModule]
internal sealed class ActualUseRuleBoundaries(IActualCardUseHistoryQueries history,
    IProgramRuleBoundaries changes)
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27469)]
    internal void Declaring(CardUseDeclaredEvent declared) => NativeUse(declared, SkillStateBoundaryTiming.Before);

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 251)]
    internal void Declared(CardUseDeclaredEvent declared) => NativeUse(declared, SkillStateBoundaryTiming.After);

    private void NativeUse(CardUseDeclaredEvent declared, SkillStateBoundaryTiming timing)
    {
        if (history.FindNativeUse(declared.ResolutionId) is not { } original) return;
        var point = new CardUseHistoryPoint(original.ActualTurn, original.TurnOwnerSeat,
            history.CardUseHistoryCursor);
        var use = history.ReadCardUseFacts(point, CardUseHistoryView.ActualUses)
            .FirstOrDefault(fact => fact.NativeUseId == original.NativeUseId);
        if (use is not null) ActualUse(use, timing);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27468)]
    internal void Accepting(CardActionAcceptedEvent accepted) => ResponseUse(accepted, SkillStateBoundaryTiming.Before);

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 252)]
    internal void Accepted(CardActionAcceptedEvent accepted) => ResponseUse(accepted, SkillStateBoundaryTiming.After);

    private void ResponseUse(CardActionAcceptedEvent accepted, SkillStateBoundaryTiming timing)
    {
        if (accepted.Action.Type != CardActionType.Response ||
            history.FindAction(accepted.Action.ActionId) is not { IsSelfNativeResponseUse: true } recorded)
            return;
        var point = new CardUseHistoryPoint(recorded.ActualTurn, recorded.TurnOwner,
            history.CardUseHistoryCursor);
        var use = history.ReadCardUseFacts(point, CardUseHistoryView.ActualUses)
            .FirstOrDefault(fact => fact.ActionId == accepted.Action.ActionId);
        if (use is not null) ActualUse(use, timing);
    }

    private void ActualUse(CardUseHistoryFact use, SkillStateBoundaryTiming timing)
    {
        // The first declaration point survives target/role updates and repeated
        // observations, so this use cannot consume states applied afterwards.
        changes.ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.ActualUse,
            use.DeclarerSeat, timing, use.DeclarationCursor));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 253)]
    internal void Returning(ActualTurnCursorReturnedEvent returned) =>
        changes.ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.ActualTurnReturned,
            returned.OwnerSeat, SkillStateBoundaryTiming.Before, returned.ActualTurn));

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 253)]
    internal void Returned(ActualTurnCursorReturnedEvent returned) =>
        changes.ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.ActualTurnReturned,
            returned.OwnerSeat, SkillStateBoundaryTiming.After, returned.ActualTurn));
}
