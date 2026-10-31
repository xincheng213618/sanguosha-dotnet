#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNumericRuleAurasRuntime
{
    internal IReadOnlyList<RuleQueryContribution> CollectOtherPublicTeamNumericRuleContributions(CharacterState recipient, SkillRuleQuery query)
    {
        // Only bindings are cached. Public relationships and life state belong
        // to this query, before the native producer freezes its requested count.
        if (query is not (SkillRuleQuery.DrawCount or SkillRuleQuery.SlashLimit) || !recipient.IsAlive || !recipient.TeamRevealed || recipient.TeamId is null)
            return Array.Empty<RuleQueryContribution>();
        var contributions = new List<RuleQueryContribution>();
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players)
        {
            if (!_coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(owner, recipient))
                continue;
            var bindings = _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetNumericModifiers(query).Where(binding => binding.Modifier.RecipientScope == SkillRuleRecipientScope.OtherPublicTeamAllies).ToArray();
            if (bindings.Length == 0)
                continue;
            // Owner stays the actual skill holder, so identities from separate
            // owners and separate acquired instances cannot collapse together.
            var context = new SkillProgramRuleContext(_coreNativeSkillContextQueriesOperations.CreateSkillContext(owner), _coreRuleQueriesOperations.GetLivingFactionCount());
            contributions.AddRange(SkillProgramRules.CollectIndexedContributions(query, context, bindings, SkillRuleRecipientScope.OtherPublicTeamAllies));
        }

        return Array.AsReadOnly(contributions.ToArray());
    }
}
