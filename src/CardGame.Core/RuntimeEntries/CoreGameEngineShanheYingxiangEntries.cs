#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheYingxiangEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheYingxiangRuntime> _resolve;
    private CoreGameEngineShanheYingxiangEntries(Func<EngineOwners.CoreGameEngineShanheYingxiangRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheYingxiangEntries Create(Func<EngineOwners.CoreGameEngineShanheYingxiangRuntime> resolve) => new(resolve);
}
