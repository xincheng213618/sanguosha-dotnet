#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed partial class CoreRecoveryReplacementEntries
{
    private readonly Func<EngineOwners.CoreRecoveryReplacementRuntime> _resolve;
    private CoreRecoveryReplacementEntries(Func<EngineOwners.CoreRecoveryReplacementRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreRecoveryReplacementEntries Create(Func<EngineOwners.CoreRecoveryReplacementRuntime> resolve) => new(resolve);
}
