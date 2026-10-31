namespace CardGame.Core;

// Composition exposes dedicated contracts of this match's actual state owners.
// No engine reference, duplicate store or broad state bag exists.
internal sealed class MatchCoreCapabilityProvider
{
    internal IReadOnlyList<ProgramNativeCapabilityBinding> Bindings { get; }

    internal MatchCoreCapabilityProvider(CardZoneStore cards, IProgramEventHistoryQueries? history = null,
        CardMovementBatchStore? movements = null, ContentRegistry? content = null,
        IReadOnlyList<CharacterState>? players = null, IReadOnlyList<CardMovementRecord>? cardMovements = null,
        CardSnapshotCache? cardSnapshots = null, TurnCardUseEffectStore? turnEffects = null,
        IMatchRandomSource? random = null, SkillGrantRevisionStore? skillObservations = null)
    {
        ArgumentNullException.ThrowIfNull(cards);
        var bindings = new List<ProgramNativeCapabilityBinding>
        {
            new(typeof(IAtomicCardMovementZones), cards),
            new(typeof(ICardZoneQueries), cards),
            new(typeof(ICardDrawPileOrder), cards)
        };
        if (history is not null) bindings.Add(new(typeof(IProgramEventHistoryQueries), history));
        if (history is IProgramEventBoundaryQueries boundaries) bindings.Add(new(typeof(IProgramEventBoundaryQueries), boundaries));
        if (history is IProgramEventJournalQueries journal) bindings.Add(new(typeof(IProgramEventJournalQueries), journal));
        if (random is not null) bindings.Add(new(typeof(IMatchRandomSource), random));
        if (skillObservations is not null) bindings.Add(new(typeof(ISkillGrantRevisionObservations), skillObservations));
        if (movements is not null)
        {
            bindings.Add(new(typeof(ICardMovementBatchQueue), movements));
            bindings.Add(new(typeof(ICardMovementBatchNativeDrawInvoices), movements));
            bindings.Add(new(typeof(IGameStartingMovementBatchOrigins), movements));
            bindings.Add(new(typeof(ICardMovementEventSuppression), movements));
            bindings.Add(new(typeof(ICardMovementBatchLifecycle), movements));
        }
        if (content is not null) bindings.Add(new(typeof(IProgramContentQueries), content));
        if (cardSnapshots is not null) bindings.Add(new(typeof(ICardSnapshotCache), cardSnapshots));
        if (players is not null)
        {
            bindings.Add(new(typeof(IMatchRoleQueries), new MatchRoleQueries(players)));
            bindings.Add(new(typeof(IMatchParticipantStateAccess), new MatchParticipantStateAccess(players)));
        }
        if (turnEffects is not null)
        {
            bindings.Add(new(typeof(ITurnRuleModifierStore), turnEffects));
            bindings.Add(new(typeof(IActivePlayerTurnEffectQueries), turnEffects));
        }
        if (cardMovements is not null)
            bindings.Add(new(typeof(ICardMovementHistoryQueries), new CardMovementHistoryQueries(cardMovements)));
        if (players is not null && cardMovements is not null && history is not null)
            bindings.Add(new(typeof(INativeGivenCardQueries), new NativeGivenCardQueries(players, cardMovements, history)));
        Bindings = Array.AsReadOnly(bindings.ToArray());
    }
}
