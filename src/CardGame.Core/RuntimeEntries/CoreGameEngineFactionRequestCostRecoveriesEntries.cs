#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime))]
internal sealed partial class CoreGameEngineFactionRequestCostRecoveriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> _resolve;
    private CoreGameEngineFactionRequestCostRecoveriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFactionRequestCostRecoveriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> resolve) => new(resolve);
}
