#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNeighborDiscardOpportunitiesEntries
{
    internal void CaptureMovementBatchFactsCaptureNeighborDiscardOpportunity(global::CardGame.Core.CardMovementBatchBuilder batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureNeighborDiscardOpportunity(batch, movements);
    internal void CaptureNeighborDiscardOpportunity(long batchId, int actualTurn, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureNeighborDiscardOpportunity(batchId, actualTurn, movements);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectNeighborDiscardCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectNeighborDiscardCandidates(batch);
    internal void CollectWindow_CollectNeighborDiscardCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectNeighborDiscardCandidates(batch, result);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateNeighborDiscardContext(global::CardGame.Core.CardsMovedTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateNeighborDiscardContext(window, candidate);
    internal bool ExcludeOrdinaryMovement_IsNeighborDiscardTopTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsNeighborDiscardTopTrigger(candidate, trigger);
    internal bool IsProgramDiscardTopMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementRecord move) => _resolve().IsProgramDiscardTopMovement(frame, move);
    internal bool MatchesFrozenOwnOrPreviousEnding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().MatchesFrozenOwnOrPreviousEnding(candidate, context);
    internal global::CardGame.Core.DiscardNeighborhoodFrozenEvent? NeighborDiscardFact(global::CardGame.Core.CardMovementBatchContext batch, int owner) => _resolve().NeighborDiscardFact(batch, owner);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> NeighborDiscardHistory() => _resolve().NeighborDiscardHistory();
    internal int[] NeighborDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, int owner) => _resolve().NeighborDiscardIndexes(batch, owner);
    internal int? NeighborDiscardSourceSeat(global::CardGame.Core.CardMovementBatchContext batch, int owner) => _resolve().NeighborDiscardSourceSeat(batch, owner);
    internal int PreviousLivingSeatFor(int owner) => _resolve().PreviousLivingSeatFor(owner);
    internal bool? ProveNeighborDiscardMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.CharacterState owner) => _resolve().ProveNeighborDiscardMovement(trigger, context, batch, owner);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateNeighborDiscardContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateNeighborDiscardContext(frame, candidate);
    internal bool TracksNeighborDiscardOpportunities { get => _resolve().TracksNeighborDiscardOpportunities; }
}
