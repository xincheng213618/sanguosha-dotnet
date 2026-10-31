#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime))]
internal sealed partial class CoreGameEngineTypedRecoveryDamageCursorEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> _resolve;
    private CoreGameEngineTypedRecoveryDamageCursorEntries(Func<EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTypedRecoveryDamageCursorEntries Create(Func<EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> resolve) => new(resolve);
}
