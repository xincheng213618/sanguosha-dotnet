#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime))]
internal sealed partial class CoreGameEngineVirtualOrdinaryTrickUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> _resolve;
    private CoreGameEngineVirtualOrdinaryTrickUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineVirtualOrdinaryTrickUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> resolve) => new(resolve);
}
