#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMouLuMengEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime> _resolve;
    private CoreGameEngineMouLuMengEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMouLuMengEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime> resolve) => new(resolve);
}
