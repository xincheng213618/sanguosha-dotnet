#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineScenarioPrintedLordQualificationsRuntime))]
internal sealed partial class CoreGameEngineScenarioPrintedLordQualificationsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineScenarioPrintedLordQualificationsRuntime> _resolve;
    private CoreGameEngineScenarioPrintedLordQualificationsEntries(Func<EngineOwners.CoreGameEngineScenarioPrintedLordQualificationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineScenarioPrintedLordQualificationsEntries Create(Func<EngineOwners.CoreGameEngineScenarioPrintedLordQualificationsRuntime> resolve) => new(resolve);
}
