#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBatch10MovementIntegrationEntries
{
    internal void CaptureMovementBatchStartXiaHuiOriginalSources(CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartXiaHuiOriginalSources(batch);
    internal IReadOnlyDictionary<int, IReadOnlyList<XiaHuiMovementSource>>? CaptureXiaHuiOriginalSources(IEnumerable<CardLocation> sources) => _resolve().CaptureXiaHuiOriginalSources(sources);
    internal IEnumerable<ProgramTriggerCandidate> CollectBuZhiMovementCandidates(CardMovementBatchContext batch) => _resolve().CollectBuZhiMovementCandidates(batch);
    internal void CollectWindow_CollectBuZhiMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectBuZhiMovementCandidates(batch, result);
    internal bool ExcludeOrdinaryMovement_OfferNativeBatchHandGainOrCardLossBenefit(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_OfferNativeBatchHandGainOrCardLossBenefit(candidate, trigger);
    internal ProgramSkillWindowContext? TryNativeBatchBenefit(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryNativeBatchBenefit(frame, candidate);
}
