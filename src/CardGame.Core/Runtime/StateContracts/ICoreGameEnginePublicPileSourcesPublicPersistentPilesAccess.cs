#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<(int Owner, string Skill, string Instance), global::CardGame.Core.PublicPersistentPileSource> _publicPersistentPiles { get; }
}
