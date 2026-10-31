#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiWanOlRuntime
{
    internal int LianjuEndingSerial => _coreProgramEventHistoryOperations.ProgramEventHistory<LianjuEndingReachedEvent>().LastOrDefault()?.Serial ?? 0;

    internal void RecordLianjuEntityUse(int actor, long? action, long nativeUse, int card)
    {
        if (card <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber < 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<LianjuEntityUsedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.ActionId == action && e.UseFrameId == nativeUse && e.CardId == card))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LianjuEntityUsedEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, actor, action, nativeUse, card));
    }

    internal IReadOnlyList<int> LianjuEligibleCards(int actor, bool red) => Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<LianjuEntityUsedEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && LianjuFinalUseActor(e) == actor).Select(e => e.CardId).Distinct().Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DiscardPile && CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Suit) == red).Order().ToArray());
    internal LianjuPromiseGrantedEvent[] LianjuDuePromises()
    {
        var endings = _coreProgramEventHistoryOperations.ProgramEventHistory<LianjuEndingReachedEvent>().ToArray();
        return _coreProgramEventHistoryOperations.ProgramEventHistory<LianjuPromiseGrantedEvent>().Where(p => endings.FirstOrDefault(e => e.Serial > p.EndingSerial && e.ActorSeat == p.RecipientSeat)?.Serial == LianjuEndingSerial).OrderBy(p => p.FrameId).ToArray();
    }

    internal void AppendLianjuEndingItems(List<TurnEndingBoundaryItem> items)
    {
        if (!HasLiWanCapability)
            return;
        items.RemoveAll(item => item.Candidate is { } c && _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveLianjuOppositeColorReturn));
        var due = LianjuDuePromises();
        for (var i = 0; i < due.Length; i++)
        {
            var p = due[i];
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<LianjuPromiseConsumedEvent>().Any(e => e.PromiseFrameId == p.FrameId) || !_coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat].IsAlive || _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program is not { } program || program.GameplayHash != p.GameplayHash)
                continue;
            var trigger = program.Triggers.Single(t => t.Effects is [{ Op: SkillProgramEffectOp.ResolveLianjuOppositeColorReturn }]);
            items.Add(new(TurnEndingBoundaryItemKind.Program, trigger.Priority, $"lianju:{(p.FrameId)}", new(p.Source.OwnerSeat, p.Source.SkillId, trigger.Id, p.Source.SkillInstanceId, p.GameplayHash, trigger.Priority, i), _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat])));
        }
    }
}
