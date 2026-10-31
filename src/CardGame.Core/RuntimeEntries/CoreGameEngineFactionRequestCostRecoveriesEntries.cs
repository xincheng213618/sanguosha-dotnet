#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime))]
internal sealed partial class CoreGameEngineFactionRequestCostRecoveriesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> _resolve;
    private CoreGameEngineFactionRequestCostRecoveriesEntries(Func<EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFactionRequestCostRecoveriesEntries Create(Func<EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> resolve) => new(resolve);
}
