#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineThirdPartyHandGainsEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectKanggeHandGainCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectKanggeHandGainCandidates(batch);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectOwnerSourceHandGainCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectOwnerSourceHandGainCandidates(batch);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectThirdPartyHandGainCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectThirdPartyHandGainCandidates(batch);
    internal void CollectWindow_CollectKanggeHandGainCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectKanggeHandGainCandidates(batch, result);
    internal void CollectWindow_CollectOwnerSourceHandGainCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectOwnerSourceHandGainCandidates(batch, result);
    internal void CollectWindow_CollectThirdPartyHandGainCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectThirdPartyHandGainCandidates(batch, result);
    internal bool ExcludeOrdinaryMovement_IsOwnerSourceHandGainTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsOwnerSourceHandGainTrigger(candidate, trigger);
    internal bool ExcludeOrdinaryMovement_IsThirdPartyHandGainTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsThirdPartyHandGainTrigger(candidate, trigger);
}
