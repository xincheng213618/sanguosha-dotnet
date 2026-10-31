#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageCompletionRuntime))]
internal sealed partial class CoreNativeDamageCompletionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageCompletionRuntime> _resolve;
    private CoreNativeDamageCompletionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDamageCompletionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageCompletionRuntime> resolve) => new(resolve);
}
