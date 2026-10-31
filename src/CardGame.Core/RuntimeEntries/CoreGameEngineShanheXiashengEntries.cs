#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheXiashengEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheXiashengRuntime> _resolve;
    private CoreGameEngineShanheXiashengEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheXiashengRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheXiashengEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheXiashengRuntime> resolve) => new(resolve);
}
