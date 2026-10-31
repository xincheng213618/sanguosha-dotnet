#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime))]
internal sealed partial class CoreNativeGroupRecoveryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime> _resolve;
    private CoreNativeGroupRecoveryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeGroupRecoveryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime> resolve) => new(resolve);
}
