#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime))]
internal sealed partial class CoreGameEngineRandomDamageTargetSkillsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime> _resolve;
    private CoreGameEngineRandomDamageTargetSkillsEntries(Func<EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRandomDamageTargetSkillsEntries Create(Func<EngineOwners.CoreGameEngineRandomDamageTargetSkillsRuntime> resolve) => new(resolve);
}
