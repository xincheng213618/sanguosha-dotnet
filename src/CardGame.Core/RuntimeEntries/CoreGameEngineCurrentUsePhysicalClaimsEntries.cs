#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentUsePhysicalClaimsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> _resolve;
    private CoreGameEngineCurrentUsePhysicalClaimsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentUsePhysicalClaimsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> resolve) => new(resolve);
}
