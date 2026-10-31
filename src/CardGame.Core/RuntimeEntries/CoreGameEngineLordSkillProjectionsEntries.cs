#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime))]
internal sealed partial class CoreGameEngineLordSkillProjectionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> _resolve;
    private CoreGameEngineLordSkillProjectionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLordSkillProjectionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> resolve) => new(resolve);
}
