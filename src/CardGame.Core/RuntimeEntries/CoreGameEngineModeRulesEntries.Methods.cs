#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineModeRulesEntries
{
    internal void AddModeDistanceBaseTerms(ProgramDistanceQuery query, List<RuleQueryBaseTerm> terms) => _resolve().AddModeDistanceBaseTerms(query, terms);
    internal void ApplyModeDeathOutcome(long deathFrameId, CharacterState victim, CharacterState? killer) => _resolve().ApplyModeDeathOutcome(deathFrameId, victim, killer);
    internal RuleQueryEvaluation ApplyModeNumericRules(CharacterState player, RuleQueryEvaluation ordinary, SkillRuleQuery query) => _resolve().ApplyModeNumericRules(player, ordinary, query);
    internal ModePlayerLife[] CaptureModeLife() => _resolve().CaptureModeLife();
    internal IMatchModeFactionPolicy ModeFactionPolicy { get => _resolve().ModeFactionPolicy; }
    internal IMatchModeSetupPresentation ModeSetupPresentation { get => _resolve().ModeSetupPresentation; }
}
