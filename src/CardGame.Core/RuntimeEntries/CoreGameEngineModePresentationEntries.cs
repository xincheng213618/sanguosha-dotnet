#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModePresentationRuntime))]
internal sealed partial class CoreGameEngineModePresentationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModePresentationRuntime> _resolve;
    private CoreGameEngineModePresentationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModePresentationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineModePresentationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModePresentationRuntime> resolve) => new(resolve);
}
