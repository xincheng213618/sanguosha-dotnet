#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheYingxiangEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheYingxiangRuntime> _resolve;
    private CoreGameEngineShanheYingxiangEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheYingxiangRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheYingxiangEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheYingxiangRuntime> resolve) => new(resolve);
}
