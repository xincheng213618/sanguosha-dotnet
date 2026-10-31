#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeCardUseCompletionRuntime))]
internal sealed partial class CoreNativeCardUseCompletionEntries
{
    private readonly Func<EngineOwners.CoreNativeCardUseCompletionRuntime> _resolve;
    private CoreNativeCardUseCompletionEntries(Func<EngineOwners.CoreNativeCardUseCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardUseCompletionEntries Create(Func<EngineOwners.CoreNativeCardUseCompletionRuntime> resolve) => new(resolve);
}
