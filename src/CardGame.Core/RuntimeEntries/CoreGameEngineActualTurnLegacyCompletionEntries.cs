#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualTurnLegacyCompletionRuntime))]
internal sealed partial class CoreGameEngineActualTurnLegacyCompletionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualTurnLegacyCompletionRuntime> _resolve;
    private CoreGameEngineActualTurnLegacyCompletionEntries(Func<EngineOwners.CoreGameEngineActualTurnLegacyCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualTurnLegacyCompletionEntries Create(Func<EngineOwners.CoreGameEngineActualTurnLegacyCompletionRuntime> resolve) => new(resolve);
}
