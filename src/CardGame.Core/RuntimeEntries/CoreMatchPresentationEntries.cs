#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreMatchPresentationRuntime))]
internal sealed partial class CoreMatchPresentationEntries
{
    private readonly Func<EngineOwners.CoreMatchPresentationRuntime> _resolve;
    private CoreMatchPresentationEntries(Func<EngineOwners.CoreMatchPresentationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchPresentationEntries Create(Func<EngineOwners.CoreMatchPresentationRuntime> resolve) => new(resolve);
}
