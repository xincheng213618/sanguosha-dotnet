#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeAiDecisionsRuntime))]
internal sealed partial class CoreNativeAiDecisionsEntries
{
    private readonly Func<EngineOwners.CoreNativeAiDecisionsRuntime> _resolve;
    private CoreNativeAiDecisionsEntries(Func<EngineOwners.CoreNativeAiDecisionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeAiDecisionsEntries Create(Func<EngineOwners.CoreNativeAiDecisionsRuntime> resolve) => new(resolve);
}
