#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime))]
internal sealed partial class CoreGameEngineRandomDamageTargetSkillsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime> _resolve;
    private CoreGameEngineRandomDamageTargetSkillsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRandomDamageTargetSkillsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime> resolve) => new(resolve);
}
