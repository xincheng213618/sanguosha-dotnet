#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSchoolLateRuntime))]
internal sealed partial class CoreGameEngineShanheSchoolLateEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSchoolLateRuntime> _resolve;
    private CoreGameEngineShanheSchoolLateEntries(Func<EngineOwners.CoreGameEngineShanheSchoolLateRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSchoolLateEntries Create(Func<EngineOwners.CoreGameEngineShanheSchoolLateRuntime> resolve) => new(resolve);
}
