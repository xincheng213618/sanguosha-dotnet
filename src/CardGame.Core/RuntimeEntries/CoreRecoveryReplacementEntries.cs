#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed partial class CoreRecoveryReplacementEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _resolve;
    private CoreRecoveryReplacementEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreRecoveryReplacementEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> resolve) => new(resolve);
}
