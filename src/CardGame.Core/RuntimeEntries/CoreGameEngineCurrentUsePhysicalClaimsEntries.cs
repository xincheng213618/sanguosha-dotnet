#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime))]
internal sealed partial class CoreGameEngineCurrentUsePhysicalClaimsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> _resolve;
    private CoreGameEngineCurrentUsePhysicalClaimsEntries(Func<EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentUsePhysicalClaimsEntries Create(Func<EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> resolve) => new(resolve);
}
