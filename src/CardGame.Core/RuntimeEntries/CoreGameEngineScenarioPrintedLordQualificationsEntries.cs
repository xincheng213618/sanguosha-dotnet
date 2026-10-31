#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineScenarioPrintedLordQualificationsRuntime))]
internal sealed partial class CoreGameEngineScenarioPrintedLordQualificationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineScenarioPrintedLordQualificationsRuntime> _resolve;
    private CoreGameEngineScenarioPrintedLordQualificationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineScenarioPrintedLordQualificationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineScenarioPrintedLordQualificationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineScenarioPrintedLordQualificationsRuntime> resolve) => new(resolve);
}
