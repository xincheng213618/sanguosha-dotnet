#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime))]
internal sealed partial class CoreGameEngineOwnedExactCountAndNamedTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> _resolve;
    private CoreGameEngineOwnedExactCountAndNamedTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedExactCountAndNamedTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> resolve) => new(resolve);
}
