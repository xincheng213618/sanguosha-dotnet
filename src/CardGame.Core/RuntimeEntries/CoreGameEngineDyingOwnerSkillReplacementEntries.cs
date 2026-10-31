#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnerSkillReplacementRuntime))]
internal sealed partial class CoreGameEngineDyingOwnerSkillReplacementEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnerSkillReplacementRuntime> _resolve;
    private CoreGameEngineDyingOwnerSkillReplacementEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnerSkillReplacementRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingOwnerSkillReplacementEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnerSkillReplacementRuntime> resolve) => new(resolve);
}
