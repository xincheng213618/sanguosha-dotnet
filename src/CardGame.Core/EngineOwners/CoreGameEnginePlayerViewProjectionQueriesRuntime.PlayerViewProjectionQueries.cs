#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePlayerViewProjectionQueriesRuntime
{
    internal GameSnapshot ProjectPlayerView(int viewerSeat, bool revealAll)
    {
        using var queryScope = _coreActionQueriesOperations.BeginActionQuery();
        return _playerViewProjectorOperations.Get().Project(_kernelCoreGameEnginePlayerViewProjectionQueriesOperations.CreatePlayerViewVisibilityQueries(), _kernelCoreGameEnginePlayerViewProjectionQueriesOperations.CreatePlayerViewCardsQueries(), _kernelCoreGameEnginePlayerViewProjectionQueriesOperations.CreatePlayerViewRulesQueries(), _kernelCoreGameEngineActivePlayerEffectQueriesOperations.CreateActivePlayerEffectQueries(), _kernelCoreGameEnginePlayerViewProjectionQueriesOperations.CreatePlayerViewMatchQueries(), viewerSeat, revealAll);
    }

    HandCardIdentityProjection? IHandCardProjectionRules.FirstIdentity(CharacterState owner, Card card)
    {
        var identity = _coreActionQueriesOperations.GetProgramCardIdentityMatches(owner, card).FirstOrDefault();
        return identity is null ? null : new(identity.Identity.OutputKind, identity.Source.SkillId);
    }

    int? IHandCardProjectionRules.Rank(CharacterState owner, Card card) => _characterGuanYuWumiaoOperations.JuewuForeignHandRank(owner, card);
    CardSnapshot IHandCardProjectionRules.Decorate(CharacterState owner, Card card, CardSnapshot snapshot) => _programPipelineHooksOperations._programPipelineHooks.ProjectHandCard(owner, card, snapshot);
}
