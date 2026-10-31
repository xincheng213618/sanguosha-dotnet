#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangOriginalSkillRulesEntries
{
    internal bool AllowsWushuangOriginalActivation(ProgramActivationLegalityContext gate) => _resolve().AllowsWushuangOriginalActivation(gate);
    internal bool AllowsWushuangOriginalTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsWushuangOriginalTrigger(gate);
    internal void ExpireWushuangOriginalPolicies(IGameEvent payload) => _resolve().ExpireWushuangOriginalPolicies(payload);
    internal bool IsWushuangCiweiIneffective(CardUseFrame use) => _resolve().IsWushuangCiweiIneffective(use);
    internal void NullifyWushuangCiweiUse(ProgramSkillFrame f) => _resolve().NullifyWushuangCiweiUse(f);
    internal void RecordWushuangTairanRecovery(IGameEvent payload) => _resolve().RecordWushuangTairanRecovery(payload);
    internal void RestrictWushuangQianxiJudgmentColor(ProgramSkillFrame f) => _resolve().RestrictWushuangQianxiJudgmentColor(f);
    internal void SkipWushuangFenliPhase(ProgramSkillFrame f) => _resolve().SkipWushuangFenliPhase(f);
    internal GameEngine.ProgramCardIdentityMatch? WushuangChoufaIdentity(CharacterState owner, Card card) => _resolve().WushuangChoufaIdentity(owner, card);
    internal int WushuangOriginalDamage(GameEngine.IDamageAttempt attack) => _resolve().WushuangOriginalDamage(attack);
    internal bool WushuangYashiGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().WushuangYashiGrantQualified(owner, grant);
}
