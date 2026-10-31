#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSkillPromptsRuntime))]
internal sealed partial class CoreGameEngineSkillPromptsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSkillPromptsRuntime> _resolve;
    private CoreGameEngineSkillPromptsEntries(Func<EngineOwners.CoreGameEngineSkillPromptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSkillPromptsEntries Create(Func<EngineOwners.CoreGameEngineSkillPromptsRuntime> resolve) => new(resolve);
}
