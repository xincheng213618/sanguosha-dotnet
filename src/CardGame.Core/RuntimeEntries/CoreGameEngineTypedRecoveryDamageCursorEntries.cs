#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime))]
internal sealed partial class CoreGameEngineTypedRecoveryDamageCursorEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> _resolve;
    private CoreGameEngineTypedRecoveryDamageCursorEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTypedRecoveryDamageCursorEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> resolve) => new(resolve);
}
