#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRandomDamageTargetSkillsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime> _resolve;
    private CoreGameEngineRandomDamageTargetSkillsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRandomDamageTargetSkillsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime> resolve) => new(resolve);
}
