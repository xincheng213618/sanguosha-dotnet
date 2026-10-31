#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheZhanhunDaoEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheZhanhunDaoRuntime> _resolve;
    private CoreGameEngineShanheZhanhunDaoEntries(Func<EngineOwners.CoreGameEngineShanheZhanhunDaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheZhanhunDaoEntries Create(Func<EngineOwners.CoreGameEngineShanheZhanhunDaoRuntime> resolve) => new(resolve);
}
