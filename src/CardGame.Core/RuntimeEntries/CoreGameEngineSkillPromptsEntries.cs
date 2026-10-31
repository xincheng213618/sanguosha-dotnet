#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSkillPromptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillPromptsRuntime> _resolve;
    private CoreGameEngineSkillPromptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillPromptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSkillPromptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillPromptsRuntime> resolve) => new(resolve);
}
