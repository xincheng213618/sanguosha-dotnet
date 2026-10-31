#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheXiaoruiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheXiaoruiRuntime> _resolve;
    private CoreGameEngineShanheXiaoruiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheXiaoruiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheXiaoruiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheXiaoruiRuntime> resolve) => new(resolve);
}
