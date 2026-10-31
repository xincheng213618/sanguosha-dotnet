#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheAlliedTricksRuntime))]
internal sealed partial class CoreGameEngineShanheAlliedTricksEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheAlliedTricksRuntime> _resolve;
    private CoreGameEngineShanheAlliedTricksEntries(Func<EngineOwners.CoreGameEngineShanheAlliedTricksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheAlliedTricksEntries Create(Func<EngineOwners.CoreGameEngineShanheAlliedTricksRuntime> resolve) => new(resolve);
}
