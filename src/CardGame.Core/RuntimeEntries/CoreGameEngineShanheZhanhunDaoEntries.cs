#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheZhanhunDaoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheZhanhunDaoRuntime> _resolve;
    private CoreGameEngineShanheZhanhunDaoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheZhanhunDaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheZhanhunDaoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheZhanhunDaoRuntime> resolve) => new(resolve);
}
