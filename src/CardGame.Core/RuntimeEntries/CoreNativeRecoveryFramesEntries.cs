#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeRecoveryFramesRuntime))]
internal sealed partial class CoreNativeRecoveryFramesEntries
{
    private readonly Func<EngineOwners.CoreNativeRecoveryFramesRuntime> _resolve;
    private CoreNativeRecoveryFramesEntries(Func<EngineOwners.CoreNativeRecoveryFramesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeRecoveryFramesEntries Create(Func<EngineOwners.CoreNativeRecoveryFramesRuntime> resolve) => new(resolve);
}
