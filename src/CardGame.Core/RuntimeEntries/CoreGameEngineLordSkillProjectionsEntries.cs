#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineLordSkillProjectionsRuntime))]
internal sealed partial class CoreGameEngineLordSkillProjectionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> _resolve;
    private CoreGameEngineLordSkillProjectionsEntries(Func<EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLordSkillProjectionsEntries Create(Func<EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> resolve) => new(resolve);
}
