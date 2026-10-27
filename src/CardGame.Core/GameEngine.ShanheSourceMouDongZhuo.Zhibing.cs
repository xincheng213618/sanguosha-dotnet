namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 62055)]
    private void RecordSourceMouActualUse(IGameEvent payload)
    {
        if (!IsSourceMouMode || !_contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheSourceZhibing) || !HasOngoingActualUseTurn) return;
        CardActionContext? action = null; long origin = 0;
        if (payload is CardUseDeclaredEvent declared && LifecycleCardUse(declared.ResolutionId) is { Action: { Type: CardActionType.Use } a } use &&
            use.SourceSeat == declared.SourceSeat && use.CardId == declared.CardId && use.CardKind == declared.CardKind && a.ActorSeat == use.SourceSeat && a.EffectiveKind == use.CardKind)
        { action = a; origin = use.Id; }
        else if (payload is CardActionAcceptedEvent { Action.Type: CardActionType.Response } response)
        {
            // The earlier shared true-Use observer proves Dodge against Slash
            // and Nullification. A pure played Slash/Dodge has no such fact.
            var fact = ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().LastOrDefault(e =>
                e.CardActionId == response.Action.ActionId && e.ActorSeat == response.Action.ActorSeat && e.RoundActionType == CardActionType.Response);
            if (fact is not null) { action = response.Action; origin = fact.OriginFrameId; }
        }
        if (action is null || action.ActionId <= 0 || ProgramEventHistory<ShanheSourceMouBlackUseEvent>().Any(e => e.ActionId == action.ActionId)) return;
        // NativeActionAppearance is required by the descriptor, so physical
        // uses must already carry their pre-payment effective appearance.
        // Never re-evaluate a lost equipment/skill after the use was paid.
        if (action.EffectiveSuit is null && action.PhysicalCards.Count == 1)
            throw new InvalidOperationException("Source Zhibing requires the actual use's frozen pre-payment suit.");
        var suit = action.EffectiveSuit ?? Suit.None;
        // Different-suit multi-card conversions legitimately have no suit;
        // their original payment can still freeze one common effective color.
        var black = action.EffectiveIsRed is { } red ? !red : suit is Suit.Spade or Suit.Club;
        AdvanceEventRulesAndQueueFact(new ShanheSourceMouBlackUseEvent(action.ActionId, origin, action.ActorSeat, _turnNumber, _currentSeat, action.EffectiveKind, suit, black));
    }
    private (ShanheSourceMouBlackUseEvent Use, int Ordinal)? SourceMouThreshold(int owner, ProgramSkillWindowContext context)
    {
        if (!IsSourceMouMode || !HasOngoingActualUseTurn || context.Window != SkillProgramTriggerWindow.CardUseCommitted || context.CardUse is not { } action || action.ActorSeat == owner) return null;
        var history = ProgramEventHistory<ShanheSourceMouBlackUseEvent>().ToArray();
        var actual = history.SingleOrDefault(e => e.ActionId == action.CardActionId && e.Actor == action.ActorSeat && e.Turn == _turnNumber && e.TurnOwner == _currentSeat);
        if (actual is not { Black: true }) return null;
        // ActionId proves identity, never chronology. Freeze the native fact
        // prefix ending at this actual submission, excluding later nested uses.
        var index = Array.FindIndex(history, e => e == actual);
        var ordinal = history.Take(index + 1).Count(e => e.Actor == actual.Actor && e.Turn == actual.Turn && e.TurnOwner == actual.TurnOwner && e.Black);
        if (ordinal is not (3 or 6 or 9) || ProgramEventHistory<ShanheSourceMouThresholdEvent>().Any(e => e.Owner == owner && e.ActionId == actual.ActionId)) return null;
        return (actual, ordinal);
    }
    private void ApplySourceMouThreshold(ProgramSkillFrame f)
    {
        var r = f.ShanheSourceMou!;
        if (r.Operation != SkillProgramEffectOp.ResolveShanheSourceZhibing || SourceMouThreshold(f.OwnerSeat, f.WindowContext!) is not { } threshold ||
            threshold.Use.ActionId != r.ActionId || threshold.Use.Actor != r.Actor || threshold.Ordinal != r.Ordinal)
            throw new InvalidOperationException("Source Zhibing lost its exact other actor's actual black Use threshold.");
        AdvanceEventRulesAndQueueFact(new ShanheSourceMouThresholdEvent(f.Id, f.OwnerSeat, r.Actor, r.Turn, r.Ordinal, r.ActionId));
        if (r.Ordinal == 3)
        {
            if (_players[r.Actor].IsAlive) new ProgramSkillHost(this).LoseHp(f.Id, f.SkillId, r.Actor, 1);
        }
        else if (r.Ordinal == 6)
        {
            var count = GetHand(_players[f.OwnerSeat]).Count;
            if (count < 9)
            {
                ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Before = _movementSequence, After = _movementSequence },
                    PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                DrawProgramCards(f.Id, f.OwnerSeat, 9 - count, null, null, SkillProgramCardSetVisibility.Private, new("skill.shanhe-source-zhibing.draw"));
                var active = GetActiveProgramFrame(f.Id);
                ReplaceRuntimeTop(active with { ShanheSourceMou = active.ShanheSourceMou! with { After = _movementSequence } });
            }
            else if (count > 9) ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { ShanheSourceMou = r with { Stage = ShanheSourceMouStage.HandDiscardChoice, Required = count - 9, Payer = f.OwnerSeat } });
        }
        else if (_players[r.Actor].IsAlive)
            GrantIssuedProgramTurnSkills(f, r.Actor, [SourceMouZhixi], new(_turnNumber, _currentSeat));
    }
    private bool HasSourceMouZhixi(int seat) => IsSourceMouMode && IsValidPlayerSeat(seat) && HasRuntimeSkill(_players[seat], SourceMouZhixi);
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 62055)]
    private bool SourceMouZhixiUseForbidden(ProgramCardUseQuery query) => HasSourceMouZhixi(query.ActorSeat) && query.ExistingUseFrameId is null &&
        (query.ActionType == CardActionType.Use || query.ActionType == CardActionType.Response &&
            (query.Kind == CardKind.Nullification || IsProgramResponseCardUse(_players[query.ActorSeat], query.Kind)));
    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 62055)]
    private bool SourceMouZhixiNativeUseForbidden(ProgramNativeResponseQuery query) => HasSourceMouZhixi(query.Provider.Seat) &&
        (query.DyingUse || query.Kind == CardKind.Nullification || query.Kind == CardKind.Dodge && IsProgramResponseCardUse(query.Provider, query.Kind));
}
