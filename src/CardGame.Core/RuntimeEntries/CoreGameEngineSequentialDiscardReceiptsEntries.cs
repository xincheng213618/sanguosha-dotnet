#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialDiscardReceiptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime> _resolve;
    private CoreGameEngineSequentialDiscardReceiptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSequentialDiscardReceiptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime> resolve) => new(resolve);
}
