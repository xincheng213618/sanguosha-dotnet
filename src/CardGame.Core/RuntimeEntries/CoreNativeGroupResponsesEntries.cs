#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeGroupResponsesRuntime))]
internal sealed partial class CoreNativeGroupResponsesEntries
{
    private readonly Func<EngineOwners.CoreNativeGroupResponsesRuntime> _resolve;
    private CoreNativeGroupResponsesEntries(Func<EngineOwners.CoreNativeGroupResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeGroupResponsesEntries Create(Func<EngineOwners.CoreNativeGroupResponsesRuntime> resolve) => new(resolve);
}
