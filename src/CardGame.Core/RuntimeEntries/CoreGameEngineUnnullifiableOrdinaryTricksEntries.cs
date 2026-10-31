#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime))]
internal sealed partial class CoreGameEngineUnnullifiableOrdinaryTricksEntries
{
    private readonly Func<EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime> _resolve;
    private CoreGameEngineUnnullifiableOrdinaryTricksEntries(Func<EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUnnullifiableOrdinaryTricksEntries Create(Func<EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime> resolve) => new(resolve);
}
