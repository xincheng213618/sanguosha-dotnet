#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJudgedRankSlashPoliciesEntries
{
    internal bool AllowsRegisteredIsOtherActualBasicDiscardTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsRegisteredIsOtherActualBasicDiscardTrigger(gate);
    internal bool CanRunOtherActualBasicDiscard(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanRunOtherActualBasicDiscard(candidate, context);
    internal TurnJudgedRankSlashPolicy? CurrentJudgedRankSlashPolicy(int actor) => _resolve().CurrentJudgedRankSlashPolicy(actor);
    internal bool DistancePhysicalSlash140HasIssuedJudgedRankSlashDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash140HasIssuedJudgedRankSlashDistance(q);
    internal bool DistanceSpecificSlashExemption20HasJudgedRankSlashDistance(ProgramSlashDistanceQuery q) => _resolve().DistanceSpecificSlashExemption20HasJudgedRankSlashDistance(q);
    internal void DrawFromOriginalOtherBasicDiscard(ProgramSkillFrame supplied) => _resolve().DrawFromOriginalOtherBasicDiscard(supplied);
    internal void GrantOriginalJudgedRankSlashPolicy(ProgramSkillFrame supplied, string bind) => _resolve().GrantOriginalJudgedRankSlashPolicy(supplied, bind);
    internal bool HasIssuedJudgedRankSlashDistance(long? useId, int actor) => _resolve().HasIssuedJudgedRankSlashDistance(useId, actor);
    internal bool HasIssuedJudgedRankSlashQuota(long useId, int actor) => _resolve().HasIssuedJudgedRankSlashQuota(useId, actor);
    internal bool HasJudgedRankSlashDistance(int actor, CardKind kind, int? rank, long? existingUseFrameId = default) => _resolve().HasJudgedRankSlashDistance(actor, kind, rank, existingUseFrameId);
    internal bool HasJudgedRankSlashQuota(int actor, CardKind kind, int? rank) => _resolve().HasJudgedRankSlashQuota(actor, kind, rank);
    internal bool HasPotentialJudgedRankSlashQuota(int actor, CardKind kind) => _resolve().HasPotentialJudgedRankSlashQuota(actor, kind);
    internal bool JudgedRankSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => _resolve().JudgedRankSlashQuotaAfterLimit(query);
    internal JudgedRankSlashThresholdSnapshot? JudgedRankSlashSnapshot(int actor) => _resolve().JudgedRankSlashSnapshot(actor);
    internal int[] MatchingOtherActualBasicDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().MatchingOtherActualBasicDiscardIndexes(batch, candidate);
    internal void ObserveJudgedRankSlashUse(IGameEvent payload) => _resolve().ObserveJudgedRankSlashUse(payload);
    internal bool OtherActualBasicDiscardAlreadyIssued(int owner, string skill) => _resolve().OtherActualBasicDiscardAlreadyIssued(owner, skill);
    internal int[]? SelectWindowIndexes_MatchingOtherActualBasicDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingOtherActualBasicDiscardIndexes(batch, candidate, trigger);
    internal bool HasJudgedRankSlashCapability { get => _resolve().HasJudgedRankSlashCapability; }
    internal bool HasOtherActualBasicDiscardCapability { get => _resolve().HasOtherActualBasicDiscardCapability; }
}
