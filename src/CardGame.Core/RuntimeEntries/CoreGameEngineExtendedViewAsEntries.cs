#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime))]
internal sealed partial class CoreGameEngineExtendedViewAsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> _resolve;
    private CoreGameEngineExtendedViewAsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtendedViewAsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> resolve) => new(resolve);
}
