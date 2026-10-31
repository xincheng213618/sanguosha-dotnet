#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramIssuedTriggerBridgesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIssuedTriggerBridgesRuntime> _resolve;
    private CoreGameEngineProgramIssuedTriggerBridgesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIssuedTriggerBridgesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramIssuedTriggerBridgesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIssuedTriggerBridgesRuntime> resolve) => new(resolve);
}
