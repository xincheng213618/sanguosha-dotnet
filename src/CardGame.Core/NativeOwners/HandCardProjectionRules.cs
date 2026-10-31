#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HandCardProjectionRules(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries> getCoreGameEngineActionQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries> getCharacterGuanYuWumiaoRuntime, global::System.Func<global::CardGame.Core.ProgramPipelineHooks> getProgramPipelineHooks) : IHandCardProjectionRules
{
    public HandCardIdentityProjection? FirstIdentity(CharacterState owner, Card card)
    {
        var identity = getCoreGameEngineActionQueriesRuntime().GetProgramCardIdentityMatches(owner, card).FirstOrDefault();
        return identity is null ? null : new(identity.Identity.OutputKind, identity.Source.SkillId);
    }

    public int? Rank(CharacterState owner, Card card) => getCharacterGuanYuWumiaoRuntime().JuewuForeignHandRank(owner, card);
    public CardSnapshot Decorate(CharacterState owner, Card card, CardSnapshot snapshot) => getProgramPipelineHooks().ProjectHandCard(owner, card, snapshot);
}
