#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramRecoveryReceiptsRuntime))]
internal sealed partial class CoreGameEngineProgramRecoveryReceiptsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramRecoveryReceiptsRuntime> _resolve;
    private CoreGameEngineProgramRecoveryReceiptsEntries(Func<EngineOwners.CoreGameEngineProgramRecoveryReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramRecoveryReceiptsEntries Create(Func<EngineOwners.CoreGameEngineProgramRecoveryReceiptsRuntime> resolve) => new(resolve);
}
