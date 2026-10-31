#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime))]
internal sealed partial class CoreNativeRecoveryCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> _resolve;
    private CoreNativeRecoveryCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeRecoveryCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> resolve) => new(resolve);
}
