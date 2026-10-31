#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTurnHandCategoryRestrictionsRuntime
{
    internal void GrantDamageSourceHandCategory(ProgramSkillFrame f, SkillProgramCardCategory category)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, TargetSeat: { } victim } c || victim != f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source) || !_coreMatchParticipantsPlayersAccess._players[source].IsAlive || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not { } damage || damage.Id != c.ParentFrameId || damage.ParentFrameId != c.DamageFrameId || _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(damage).IsSourceLess || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2].Id != damage.Id || damage.CandidateIndex < 0 || damage.CandidateIndex >= damage.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, damage.Candidates[damage.CandidateIndex].ToProgramCandidate()))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "伤害来源已不存在，未发行手牌类别限制。");
            return;
        }

        var policy = _turnCardUseEffectsOperations._turnCardUseEffects.GrantHandCategoryRestriction(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, f.Id, damage.Id, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), source, category);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HandCategoryRestrictionGrantedEvent(policy));
    }

    internal bool IsTurnHandCategoryRestricted(CharacterState owner, Card card) => _turnCardUseEffectsOperations._turnCardUseEffects.HasHandCategoryRestriction(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, owner.Seat) && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Hand(owner.Seat) && _turnCardUseEffectsOperations._turnCardUseEffects.IsHandCategoryRestricted(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, owner.Seat, CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(owner, card)));
    internal bool IsHandCategoryMaterialRestricted(IReadOnlyList<int> ids) => ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id)is { Zone: CardZoneKind.Hand, OwnerSeat: { } owner } from && (_coreProgramCardLegalityOperations.IsProgramHandPhysicalRestricted(owner, id) || IsTurnHandCategoryRestricted(_coreMatchParticipantsPlayersAccess._players[owner], _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == id))));
    internal bool IsSelfHandCategoryDiscardForbidden(int actor, Card card, CardLocation from, OwnedCardMoveIntent intent) => _closedOwnedCardMoveQueriesSource.Get().IsSelfHandCategoryDiscardForbidden(actor, card, from, intent);
    internal IReadOnlyList<Card> GetSelfDiscardableHand(CharacterState owner) => _coreDeferredCardsOperations.GetDiscardEligibleHand(owner).Where(c => !IsSelfHandCategoryDiscardForbidden(owner.Seat, c, CardLocation.Hand(owner.Seat), OwnedCardMoveIntent.Discard)).ToArray();
    internal IReadOnlyList<Card> GetSelfDiscardableOwnedHand(CharacterState owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(c => !IsSelfHandCategoryDiscardForbidden(owner.Seat, c, CardLocation.Hand(owner.Seat), OwnedCardMoveIntent.Discard)).ToArray();
    internal int RequiredSelfHandLimitDiscard(CharacterState owner)
    {
        var excess = Math.Max(0, _coreDeferredCardsOperations.GetDiscardEligibleHand(owner).Count - _coreNativeRuleQueriesOperations.GetHandLimit(owner));
        if (excess == 0)
            return 0;
        // Counted cards and self-discardable cards remain different sets. Only
        // the amount owed changes, then the actual legal payment caps it.
        return (int)Math.Min((long)excess * _corePlannedPhaseQuantitiesOperations.GetDiscardQuotaMultiplier(owner), GetSelfDiscardableHand(owner).Count);
    }

    internal IReadOnlyList<TurnHandCategoryRestriction>? GetTurnHandCategoryRestrictionSnapshot(int seat)
    {
        var policies = _turnCardUseEffectsOperations._turnCardUseEffects.HandCategoryRestrictions.Where(p => p.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && p.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && p.AffectedSeat == seat).ToArray();
        return policies.Length == 0 ? null : Array.AsReadOnly(policies);
    }

    internal bool ProgramSelectionWillSelfDiscard(ProgramSkillFrame f, string bind)
    {
        var instructions = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions;
        // Do not infer an unchosen future discard branch from the binding name.
        // Other conditional destinations are revalidated with the frozen actual
        // selection actor by MoveProgramBoundCards only when that node executes.
        return instructions.Any(e => e.Op == SkillProgramEffectOp.DiscardEquipmentAndResolveJiqiao && e.SourceBind == bind) || instructions.Any(e => e.Op == SkillProgramEffectOp.MoveBoundCards && e.SourceBind == bind && e.Destination == SkillProgramCardDestination.DiscardPile && (e.Condition.Kind == SkillProgramConditionKind.Always || e.Condition.Kind == SkillProgramConditionKind.ChoiceIs && f.ChoiceBindings.Any(c => c.Name == e.Condition.SourceBind && c.OptionId == e.Condition.OptionId) || e.Condition.Kind == SkillProgramConditionKind.JudgmentOutcomeMatchesSuits && f.JudgmentOutcomeBindings?.Any(outcome => outcome.Name == e.Condition.SourceBind && outcome.Suit is { } suit && e.Condition.Suits.Contains(suit)) == true));
    }

    internal bool CanSelectSelfDiscardBinding(ProgramSkillFrame f, string bind, int selectionActorSeat, Card card, CardLocation location) => !ProgramSelectionWillSelfDiscard(f, bind) || !IsSelfHandCategoryDiscardForbidden(selectionActorSeat, card, location, OwnedCardMoveIntent.Discard);
}
