#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePhysicalCardDrawRuntime))]
internal sealed partial class CoreGameEnginePhysicalCardDrawEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePhysicalCardDrawRuntime> _resolve;
    private CoreGameEnginePhysicalCardDrawEntries(Func<EngineOwners.CoreGameEnginePhysicalCardDrawRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhysicalCardDrawEntries Create(Func<EngineOwners.CoreGameEnginePhysicalCardDrawRuntime> resolve) => new(resolve);
}
