#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreDrawPhaseObligationsRuntime))]
internal sealed partial class CoreDrawPhaseObligationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDrawPhaseObligationsRuntime> _resolve;
    private CoreDrawPhaseObligationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreDrawPhaseObligationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDrawPhaseObligationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreDrawPhaseObligationsRuntime> resolve) => new(resolve);
}
