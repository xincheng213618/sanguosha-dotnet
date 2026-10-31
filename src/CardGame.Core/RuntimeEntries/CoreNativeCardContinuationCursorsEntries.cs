#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed partial class CoreNativeCardContinuationCursorsEntries
{
    private readonly Func<EngineOwners.CoreNativeCardContinuationCursorsRuntime> _resolve;
    private CoreNativeCardContinuationCursorsEntries(Func<EngineOwners.CoreNativeCardContinuationCursorsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardContinuationCursorsEntries Create(Func<EngineOwners.CoreNativeCardContinuationCursorsRuntime> resolve) => new(resolve);
}
