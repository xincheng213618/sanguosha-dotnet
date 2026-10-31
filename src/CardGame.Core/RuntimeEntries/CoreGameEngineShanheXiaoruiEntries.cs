#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheXiaoruiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheXiaoruiRuntime> _resolve;
    private CoreGameEngineShanheXiaoruiEntries(Func<EngineOwners.CoreGameEngineShanheXiaoruiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheXiaoruiEntries Create(Func<EngineOwners.CoreGameEngineShanheXiaoruiRuntime> resolve) => new(resolve);
}
