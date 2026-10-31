#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime))]
internal sealed partial class CoreGameEnginePrepDiscardReceiptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime> _resolve;
    private CoreGameEnginePrepDiscardReceiptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepDiscardReceiptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime> resolve) => new(resolve);
}
