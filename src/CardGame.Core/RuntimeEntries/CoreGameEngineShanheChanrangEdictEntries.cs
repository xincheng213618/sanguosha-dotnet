#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheChanrangEdictRuntime))]
internal sealed partial class CoreGameEngineShanheChanrangEdictEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheChanrangEdictRuntime> _resolve;
    private CoreGameEngineShanheChanrangEdictEntries(Func<EngineOwners.CoreGameEngineShanheChanrangEdictRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheChanrangEdictEntries Create(Func<EngineOwners.CoreGameEngineShanheChanrangEdictRuntime> resolve) => new(resolve);
}
