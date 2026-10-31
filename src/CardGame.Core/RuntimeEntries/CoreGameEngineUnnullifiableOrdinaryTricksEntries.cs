#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime))]
internal sealed partial class CoreGameEngineUnnullifiableOrdinaryTricksEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime> _resolve;
    private CoreGameEngineUnnullifiableOrdinaryTricksEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUnnullifiableOrdinaryTricksEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime> resolve) => new(resolve);
}
