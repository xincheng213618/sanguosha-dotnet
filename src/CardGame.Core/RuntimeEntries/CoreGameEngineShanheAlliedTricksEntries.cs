#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheAlliedTricksRuntime))]
internal sealed partial class CoreGameEngineShanheAlliedTricksEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheAlliedTricksRuntime> _resolve;
    private CoreGameEngineShanheAlliedTricksEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheAlliedTricksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheAlliedTricksEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheAlliedTricksRuntime> resolve) => new(resolve);
}
