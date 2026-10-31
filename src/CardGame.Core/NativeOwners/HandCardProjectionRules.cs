#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HandCardProjectionRules(Func<RuntimeEntries.CoreGameEngineActionQueriesEntries> getCoreGameEngineActionQueriesRuntime, Func<RuntimeEntries.CharacterGuanYuWumiaoEntries> getCharacterGuanYuWumiaoRuntime, Func<ProgramPipelineHooks> getProgramPipelineHooks) : IHandCardProjectionRules
{
    public HandCardIdentityProjection? FirstIdentity(CharacterState owner, Card card)
    {
        var identity = getCoreGameEngineActionQueriesRuntime().GetProgramCardIdentityMatches(owner, card).FirstOrDefault();
        return identity is null ? null : new(identity.Identity.OutputKind, identity.Source.SkillId);
    }

    public int? Rank(CharacterState owner, Card card) => getCharacterGuanYuWumiaoRuntime().JuewuForeignHandRank(owner, card);
    public CardSnapshot Decorate(CharacterState owner, Card card, CardSnapshot snapshot) => getProgramPipelineHooks().ProjectHandCard(owner, card, snapshot);
}
