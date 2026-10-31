#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEnginePublicPileSourcesState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess
{
    private global::System.Collections.Generic.Dictionary<(int Owner, string Skill, string Instance), global::CardGame.Core.PublicPersistentPileSource> _publicPersistentPiles = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<(int Owner, string Skill, string Instance), global::CardGame.Core.PublicPersistentPileSource> ReferenceToPublicPersistentPiles => ref _publicPersistentPiles;

    ref readonly global::System.Collections.Generic.Dictionary<(int Owner, string Skill, string Instance), global::CardGame.Core.PublicPersistentPileSource> global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles => ref _publicPersistentPiles;
}
