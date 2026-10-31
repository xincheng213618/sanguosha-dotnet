#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJudgedRankSlashPoliciesEntries
{
    internal bool AllowsRegisteredIsOtherActualBasicDiscardTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsRegisteredIsOtherActualBasicDiscardTrigger(gate);
    internal void AssertJudgedRankSlashPolicies() => _resolve().AssertJudgedRankSlashPolicies();
    internal bool CanRunOtherActualBasicDiscard(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunOtherActualBasicDiscard(candidate, context);
    internal global::CardGame.Core.TurnJudgedRankSlashPolicy? CurrentJudgedRankSlashPolicy(int actor) => _resolve().CurrentJudgedRankSlashPolicy(actor);
    internal bool DistancePhysicalSlash140HasIssuedJudgedRankSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash140HasIssuedJudgedRankSlashDistance(q);
    internal bool DistanceSpecificSlashExemption20HasJudgedRankSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceSpecificSlashExemption20HasJudgedRankSlashDistance(q);
    internal void DrawFromOriginalOtherBasicDiscard(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().DrawFromOriginalOtherBasicDiscard(supplied);
    internal void GrantOriginalJudgedRankSlashPolicy(global::CardGame.Core.ProgramSkillFrame supplied, string bind) => _resolve().GrantOriginalJudgedRankSlashPolicy(supplied, bind);
    internal bool HasIssuedJudgedRankSlashDistance(long? useId, int actor) => _resolve().HasIssuedJudgedRankSlashDistance(useId, actor);
    internal bool HasIssuedJudgedRankSlashQuota(long useId, int actor) => _resolve().HasIssuedJudgedRankSlashQuota(useId, actor);
    internal bool HasJudgedRankSlashDistance(int actor, global::CardGame.Core.CardKind kind, int? rank, long? existingUseFrameId = default) => _resolve().HasJudgedRankSlashDistance(actor, kind, rank, existingUseFrameId);
    internal bool HasJudgedRankSlashQuota(int actor, global::CardGame.Core.CardKind kind, int? rank) => _resolve().HasJudgedRankSlashQuota(actor, kind, rank);
    internal bool HasPotentialJudgedRankSlashQuota(int actor, global::CardGame.Core.CardKind kind) => _resolve().HasPotentialJudgedRankSlashQuota(actor, kind);
    internal bool JudgedRankSlashQuotaAfterLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().JudgedRankSlashQuotaAfterLimit(query);
    internal global::CardGame.Core.JudgedRankSlashThresholdSnapshot? JudgedRankSlashSnapshot(int actor) => _resolve().JudgedRankSlashSnapshot(actor);
    internal int[] MatchingOtherActualBasicDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().MatchingOtherActualBasicDiscardIndexes(batch, candidate);
    internal void ObserveJudgedRankSlashUse(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveJudgedRankSlashUse(payload);
    internal bool OtherActualBasicDiscardAlreadyIssued(int owner, string skill) => _resolve().OtherActualBasicDiscardAlreadyIssued(owner, skill);
    internal int[]? SelectWindowIndexes_MatchingOtherActualBasicDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingOtherActualBasicDiscardIndexes(batch, candidate, trigger);
    internal bool HasJudgedRankSlashCapability { get => _resolve().HasJudgedRankSlashCapability; }
    internal bool HasOtherActualBasicDiscardCapability { get => _resolve().HasOtherActualBasicDiscardCapability; }
}
