#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageTargetSkillRoundDrawRuntime))]
internal sealed partial class CoreGameEngineDamageTargetSkillRoundDrawEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageTargetSkillRoundDrawRuntime> _resolve;
    private CoreGameEngineDamageTargetSkillRoundDrawEntries(Func<EngineOwners.CoreGameEngineDamageTargetSkillRoundDrawRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetSkillRoundDrawEntries Create(Func<EngineOwners.CoreGameEngineDamageTargetSkillRoundDrawRuntime> resolve) => new(resolve);
}
