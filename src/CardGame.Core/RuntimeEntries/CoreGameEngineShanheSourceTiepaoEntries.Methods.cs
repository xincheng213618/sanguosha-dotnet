#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceTiepaoEntries
{
    internal PromptChoice AiSourceTiepao(PendingDecision p) => _resolve().AiSourceTiepao(p);
    internal void AnswerSourceTiepao(PromptChoice selected) => _resolve().AnswerSourceTiepao(selected);
    internal SkillProgramStepOutcome BeginShanheSourceTiepao(ProgramSkillFrame supplied) => _resolve().BeginShanheSourceTiepao(supplied);
    internal void BindSourceTiepaoUse(CardUseDeclaredEvent fact) => _resolve().BindSourceTiepaoUse(fact);
    internal bool CanOfferSourceTiepao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferSourceTiepao(c, t, w);
    internal bool ResumeSourceTiepao(long id) => _resolve().ResumeSourceTiepao(id);
    internal void ReturnSourceTiepaoUse(CardUseFrame use) => _resolve().ReturnSourceTiepaoUse(use);
    internal IReadOnlyList<PromptChoice> SourceTiepaoChoices(ProgramSkillFrame f) => _resolve().SourceTiepaoChoices(f);
    internal CardConversionSource SourceTiepaoConversion(ProgramSkillFrame f) => _resolve().SourceTiepaoConversion(f);
    internal bool SourceTiepaoDamage(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().SourceTiepaoDamage(c, w);
    internal void SourceTiepaoDistance(ProgramDistanceQuery q, List<RuleQueryBaseTerm> terms) => _resolve().SourceTiepaoDistance(q, terms);
    internal bool SourceTiepaoMaterialLegal(int owner, int target, Card card, CardConversionSource source) => _resolve().SourceTiepaoMaterialLegal(owner, target, card, source);
    internal Card[] SourceTiepaoMaterials(int owner, int target, CardConversionSource source) => _resolve().SourceTiepaoMaterials(owner, target, source);
    internal GameEngine.IDamageAttempt? SourceTiepaoOriginalAttack(DamageTriggerWindowFrame window, long damageId, int amount) => _resolve().SourceTiepaoOriginalAttack(window, damageId, amount);
}
