#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDyingOwnedCardSelectionsRuntime))]
internal sealed partial class CoreGameEngineDyingOwnedCardSelectionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDyingOwnedCardSelectionsRuntime> _resolve;
    private CoreGameEngineDyingOwnedCardSelectionsEntries(Func<EngineOwners.CoreGameEngineDyingOwnedCardSelectionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingOwnedCardSelectionsEntries Create(Func<EngineOwners.CoreGameEngineDyingOwnedCardSelectionsRuntime> resolve) => new(resolve);
}
