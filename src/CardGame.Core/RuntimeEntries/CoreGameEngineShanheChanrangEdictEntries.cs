#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheChanrangEdictEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheChanrangEdictRuntime> _resolve;
    private CoreGameEngineShanheChanrangEdictEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheChanrangEdictRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheChanrangEdictEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheChanrangEdictRuntime> resolve) => new(resolve);
}
