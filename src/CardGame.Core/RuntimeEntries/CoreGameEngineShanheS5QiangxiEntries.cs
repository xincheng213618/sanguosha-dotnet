#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5QiangxiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5QiangxiRuntime> _resolve;
    private CoreGameEngineShanheS5QiangxiEntries(Func<EngineOwners.CoreGameEngineShanheS5QiangxiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5QiangxiEntries Create(Func<EngineOwners.CoreGameEngineShanheS5QiangxiRuntime> resolve) => new(resolve);
}
