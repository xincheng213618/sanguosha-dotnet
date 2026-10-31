#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundEndingEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> _resolve;
    private CoreGameEngineRoundEndingEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundEndingEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> resolve) => new(resolve);
}
