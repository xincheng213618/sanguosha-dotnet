#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5ZhangxiuEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5ZhangxiuRuntime> _resolve;
    private CoreGameEngineShanheS5ZhangxiuEntries(Func<EngineOwners.CoreGameEngineShanheS5ZhangxiuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5ZhangxiuEntries Create(Func<EngineOwners.CoreGameEngineShanheS5ZhangxiuRuntime> resolve) => new(resolve);
}
