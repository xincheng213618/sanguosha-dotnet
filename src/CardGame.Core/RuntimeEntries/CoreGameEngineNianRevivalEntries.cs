#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNianRevivalEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalRuntime> _resolve;
    private CoreGameEngineNianRevivalEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNianRevivalEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalRuntime> resolve) => new(resolve);
}
