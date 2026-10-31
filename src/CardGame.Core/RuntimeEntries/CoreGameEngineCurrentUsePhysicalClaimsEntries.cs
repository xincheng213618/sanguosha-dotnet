#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime))]
internal sealed partial class CoreGameEngineCurrentUsePhysicalClaimsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> _resolve;
    private CoreGameEngineCurrentUsePhysicalClaimsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentUsePhysicalClaimsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> resolve) => new(resolve);
}
