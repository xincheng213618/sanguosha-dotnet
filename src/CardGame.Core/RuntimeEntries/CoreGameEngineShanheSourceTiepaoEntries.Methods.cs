#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceTiepaoEntries
{
    internal global::CardGame.Core.PromptChoice AiSourceTiepao(global::CardGame.Core.PendingDecision p) => _resolve().AiSourceTiepao(p);
    internal void AnswerSourceTiepao(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerSourceTiepao(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheSourceTiepao(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginShanheSourceTiepao(supplied);
    internal void BindSourceTiepaoUse(global::CardGame.Core.CardUseDeclaredEvent fact) => _resolve().BindSourceTiepaoUse(fact);
    internal bool CanOfferSourceTiepao(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferSourceTiepao(c, t, w);
    internal bool ResumeSourceTiepao(long id) => _resolve().ResumeSourceTiepao(id);
    internal void ReturnSourceTiepaoUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnSourceTiepaoUse(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SourceTiepaoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceTiepaoChoices(f);
    internal global::CardGame.Core.CardConversionSource SourceTiepaoConversion(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceTiepaoConversion(f);
    internal bool SourceTiepaoDamage(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().SourceTiepaoDamage(c, w);
    internal void SourceTiepaoDistance(global::CardGame.Core.ProgramDistanceQuery q, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryBaseTerm> terms) => _resolve().SourceTiepaoDistance(q, terms);
    internal bool SourceTiepaoMaterialLegal(int owner, int target, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource source) => _resolve().SourceTiepaoMaterialLegal(owner, target, card, source);
    internal global::CardGame.Core.Card[] SourceTiepaoMaterials(int owner, int target, global::CardGame.Core.CardConversionSource source) => _resolve().SourceTiepaoMaterials(owner, target, source);
    internal global::CardGame.Core.GameEngine.IDamageAttempt? SourceTiepaoOriginalAttack(global::CardGame.Core.DamageTriggerWindowFrame window, long damageId, int amount) => _resolve().SourceTiepaoOriginalAttack(window, damageId, amount);
}
