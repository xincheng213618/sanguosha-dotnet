#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageSourceHandChoiceRuntime))]
internal sealed partial class CoreGameEngineDamageSourceHandChoiceEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageSourceHandChoiceRuntime> _resolve;
    private CoreGameEngineDamageSourceHandChoiceEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageSourceHandChoiceRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageSourceHandChoiceEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageSourceHandChoiceRuntime> resolve) => new(resolve);
}
