#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheYongzhanEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheYongzhanRuntime> _resolve;
    private CoreGameEngineShanheYongzhanEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheYongzhanRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheYongzhanEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheYongzhanRuntime> resolve) => new(resolve);
}
