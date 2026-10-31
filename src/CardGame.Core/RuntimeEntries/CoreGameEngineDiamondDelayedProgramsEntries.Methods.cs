#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDiamondDelayedProgramsEntries
{
    internal SkillProgramStepOutcome BeginDiamondDelayed(ProgramSkillFrame frame) => _resolve().BeginDiamondDelayed(frame);
    internal bool CanActivateDiamondDelayed(CharacterState owner, SkillProgramActivation activation, string skillId) => _resolve().CanActivateDiamondDelayed(owner, activation, skillId);
    internal void ContinueProgramAfterDiamondDelayedUse(long completedUseId) => _resolve().ContinueProgramAfterDiamondDelayedUse(completedUseId);
    internal IReadOnlyList<PromptChoice> DiamondDelayedChoices(ProgramSkillFrame frame) => _resolve().DiamondDelayedChoices(frame);
    internal IEnumerable<(int Seat, Card Card)> DiamondJudgments() => _resolve().DiamondJudgments();
    internal bool DiamondPaymentLegal(int ownerSeat, Card card, string skillId, string? instanceId = default) => _resolve().DiamondPaymentLegal(ownerSeat, card, skillId, instanceId);
    internal bool DiamondUseTargetLegal(int ownerSeat, Card card, int targetSeat, string skillId, string? instanceId = default) => _resolve().DiamondUseTargetLegal(ownerSeat, card, targetSeat, skillId, instanceId);
    internal bool IsDiamondDelayedChoiceLegal(PromptChoice choice) => _resolve().IsDiamondDelayedChoiceLegal(choice);
    internal void PublishDiamondDelayed(ProgramSkillFrame frame) => _resolve().PublishDiamondDelayed(frame);
    internal void ResolveDiamondDelayedChoice(PromptChoice choice) => _resolve().ResolveDiamondDelayedChoice(choice);
    internal bool ResumeDiamondDelayed(long frameId) => _resolve().ResumeDiamondDelayed(frameId);
}
