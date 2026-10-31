#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed partial class CoreGameEngineSkillProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _resolve;
    private CoreGameEngineSkillProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSkillProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> resolve) => new(resolve);
}
