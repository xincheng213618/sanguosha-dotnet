#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineThirdPartyHandGainsEntries
{
    internal IEnumerable<ProgramTriggerCandidate> CollectKanggeHandGainCandidates(CardMovementBatchContext batch) => _resolve().CollectKanggeHandGainCandidates(batch);
    internal IEnumerable<ProgramTriggerCandidate> CollectOwnerSourceHandGainCandidates(CardMovementBatchContext batch) => _resolve().CollectOwnerSourceHandGainCandidates(batch);
    internal IEnumerable<ProgramTriggerCandidate> CollectThirdPartyHandGainCandidates(CardMovementBatchContext batch) => _resolve().CollectThirdPartyHandGainCandidates(batch);
    internal void CollectWindow_CollectKanggeHandGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectKanggeHandGainCandidates(batch, result);
    internal void CollectWindow_CollectOwnerSourceHandGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectOwnerSourceHandGainCandidates(batch, result);
    internal void CollectWindow_CollectThirdPartyHandGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectThirdPartyHandGainCandidates(batch, result);
    internal bool ExcludeOrdinaryMovement_IsOwnerSourceHandGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsOwnerSourceHandGainTrigger(candidate, trigger);
    internal bool ExcludeOrdinaryMovement_IsThirdPartyHandGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsThirdPartyHandGainTrigger(candidate, trigger);
}
