namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<(int Owner, string Skill, string Instance), global::CardGame.Core.PublicPersistentPileSource> _publicPersistentPiles => ref _publicPileSources.ReferenceToPublicPersistentPiles;
}
