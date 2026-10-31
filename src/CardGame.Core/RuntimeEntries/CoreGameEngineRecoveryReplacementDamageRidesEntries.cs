#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime))]
internal sealed partial class CoreGameEngineRecoveryReplacementDamageRidesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> _resolve;
    private CoreGameEngineRecoveryReplacementDamageRidesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecoveryReplacementDamageRidesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> resolve) => new(resolve);
}
