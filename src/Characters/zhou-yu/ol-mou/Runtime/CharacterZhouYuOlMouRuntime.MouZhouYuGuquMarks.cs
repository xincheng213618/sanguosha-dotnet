#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuOlMouRuntime
{
    internal const string MouGuquSkill = "mou-ol:guqu";
    internal const string MouGuquBinding = "completed-que-use";
    internal bool HasMouGuquCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouZhouYuGuqu);
    internal bool MouGuquTurnOpen => HasMouGuquCapability && _coreMatchSetupSetupCompleteAccess._setupComplete && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnEndedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);

    internal IReadOnlyList<(CardConversionSource Source, string Hash)> CurrentMouGuquSources()
    {
        if (!MouGuquTurnOpen || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat].IsAlive)
            return[];
        return _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat]).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCompleted).Where(b => b.SkillId == MouGuquSkill && b.Trigger.Id == MouGuquBinding && b.Trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveMouZhouYuGuqu }]).Select(b => (new CardConversionSource(b.SkillId, b.Trigger.Id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, b.SkillInstanceId), b.Program.GameplayHash)).Distinct().ToArray();
    }

    internal bool IsCurrentMouGuquSource(CardConversionSource source, string hash) => CurrentMouGuquSources().Any(s => s.Source == source && s.Hash == hash);
    internal bool IsMouGuquQueForSource(CardConversionSource source, int cardId) => MouGuquTurnOpen && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuquQueMarkedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.Source == source && e.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.GameplayHash && e.CardId == cardId);
    // Registration only; the query still belongs to legacy GameEngine.MouZhouYuGuqu.Marks.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 210)]
    internal void ProjectMouGuquCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(HasMouGuquEntityMark(cardId) ? "阙" : null);
    }

    internal bool HasMouGuquEntityMark(int cardId) => HasMouGuquCapability && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuquQueMarkedEvent>().Any(e => e.CardId == cardId && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) && CurrentMouGuquSources().Any(s => IsMouGuquQueForSource(s.Source, cardId));
    internal bool HasMouGuquHandEntityMark(int holder, int cardId) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(holder) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder]).Any(c => c.Id == cardId) && HasMouGuquEntityMark(cardId);
    internal IReadOnlyList<(CardConversionSource Source, string Hash)> MouGuquRestrictingSources(int actor) => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || !HasMouGuquCapability || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuquQueMarkedEvent>().Any() ? [] : CurrentMouGuquSources().Where(s => s.Source.OwnerSeat != actor && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[actor]).Any(c => IsMouGuquQueForSource(s.Source, c.Id))).ToArray();
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 29021)]
    internal void CaptureMouGuquPublicHandReveals(IGameEvent payload)
    {
        if (!HasMouGuquCapability)
            return;
        var(frame, ids) = payload switch
        {
            CardsRevealedEvent e => (e.ResolutionId, e.Cards.Select(c => c.Id).ToArray()),
            ProgramCardsRevealedEvent e => (e.FrameId, e.Cards.Select(c => c.Id).ToArray()),
            FireAttackCardRevealedEvent e => (e.ResolutionId, new[] { e.CardId }),
            ProgramHandComparisonCardRevealedEvent e => (e.FrameId, new[] { e.CardId }),
            ProgramHandComparisonSourceRevealedEvent e => (e.FrameId, new[] { e.CardId }),
            ProgramHandDraftRevealedEvent e => (e.FrameId, _characterWangGuanOlOperations.MiuyanDraftHandIds(e).ToArray()),
            ProgramQingliangRevealEvent e => (e.FrameId, _characterWangGuanOlOperations.MiuyanQingliangHandIds(e).ToArray()),
            ProgramJuanxiaTrickUsedEvent { TrickKind: "FireAttack", CardId: > 0 } e => (e.FrameId, _characterWangGuanOlOperations.MiuyanJuanxiaHandIds(e).ToArray()),
            _ => (0L, Array.Empty<int>())};
        if (ids.Length == 0)
            return;
        var sources = CurrentMouGuquSources();
        if (sources.Count == 0)
            return;
        var shown = ids.ToHashSet();
        // A public show can carry deck/equipment/host-only cards too. Only
        // entities actually in a real player's Hand at this exact show qualify.
        // Private look/peek, ordinary Use/Played and pindian reveal are distinct.
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(player).Where(c => shown.Contains(c.Id)))
                foreach (var source in sources)
                {
                    if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuquQueMarkedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.Source == source.Source && e.GameplayHash == source.Hash && e.CardId == card.Id))
                        continue;
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuquQueMarkedEvent(source.Source, source.Hash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, frame, player.Seat, card.Id, card.Kind, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
                }
    }
}
