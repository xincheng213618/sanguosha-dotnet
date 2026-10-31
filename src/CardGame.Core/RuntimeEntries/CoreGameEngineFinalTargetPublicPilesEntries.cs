#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalTargetPublicPilesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetPublicPilesRuntime> _resolve;
    private CoreGameEngineFinalTargetPublicPilesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetPublicPilesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFinalTargetPublicPilesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetPublicPilesRuntime> resolve) => new(resolve);
}
