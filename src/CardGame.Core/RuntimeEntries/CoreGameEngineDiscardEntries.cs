#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDiscardRuntime))]
internal sealed partial class CoreGameEngineDiscardEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardRuntime> _resolve;
    private CoreGameEngineDiscardEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDiscardEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardRuntime> resolve) => new(resolve);
}
