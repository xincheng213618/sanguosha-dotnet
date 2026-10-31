#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGameDomainsEntries
{
    internal void CaptureFirstGameDomainCrossings(long batchId, int turn, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureFirstGameDomainCrossings(batchId, turn, movements);
    internal void CaptureMovementBatchFactsCaptureFirstGameDomainCrossings(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureFirstGameDomainCrossings(batch, movements);
    internal IEnumerable<ProgramTriggerCandidate> CollectFirstDomainCandidates(CardMovementBatchContext batch) => _resolve().CollectFirstDomainCandidates(batch);
    internal ProgramSkillWindowContext CreateFirstDomainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().CreateFirstDomainContext(frame, candidate);
    internal FirstGameDomainCrossingEvent? FirstDomainFact(ProgramSkillWindowContext context) => _resolve().FirstDomainFact(context);
    internal ProgramSkillWindowContext? TryCreateFirstDomainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateFirstDomainContext(frame, candidate);
    internal bool TracksFirstGameDomainCrossings { get => _resolve().TracksFirstGameDomainCrossings; }
}
