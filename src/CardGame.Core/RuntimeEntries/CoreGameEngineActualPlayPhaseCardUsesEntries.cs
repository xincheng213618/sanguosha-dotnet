#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime))]
internal sealed partial class CoreGameEngineActualPlayPhaseCardUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> _resolve;
    private CoreGameEngineActualPlayPhaseCardUsesEntries(Func<EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualPlayPhaseCardUsesEntries Create(Func<EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> resolve) => new(resolve);
}
