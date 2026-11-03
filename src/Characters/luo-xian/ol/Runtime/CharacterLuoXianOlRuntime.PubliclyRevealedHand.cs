#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuoXianOlRuntime
{
    internal bool TracksPublicHandReveals => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveDailiTurnEnd) == true;

    internal IEnumerable<PublicHandEntityRevealedEvent> CurrentPublicHandReveals() => !TracksPublicHandReveals ? [] : _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PublicHandEntityRevealedEvent>().GroupBy(e => (e.OwnerSeat, e.Card.Id, e.HandEntryMovementSequence)).Select(g => g.Last()).Where(e => _currentHandEntries.CurrentHandEntry(e.OwnerSeat, e.Card.Id) == e.HandEntryMovementSequence);
    internal int PubliclyRevealedHandCount(int owner) => CurrentPublicHandReveals().Count(e => e.OwnerSeat == owner);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26420)]
    internal void ObserveExplicitPublicHandReveal(IGameEvent payload)
    {
        if (!TracksPublicHandReveals)
            return;
        var (frame, ids) = _publicCardReveals.Describe(payload);
        foreach (var id in ids.Distinct().ToArray())
        {
            var location = _cardZonesOperations._cardZones.GetLocation(id);
            if (location is not { Zone: CardZoneKind.Hand, OwnerSeat: { } owner } || _currentHandEntries.CurrentHandEntry(owner, id)is not { } entry)
                continue;
            if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PublicHandEntityRevealedEvent>().Any(e => e.OwnerSeat == owner && e.Card.Id == id && e.HandEntryMovementSequence == entry))
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PublicHandEntityRevealedEvent(frame, owner, entry, _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))));
        }
    }
}
