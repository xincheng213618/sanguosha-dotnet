#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNumericRuleAurasEntries
{
    internal IReadOnlyList<RuleQueryContribution> CollectOtherPublicTeamNumericRuleContributions(CharacterState recipient, SkillRuleQuery query) => _resolve().CollectOtherPublicTeamNumericRuleContributions(recipient, query);
}
