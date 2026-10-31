#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheXingtianRuntime))]
internal sealed partial class CoreGameEngineShanheXingtianEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheXingtianRuntime> _resolve;
    private CoreGameEngineShanheXingtianEntries(Func<EngineOwners.CoreGameEngineShanheXingtianRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheXingtianEntries Create(Func<EngineOwners.CoreGameEngineShanheXingtianRuntime> resolve) => new(resolve);
}
