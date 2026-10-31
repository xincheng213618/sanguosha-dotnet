#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMobilizationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheMobilizationRuntime> _resolve;
    private CoreGameEngineShanheMobilizationEntries(Func<EngineOwners.CoreGameEngineShanheMobilizationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheMobilizationEntries Create(Func<EngineOwners.CoreGameEngineShanheMobilizationRuntime> resolve) => new(resolve);
}
