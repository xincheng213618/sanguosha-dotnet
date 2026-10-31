#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<(int Owner, string Skill, string Instance), PublicPersistentPileSource> _publicPersistentPiles { get; }
}
