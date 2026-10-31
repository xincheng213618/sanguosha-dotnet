#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed partial class CoreGameEngineSkillProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSkillProgramsRuntime> _resolve;
    private CoreGameEngineSkillProgramsEntries(Func<EngineOwners.CoreGameEngineSkillProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSkillProgramsEntries Create(Func<EngineOwners.CoreGameEngineSkillProgramsRuntime> resolve) => new(resolve);
}
