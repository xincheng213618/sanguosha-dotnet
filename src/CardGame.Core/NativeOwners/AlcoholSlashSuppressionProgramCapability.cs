#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AlcoholSlashSuppressionProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholSlashSuppressionsEntries> getCoreGameEngineAlcoholSlashSuppressionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedDamagePointJudgmentsEntries> getCoreGameEngineOwnedDamagePointJudgmentsRuntime) : IAlcoholSlashSuppressionProgramHost
{
    public void SuppressOwnSkillAfterAlcoholSlashDamage(ProgramSkillFrame frame, string skillId) => getCoreGameEngineAlcoholSlashSuppressionsRuntime().SuppressOwnSkillAfterAlcoholSlashDamage(frame, skillId);
    public SkillProgramStepOutcome StartOwnedDamagePointJudgment(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineOwnedDamagePointJudgmentsRuntime().BeginOwnedDamagePointJudgment(frame, effect);
}
