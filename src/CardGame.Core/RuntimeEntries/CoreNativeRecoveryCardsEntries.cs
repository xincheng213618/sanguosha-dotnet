#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeRecoveryCardsRuntime))]
internal sealed partial class CoreNativeRecoveryCardsEntries
{
    private readonly Func<EngineOwners.CoreNativeRecoveryCardsRuntime> _resolve;
    private CoreNativeRecoveryCardsEntries(Func<EngineOwners.CoreNativeRecoveryCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeRecoveryCardsEntries Create(Func<EngineOwners.CoreNativeRecoveryCardsRuntime> resolve) => new(resolve);
}
