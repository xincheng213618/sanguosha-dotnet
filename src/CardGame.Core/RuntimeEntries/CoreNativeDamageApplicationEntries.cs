#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed partial class CoreNativeDamageApplicationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> _resolve;
    private CoreNativeDamageApplicationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDamageApplicationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> resolve) => new(resolve);
}
