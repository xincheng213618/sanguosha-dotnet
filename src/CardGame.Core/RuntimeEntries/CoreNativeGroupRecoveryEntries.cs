#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeGroupRecoveryRuntime))]
internal sealed partial class CoreNativeGroupRecoveryEntries
{
    private readonly Func<EngineOwners.CoreNativeGroupRecoveryRuntime> _resolve;
    private CoreNativeGroupRecoveryEntries(Func<EngineOwners.CoreNativeGroupRecoveryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeGroupRecoveryEntries Create(Func<EngineOwners.CoreNativeGroupRecoveryRuntime> resolve) => new(resolve);
}
