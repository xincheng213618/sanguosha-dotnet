#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSayingTieredSuppliesRuntime))]
internal sealed partial class CoreGameEngineSayingTieredSuppliesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSayingTieredSuppliesRuntime> _resolve;
    private CoreGameEngineSayingTieredSuppliesEntries(Func<EngineOwners.CoreGameEngineSayingTieredSuppliesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSayingTieredSuppliesEntries Create(Func<EngineOwners.CoreGameEngineSayingTieredSuppliesRuntime> resolve) => new(resolve);
}
