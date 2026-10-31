#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBatch10MovementIntegrationEntries
{
    internal void CaptureMovementBatchStartXiaHuiOriginalSources(global::CardGame.Core.CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartXiaHuiOriginalSources(batch);
    internal global::System.Collections.Generic.IReadOnlyDictionary<int, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.XiaHuiMovementSource>>? CaptureXiaHuiOriginalSources(global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> sources) => _resolve().CaptureXiaHuiOriginalSources(sources);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectBuZhiMovementCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectBuZhiMovementCandidates(batch);
    internal void CollectWindow_CollectBuZhiMovementCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectBuZhiMovementCandidates(batch, result);
    internal bool ExcludeOrdinaryMovement_OfferNativeBatchHandGainOrCardLossBenefit(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_OfferNativeBatchHandGainOrCardLossBenefit(candidate, trigger);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryNativeBatchBenefit(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryNativeBatchBenefit(frame, candidate);
}
