#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDirectSkillDamageRuntime))]
internal sealed partial class CoreGameEngineDirectSkillDamageEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDirectSkillDamageRuntime> _resolve;
    private CoreGameEngineDirectSkillDamageEntries(Func<EngineOwners.CoreGameEngineDirectSkillDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDirectSkillDamageEntries Create(Func<EngineOwners.CoreGameEngineDirectSkillDamageRuntime> resolve) => new(resolve);
}
