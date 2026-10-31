#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheQianlongEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheQianlongRuntime> _resolve;
    private CoreGameEngineShanheQianlongEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheQianlongRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheQianlongEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheQianlongRuntime> resolve) => new(resolve);
}
