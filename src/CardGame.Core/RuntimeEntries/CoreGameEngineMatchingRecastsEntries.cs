#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastsRuntime))]
internal sealed partial class CoreGameEngineMatchingRecastsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastsRuntime> _resolve;
    private CoreGameEngineMatchingRecastsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMatchingRecastsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastsRuntime> resolve) => new(resolve);
}
