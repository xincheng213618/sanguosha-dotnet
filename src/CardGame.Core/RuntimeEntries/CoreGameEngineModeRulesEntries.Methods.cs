#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineModeRulesEntries
{
    internal void AddModeDistanceBaseTerms(global::CardGame.Core.ProgramDistanceQuery query, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryBaseTerm> terms) => _resolve().AddModeDistanceBaseTerms(query, terms);
    internal void ApplyModeDeathOutcome(long deathFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer) => _resolve().ApplyModeDeathOutcome(deathFrameId, victim, killer);
    internal global::CardGame.Core.RuleQueryEvaluation ApplyModeNumericRules(global::CardGame.Core.CharacterState player, global::CardGame.Core.RuleQueryEvaluation ordinary, global::CardGame.Core.SkillRuleQuery query) => _resolve().ApplyModeNumericRules(player, ordinary, query);
    internal global::CardGame.Core.ModePlayerLife[] CaptureModeLife() => _resolve().CaptureModeLife();
    internal global::CardGame.Core.IMatchModeFactionPolicy ModeFactionPolicy { get => _resolve().ModeFactionPolicy; }
    internal global::CardGame.Core.IMatchModeSetupPresentation ModeSetupPresentation { get => _resolve().ModeSetupPresentation; }
}
