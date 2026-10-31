#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFireTargetBenefitObserversEntries
{
    internal void AssertFireTargetBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertFireTargetBenefit(f);
    internal global::CardGame.Core.ProgramSkillFrame? FireTargetBenefitObserverRoot(long? windowId = default) => _resolve().FireTargetBenefitObserverRoot(windowId);
    internal bool HasFireTargetBenefitObserver(long windowId) => _resolve().HasFireTargetBenefitObserver(windowId);
    internal bool IsFireTargetBenefitProgramDying() => _resolve().IsFireTargetBenefitProgramDying();
    internal bool ValidFireTargetBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidFireTargetBenefit(f);
}
