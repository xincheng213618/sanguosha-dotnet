#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineInitialHandModifiersRuntime))]
internal sealed partial class CoreGameEngineInitialHandModifiersEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInitialHandModifiersRuntime> _resolve;
    private CoreGameEngineInitialHandModifiersEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInitialHandModifiersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineInitialHandModifiersEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInitialHandModifiersRuntime> resolve) => new(resolve);
}
