#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalTargetPublicPilesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFinalTargetPublicPilesRuntime> _resolve;
    private CoreGameEngineFinalTargetPublicPilesEntries(Func<EngineOwners.CoreGameEngineFinalTargetPublicPilesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFinalTargetPublicPilesEntries Create(Func<EngineOwners.CoreGameEngineFinalTargetPublicPilesRuntime> resolve) => new(resolve);
}
