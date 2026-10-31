#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheS5ShuangxiongRuntime))]
internal sealed partial class CoreGameEngineShanheS5ShuangxiongEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5ShuangxiongRuntime> _resolve;
    private CoreGameEngineShanheS5ShuangxiongEntries(Func<EngineOwners.CoreGameEngineShanheS5ShuangxiongRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5ShuangxiongEntries Create(Func<EngineOwners.CoreGameEngineShanheS5ShuangxiongRuntime> resolve) => new(resolve);
}
