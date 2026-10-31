#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangOriginalSkillRulesEntries
{
    internal bool AllowsWushuangOriginalActivation(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsWushuangOriginalActivation(gate);
    internal bool AllowsWushuangOriginalTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsWushuangOriginalTrigger(gate);
    internal void ExpireWushuangOriginalPolicies(global::CardGame.Core.IGameEvent payload) => _resolve().ExpireWushuangOriginalPolicies(payload);
    internal bool IsWushuangCiweiIneffective(global::CardGame.Core.CardUseFrame use) => _resolve().IsWushuangCiweiIneffective(use);
    internal void NullifyWushuangCiweiUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NullifyWushuangCiweiUse(f);
    internal void RecordWushuangTairanRecovery(global::CardGame.Core.IGameEvent payload) => _resolve().RecordWushuangTairanRecovery(payload);
    internal void RestrictWushuangQianxiJudgmentColor(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RestrictWushuangQianxiJudgmentColor(f);
    internal void SkipWushuangFenliPhase(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SkipWushuangFenliPhase(f);
    internal global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WushuangChoufaIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().WushuangChoufaIdentity(owner, card);
    internal int WushuangOriginalDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().WushuangOriginalDamage(attack);
    internal bool WushuangYashiGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().WushuangYashiGrantQualified(owner, grant);
}
