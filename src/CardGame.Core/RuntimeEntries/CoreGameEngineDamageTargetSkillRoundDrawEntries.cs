#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetSkillRoundDrawEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetSkillRoundDrawRuntime> _resolve;
    private CoreGameEngineDamageTargetSkillRoundDrawEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetSkillRoundDrawRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetSkillRoundDrawEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetSkillRoundDrawRuntime> resolve) => new(resolve);
}
