#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeQilinBowRuntime))]
internal sealed partial class CoreNativeQilinBowEntries
{
    private readonly Func<EngineOwners.CoreNativeQilinBowRuntime> _resolve;
    private CoreNativeQilinBowEntries(Func<EngineOwners.CoreNativeQilinBowRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeQilinBowEntries Create(Func<EngineOwners.CoreNativeQilinBowRuntime> resolve) => new(resolve);
}
