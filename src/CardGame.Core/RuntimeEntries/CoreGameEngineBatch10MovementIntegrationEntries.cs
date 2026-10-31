#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBatch10MovementIntegrationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBatch10MovementIntegrationRuntime> _resolve;
    private CoreGameEngineBatch10MovementIntegrationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBatch10MovementIntegrationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBatch10MovementIntegrationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBatch10MovementIntegrationRuntime> resolve) => new(resolve);
}
