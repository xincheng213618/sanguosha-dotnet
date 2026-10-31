#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEnginePublicPileSourcesState : Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess
{
    private Dictionary<(int Owner, string Skill, string Instance), PublicPersistentPileSource> _publicPersistentPiles = [];
    internal ref readonly Dictionary<(int Owner, string Skill, string Instance), PublicPersistentPileSource> ReferenceToPublicPersistentPiles => ref _publicPersistentPiles;

    ref readonly Dictionary<(int Owner, string Skill, string Instance), PublicPersistentPileSource> Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles => ref _publicPersistentPiles;
}
