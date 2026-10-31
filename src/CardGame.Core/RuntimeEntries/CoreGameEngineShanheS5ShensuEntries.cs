#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheS5ShensuRuntime))]
internal sealed partial class CoreGameEngineShanheS5ShensuEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5ShensuRuntime> _resolve;
    private CoreGameEngineShanheS5ShensuEntries(Func<EngineOwners.CoreGameEngineShanheS5ShensuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5ShensuEntries Create(Func<EngineOwners.CoreGameEngineShanheS5ShensuRuntime> resolve) => new(resolve);
}
