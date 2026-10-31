#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed partial class CoreNativePromptBoundaryEntries
{
    private readonly Func<EngineOwners.CoreNativePromptBoundaryRuntime> _resolve;
    private CoreNativePromptBoundaryEntries(Func<EngineOwners.CoreNativePromptBoundaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativePromptBoundaryEntries Create(Func<EngineOwners.CoreNativePromptBoundaryRuntime> resolve) => new(resolve);
}
