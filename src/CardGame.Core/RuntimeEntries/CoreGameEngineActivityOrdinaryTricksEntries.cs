#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime))]
internal sealed partial class CoreGameEngineActivityOrdinaryTricksEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime> _resolve;
    private CoreGameEngineActivityOrdinaryTricksEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActivityOrdinaryTricksEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime> resolve) => new(resolve);
}
