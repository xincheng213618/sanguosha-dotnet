#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheTerrainPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheTerrainPoliciesRuntime> _resolve;
    private CoreGameEngineShanheTerrainPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheTerrainPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheTerrainPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheTerrainPoliciesRuntime> resolve) => new(resolve);
}
