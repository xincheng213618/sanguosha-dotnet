#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DamageTargetSkillRoundCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetSkillRoundDrawEntries> getCoreGameEngineDamageTargetSkillRoundDrawRuntime) : IDamageTargetSkillRoundDrawHost
{
    public SkillProgramStepOutcome GrantDamageTargetSkillAndDrawByRound(ProgramSkillFrame frame) => getCoreGameEngineDamageTargetSkillRoundDrawRuntime().GrantDamageTargetSkillAndDrawByRound(frame);
}
