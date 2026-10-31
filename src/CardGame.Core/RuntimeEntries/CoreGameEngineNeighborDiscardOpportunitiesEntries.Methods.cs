#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNeighborDiscardOpportunitiesEntries
{
    internal void CaptureMovementBatchFactsCaptureNeighborDiscardOpportunity(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureNeighborDiscardOpportunity(batch, movements);
    internal void CaptureNeighborDiscardOpportunity(long batchId, int actualTurn, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureNeighborDiscardOpportunity(batchId, actualTurn, movements);
    internal IEnumerable<ProgramTriggerCandidate> CollectNeighborDiscardCandidates(CardMovementBatchContext batch) => _resolve().CollectNeighborDiscardCandidates(batch);
    internal void CollectWindow_CollectNeighborDiscardCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectNeighborDiscardCandidates(batch, result);
    internal ProgramSkillWindowContext CreateNeighborDiscardContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate candidate) => _resolve().CreateNeighborDiscardContext(window, candidate);
    internal bool ExcludeOrdinaryMovement_IsNeighborDiscardTopTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsNeighborDiscardTopTrigger(candidate, trigger);
    internal bool IsProgramDiscardTopMovement(ProgramSkillFrame frame, CardMovementRecord move) => _resolve().IsProgramDiscardTopMovement(frame, move);
    internal bool MatchesFrozenOwnOrPreviousEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().MatchesFrozenOwnOrPreviousEnding(candidate, context);
    internal DiscardNeighborhoodFrozenEvent? NeighborDiscardFact(CardMovementBatchContext batch, int owner) => _resolve().NeighborDiscardFact(batch, owner);
    internal IEnumerable<IGameEvent> NeighborDiscardHistory() => _resolve().NeighborDiscardHistory();
    internal int[] NeighborDiscardIndexes(CardMovementBatchContext batch, int owner) => _resolve().NeighborDiscardIndexes(batch, owner);
    internal int? NeighborDiscardSourceSeat(CardMovementBatchContext batch, int owner) => _resolve().NeighborDiscardSourceSeat(batch, owner);
    internal int PreviousLivingSeatFor(int owner) => _resolve().PreviousLivingSeatFor(owner);
    internal bool? ProveNeighborDiscardMovement(SkillProgramTrigger trigger, ProgramSkillWindowContext context, CardMovementBatchContext batch, CharacterState owner) => _resolve().ProveNeighborDiscardMovement(trigger, context, batch, owner);
    internal ProgramSkillWindowContext? TryCreateNeighborDiscardContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateNeighborDiscardContext(frame, candidate);
    internal bool TracksNeighborDiscardOpportunities { get => _resolve().TracksNeighborDiscardOpportunities; }
}
