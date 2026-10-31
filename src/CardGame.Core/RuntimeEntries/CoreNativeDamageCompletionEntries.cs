#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeDamageCompletionRuntime))]
internal sealed partial class CoreNativeDamageCompletionEntries
{
    private readonly Func<EngineOwners.CoreNativeDamageCompletionRuntime> _resolve;
    private CoreNativeDamageCompletionEntries(Func<EngineOwners.CoreNativeDamageCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDamageCompletionEntries Create(Func<EngineOwners.CoreNativeDamageCompletionRuntime> resolve) => new(resolve);
}
