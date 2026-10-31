#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDyingOwnerSkillReplacementRuntime))]
internal sealed partial class CoreGameEngineDyingOwnerSkillReplacementEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDyingOwnerSkillReplacementRuntime> _resolve;
    private CoreGameEngineDyingOwnerSkillReplacementEntries(Func<EngineOwners.CoreGameEngineDyingOwnerSkillReplacementRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingOwnerSkillReplacementEntries Create(Func<EngineOwners.CoreGameEngineDyingOwnerSkillReplacementRuntime> resolve) => new(resolve);
}
