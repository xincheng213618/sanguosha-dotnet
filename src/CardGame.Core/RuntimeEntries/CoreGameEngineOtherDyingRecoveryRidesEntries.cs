#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOtherDyingRecoveryRidesRuntime))]
internal sealed partial class CoreGameEngineOtherDyingRecoveryRidesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOtherDyingRecoveryRidesRuntime> _resolve;
    private CoreGameEngineOtherDyingRecoveryRidesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOtherDyingRecoveryRidesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOtherDyingRecoveryRidesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOtherDyingRecoveryRidesRuntime> resolve) => new(resolve);
}
