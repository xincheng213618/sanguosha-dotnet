#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime))]
internal sealed partial class CoreMatchPresentationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> _resolve;
    private CoreMatchPresentationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchPresentationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> resolve) => new(resolve);
}
