#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime))]
internal sealed partial class CoreGameEngineRecoveryReplacementDamageRidesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> _resolve;
    private CoreGameEngineRecoveryReplacementDamageRidesEntries(Func<EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecoveryReplacementDamageRidesEntries Create(Func<EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> resolve) => new(resolve);
}
