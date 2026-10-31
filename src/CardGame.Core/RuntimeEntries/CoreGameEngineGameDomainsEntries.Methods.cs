#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGameDomainsEntries
{
    internal void CaptureFirstGameDomainCrossings(long batchId, int turn, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureFirstGameDomainCrossings(batchId, turn, movements);
    internal void CaptureMovementBatchFactsCaptureFirstGameDomainCrossings(global::CardGame.Core.CardMovementBatchBuilder batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureFirstGameDomainCrossings(batch, movements);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectFirstDomainCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectFirstDomainCandidates(batch);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateFirstDomainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateFirstDomainContext(frame, candidate);
    internal global::CardGame.Core.FirstGameDomainCrossingEvent? FirstDomainFact(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().FirstDomainFact(context);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateFirstDomainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateFirstDomainContext(frame, candidate);
    internal bool TracksFirstGameDomainCrossings { get => _resolve().TracksFirstGameDomainCrossings; }
}
