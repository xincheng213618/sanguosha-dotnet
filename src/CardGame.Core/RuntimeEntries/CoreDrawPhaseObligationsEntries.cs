#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreDrawPhaseObligationsRuntime))]
internal sealed partial class CoreDrawPhaseObligationsEntries
{
    private readonly Func<EngineOwners.CoreDrawPhaseObligationsRuntime> _resolve;
    private CoreDrawPhaseObligationsEntries(Func<EngineOwners.CoreDrawPhaseObligationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDrawPhaseObligationsEntries Create(Func<EngineOwners.CoreDrawPhaseObligationsRuntime> resolve) => new(resolve);
}
