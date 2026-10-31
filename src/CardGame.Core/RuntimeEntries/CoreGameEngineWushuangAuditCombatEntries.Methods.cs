#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangAuditCombatEntries
{
    internal bool AllowsWushuangCampClaim(ProgramTriggerLegalityContext gate) => _resolve().AllowsWushuangCampClaim(gate);
    internal SkillProgramStepOutcome CaptureWushuangWangong(ProgramSkillFrame supplied) => _resolve().CaptureWushuangWangong(supplied);
    internal SkillProgramStepOutcome ClaimWushuangCampHand(ProgramSkillFrame supplied) => _resolve().ClaimWushuangCampHand(supplied);
    internal bool HasWushuangWangongPrivilege(CharacterState actor, long? existingUseId = default) => _resolve().HasWushuangWangongPrivilege(actor, existingUseId);
    internal bool IsWangongModeSkill(string skillId) => _resolve().IsWangongModeSkill(skillId);
    internal bool IsWushuangCampClaimContext(ProgramSkillFrame frame) => _resolve().IsWushuangCampClaimContext(frame);
    internal CurrentTurnCardUseKindsRecordedEvent? PreviousWushuangActualUse(int actor, CardUseFrame? current = default) => _resolve().PreviousWushuangActualUse(actor, current);
    internal bool RetainWushuangCampClaim(ProgramSkillFrame frame) => _resolve().RetainWushuangCampClaim(frame);
    internal bool ReturnWushuangCampClaim(ProgramSkillFrame frame) => _resolve().ReturnWushuangCampClaim(frame);
    internal bool ValidWushuangCampClaim(ProgramSkillFrame frame) => _resolve().ValidWushuangCampClaim(frame);
    internal bool ValidWushuangWangongUse(CardUseFrame use) => _resolve().ValidWushuangWangongUse(use);
    internal int WangongPublicProposedBonus(GameSnapshot view, int sourceSeat, CardKind? kind) => _resolve().WangongPublicProposedBonus(view, sourceSeat, kind);
    internal int WushuangWangongDamage(GameEngine.IDamageAttempt attack) => _resolve().WushuangWangongDamage(attack);
    internal bool WushuangWangongDistance(ProgramSlashDistanceQuery query) => _resolve().WushuangWangongDistance(query);
    internal bool WushuangWangongQuota(ProgramSlashQuotaQuery query) => _resolve().WushuangWangongQuota(query);
}
