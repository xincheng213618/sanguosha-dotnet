#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCurrentTurnShownCardBenefitsRuntime
{
    internal bool HasShownCardTurnCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.GrantCurrentTurnDirectedHeartSlashBonus) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GrantCurrentTurnDirectedHeartSlashBonus);

    internal void RevealOwnedBoundCardAppearance(ProgramSkillFrame frame, string bind)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var set = _coreProgramLifecycleOperations.GetProgramCardSet(active, bind);
        if (set.CardIds.Count == 0)
            return;
        if (set.CardIds is not [var id] || set.SourceLocations is not [var source] || source.OwnerSeat is not { } owner || source != CardLocation.Hand(owner))
            throw new InvalidOperationException("Shown appearance requires one real owned Hand entity.");
        if (!_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || _cardZonesOperations._cardZones.GetLocation(id) != source)
        {
            _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, bind, [], SkillProgramCardSetVisibility.Public, []);
            return;
        }

        var card = _cardZonesOperations._cardZones.CardsAt(source).Single(c => c.Id == id);
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], card);
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { CardSetBindings = Array.AsReadOnly(active.CardSetBindings.Select(b => b.Name == bind ? b with { Visibility = SkillProgramCardSetVisibility.Public, FrozenRevealedSuit = suit } : b).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), owner, bind, Array.AsReadOnly(new[] { _coreSnapshotValueProjectionOperations.ToSnapshot(card) })));
    }

    internal void GrantCurrentTurnDirectedHeartSlashBonus(ProgramSkillFrame frame, int targetSeat)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || targetSeat == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
            throw new InvalidOperationException("Directed Heart Slash benefit requires a living other target.");
        var fact = new CurrentTurnDirectedHeartSlashBonus(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), targetSeat);
        var prior = _coreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess._currentTurnHeartSlashBonuses.SingleOrDefault(b => b.ParentFrameId == fact.ParentFrameId && b.EffectIndex == fact.EffectIndex);
        if (prior is not null)
        {
            if (prior != fact)
                throw new InvalidOperationException("Directed benefit changed identity.");
            return;
        }

        _coreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess._currentTurnHeartSlashBonuses.Add(fact);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentTurnDirectedHeartSlashBonusGrantedEvent(fact));
    }

    [ProgramDamageHook(ProgramDamagePhase.FactModifiers, 20)]
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> GetCurrentTurnHeartSlashBonuses(GameEngine.IDamageAttempt attack)
    {
        if (attack.IsChainPropagation || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return[];
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == attack.ResolutionId);
        if (use?.Action is not { Type: CardActionType.Use, EffectiveSuit: Suit.Heart } action)
            return[];
        return _coreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess._currentTurnHeartSlashBonuses.Where(b => b.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && b.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && b.Source.OwnerSeat == action.ActorSeat && b.Source.OwnerSeat == attack.CardUserSeat && b.TargetSeat == attack.TargetSeat).GroupBy(b => (b.Source.OwnerSeat, b.Source.SkillId, b.TargetSeat)).Select(group => (group.First().Source, 1));
    }

    internal bool HasSlashUseDistanceBySuit(CharacterState owner, CardKind kind, Suit? suit) => suit is { } value && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetCardPolicies(SkillProgramCardPolicyKind.IgnoreSlashUseDistanceBySuit).Any(item => item.Policy.InputSuit == value && ProgramCardKindMatch.Contains(item.Policy.CardKinds, kind) && item.Policy.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(owner)));
    void ICurrentTurnShownCardProgramHost.RevealOwnedBoundCardAppearance(ProgramSkillFrame f, string bind) => RevealOwnedBoundCardAppearance(f, bind);
    void ICurrentTurnShownCardProgramHost.GrantCurrentTurnDirectedHeartSlashBonus(ProgramSkillFrame f, int seat) => GrantCurrentTurnDirectedHeartSlashBonus(f, seat);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 170)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 60)]
    internal bool DistancePhysicalSlash170HasSlashUseDistanceBySuit(ProgramSlashDistanceQuery q) => HasSlashUseDistanceBySuit(q.Actor, q.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(q.Actor, q.PhysicalCard!));
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 70)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 40)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 100)]
    internal bool DistanceVirtualSlash70HasSlashUseDistanceBySuit(ProgramSlashDistanceQuery q) => HasSlashUseDistanceBySuit(q.Actor, q.Kind, q.PhysicalSuit);
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 50)]
    internal bool DistanceProvidedSlash50HasSlashUseDistanceBySuit(ProgramSlashDistanceQuery q) => q.AllowAnyPhysicalSuit && HasSlashUseDistanceBySuit(q.Actor, q.Kind, Suit.Diamond);
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 110)]
    internal bool DistanceBorrowedSlash110HasSlashUseDistanceBySuit(ProgramSlashDistanceQuery q) => q.AllowAnyPhysicalSuit && _coreActionQueriesOperations.GetSlashUseCards(q.Actor).Any(c => HasSlashUseDistanceBySuit(q.Actor, q.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(q.Actor, c)));
}
