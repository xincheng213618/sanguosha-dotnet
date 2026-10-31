#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramIssuedTriggerBridgesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramIssuedTriggerBridgesRuntime> _resolve;
    private CoreGameEngineProgramIssuedTriggerBridgesEntries(Func<EngineOwners.CoreGameEngineProgramIssuedTriggerBridgesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramIssuedTriggerBridgesEntries Create(Func<EngineOwners.CoreGameEngineProgramIssuedTriggerBridgesRuntime> resolve) => new(resolve);
}
