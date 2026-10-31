#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangAuditCombatEntries
{
    internal bool AllowsWushuangCampClaim(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsWushuangCampClaim(gate);
    internal void AssertWushuangCampClaim(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertWushuangCampClaim(frame, plan);
    internal global::CardGame.Core.SkillProgramStepOutcome CaptureWushuangWangong(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().CaptureWushuangWangong(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome ClaimWushuangCampHand(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ClaimWushuangCampHand(supplied);
    internal bool HasWushuangWangongPrivilege(global::CardGame.Core.CharacterState actor, long? existingUseId = default) => _resolve().HasWushuangWangongPrivilege(actor, existingUseId);
    internal bool IsWangongModeSkill(string skillId) => _resolve().IsWangongModeSkill(skillId);
    internal bool IsWushuangCampClaimContext(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsWushuangCampClaimContext(frame);
    internal bool IsWushuangCampClaimMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsWushuangCampClaimMovement(frame, effect, pending);
    internal global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent? PreviousWushuangActualUse(int actor, global::CardGame.Core.CardUseFrame? current = default) => _resolve().PreviousWushuangActualUse(actor, current);
    internal bool RetainWushuangCampClaim(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RetainWushuangCampClaim(frame);
    internal bool ReturnWushuangCampClaim(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnWushuangCampClaim(frame);
    internal bool ValidWushuangCampClaim(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidWushuangCampClaim(frame);
    internal bool ValidWushuangWangongUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidWushuangWangongUse(use);
    internal int WangongPublicProposedBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, global::CardGame.Core.CardKind? kind) => _resolve().WangongPublicProposedBonus(view, sourceSeat, kind);
    internal int WushuangWangongDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().WushuangWangongDamage(attack);
    internal bool WushuangWangongDistance(global::CardGame.Core.ProgramSlashDistanceQuery query) => _resolve().WushuangWangongDistance(query);
    internal bool WushuangWangongQuota(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().WushuangWangongQuota(query);
}
