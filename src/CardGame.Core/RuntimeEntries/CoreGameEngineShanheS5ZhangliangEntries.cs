#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheS5ZhangliangRuntime))]
internal sealed partial class CoreGameEngineShanheS5ZhangliangEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5ZhangliangRuntime> _resolve;
    private CoreGameEngineShanheS5ZhangliangEntries(Func<EngineOwners.CoreGameEngineShanheS5ZhangliangRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5ZhangliangEntries Create(Func<EngineOwners.CoreGameEngineShanheS5ZhangliangRuntime> resolve) => new(resolve);
}
