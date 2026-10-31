#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime))]
internal sealed partial class CoreGameEngineVirtualOrdinaryTrickUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> _resolve;
    private CoreGameEngineVirtualOrdinaryTrickUsesEntries(Func<EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineVirtualOrdinaryTrickUsesEntries Create(Func<EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> resolve) => new(resolve);
}
