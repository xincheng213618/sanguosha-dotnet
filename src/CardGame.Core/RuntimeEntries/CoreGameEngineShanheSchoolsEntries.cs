#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSchoolsRuntime))]
internal sealed partial class CoreGameEngineShanheSchoolsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSchoolsRuntime> _resolve;
    private CoreGameEngineShanheSchoolsEntries(Func<EngineOwners.CoreGameEngineShanheSchoolsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSchoolsEntries Create(Func<EngineOwners.CoreGameEngineShanheSchoolsRuntime> resolve) => new(resolve);
}
