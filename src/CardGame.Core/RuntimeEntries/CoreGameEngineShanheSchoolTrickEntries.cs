#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSchoolTrickRuntime))]
internal sealed partial class CoreGameEngineShanheSchoolTrickEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSchoolTrickRuntime> _resolve;
    private CoreGameEngineShanheSchoolTrickEntries(Func<EngineOwners.CoreGameEngineShanheSchoolTrickRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSchoolTrickEntries Create(Func<EngineOwners.CoreGameEngineShanheSchoolTrickRuntime> resolve) => new(resolve);
}
