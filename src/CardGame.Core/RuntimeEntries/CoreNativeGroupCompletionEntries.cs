#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeGroupCompletionRuntime))]
internal sealed partial class CoreNativeGroupCompletionEntries
{
    private readonly Func<EngineOwners.CoreNativeGroupCompletionRuntime> _resolve;
    private CoreNativeGroupCompletionEntries(Func<EngineOwners.CoreNativeGroupCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeGroupCompletionEntries Create(Func<EngineOwners.CoreNativeGroupCompletionRuntime> resolve) => new(resolve);
}
