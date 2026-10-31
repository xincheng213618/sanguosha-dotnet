#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5QiangxiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QiangxiRuntime> _resolve;
    private CoreGameEngineShanheS5QiangxiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QiangxiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5QiangxiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QiangxiRuntime> resolve) => new(resolve);
}
