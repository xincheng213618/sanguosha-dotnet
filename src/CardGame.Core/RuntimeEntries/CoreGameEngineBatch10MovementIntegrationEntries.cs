#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBatch10MovementIntegrationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineBatch10MovementIntegrationRuntime> _resolve;
    private CoreGameEngineBatch10MovementIntegrationEntries(Func<EngineOwners.CoreGameEngineBatch10MovementIntegrationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBatch10MovementIntegrationEntries Create(Func<EngineOwners.CoreGameEngineBatch10MovementIntegrationRuntime> resolve) => new(resolve);
}
