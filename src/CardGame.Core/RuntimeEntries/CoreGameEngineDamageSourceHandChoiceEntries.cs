#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageSourceHandChoiceRuntime))]
internal sealed partial class CoreGameEngineDamageSourceHandChoiceEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageSourceHandChoiceRuntime> _resolve;
    private CoreGameEngineDamageSourceHandChoiceEntries(Func<EngineOwners.CoreGameEngineDamageSourceHandChoiceRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageSourceHandChoiceEntries Create(Func<EngineOwners.CoreGameEngineDamageSourceHandChoiceRuntime> resolve) => new(resolve);
}
