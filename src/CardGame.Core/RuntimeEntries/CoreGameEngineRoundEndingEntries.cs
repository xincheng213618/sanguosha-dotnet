#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundEndingRuntime))]
internal sealed partial class CoreGameEngineRoundEndingEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundEndingRuntime> _resolve;
    private CoreGameEngineRoundEndingEntries(Func<EngineOwners.CoreGameEngineRoundEndingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundEndingEntries Create(Func<EngineOwners.CoreGameEngineRoundEndingRuntime> resolve) => new(resolve);
}
