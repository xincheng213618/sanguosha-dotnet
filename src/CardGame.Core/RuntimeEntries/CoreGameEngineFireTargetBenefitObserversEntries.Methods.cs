#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFireTargetBenefitObserversEntries
{
    internal void AssertFireTargetBenefit(ProgramSkillFrame f) => _resolve().AssertFireTargetBenefit(f);
    internal ProgramSkillFrame? FireTargetBenefitObserverRoot(long? windowId = default) => _resolve().FireTargetBenefitObserverRoot(windowId);
    internal bool IsFireTargetBenefitProgramDying() => _resolve().IsFireTargetBenefitProgramDying();
    internal bool ValidFireTargetBenefit(ProgramSkillFrame f) => _resolve().ValidFireTargetBenefit(f);
}
