#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNextActualUsePindianAiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUsePindianAiRuntime> _resolve;
    private CoreGameEngineNextActualUsePindianAiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUsePindianAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNextActualUsePindianAiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUsePindianAiRuntime> resolve) => new(resolve);
}
