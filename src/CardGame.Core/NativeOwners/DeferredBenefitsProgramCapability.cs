#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DeferredBenefitsProgramCapability(Func<RuntimeEntries.CoreGameEngineDeferredBenefitsProgramsEntries> getCoreGameEngineDeferredBenefitsProgramsRuntime) : IDeferredBenefitsProgramHost
{
    public SkillProgramStepOutcome RequestFactionRecovery(ProgramSkillFrame frame, string factionId) => getCoreGameEngineDeferredBenefitsProgramsRuntime().RequestProgramFactionRecovery(frame, factionId);
    public void SetNextTurnRuleModifier(ProgramSkillFrame frame, int targetSeat, SkillRuleQuery query, int amount) => getCoreGameEngineDeferredBenefitsProgramsRuntime().QueueProgramNextTurnRuleModifier(frame, targetSeat, query, amount);
    public SkillProgramStepOutcome WeaponDiscardOrDamageBonus(ProgramSkillFrame frame, int amount) => getCoreGameEngineDeferredBenefitsProgramsRuntime().BeginProgramWeaponDamageChoice(frame, amount);
    public void ResolveJudgmentColorBenefit(ProgramSkillFrame frame, int targetSeat, string sourceBind, int recovery) => getCoreGameEngineDeferredBenefitsProgramsRuntime().ResolveProgramJudgmentColorBenefit(frame, targetSeat, sourceBind, recovery);
}
