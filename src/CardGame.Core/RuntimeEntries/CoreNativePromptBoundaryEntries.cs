#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed partial class CoreNativePromptBoundaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _resolve;
    private CoreNativePromptBoundaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativePromptBoundaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> resolve) => new(resolve);
}
