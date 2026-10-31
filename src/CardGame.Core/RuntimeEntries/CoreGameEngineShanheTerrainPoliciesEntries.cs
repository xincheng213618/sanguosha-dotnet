#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheTerrainPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheTerrainPoliciesRuntime> _resolve;
    private CoreGameEngineShanheTerrainPoliciesEntries(Func<EngineOwners.CoreGameEngineShanheTerrainPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheTerrainPoliciesEntries Create(Func<EngineOwners.CoreGameEngineShanheTerrainPoliciesRuntime> resolve) => new(resolve);
}
