#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramRecoveryReceiptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRecoveryReceiptsRuntime> _resolve;
    private CoreGameEngineProgramRecoveryReceiptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRecoveryReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramRecoveryReceiptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRecoveryReceiptsRuntime> resolve) => new(resolve);
}
