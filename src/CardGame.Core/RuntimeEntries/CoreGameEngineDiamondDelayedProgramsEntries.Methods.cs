#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDiamondDelayedProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDiamondDelayed(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginDiamondDelayed(frame);
    internal bool CanActivateDiamondDelayed(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramActivation activation, string skillId) => _resolve().CanActivateDiamondDelayed(owner, activation, skillId);
    internal void ContinueProgramAfterDiamondDelayedUse(long completedUseId) => _resolve().ContinueProgramAfterDiamondDelayedUse(completedUseId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DiamondDelayedChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DiamondDelayedChoices(frame);
    internal global::System.Collections.Generic.IEnumerable<(int Seat, global::CardGame.Core.Card Card)> DiamondJudgments() => _resolve().DiamondJudgments();
    internal bool DiamondPaymentLegal(int ownerSeat, global::CardGame.Core.Card card, string skillId, string? instanceId = default) => _resolve().DiamondPaymentLegal(ownerSeat, card, skillId, instanceId);
    internal bool DiamondUseTargetLegal(int ownerSeat, global::CardGame.Core.Card card, int targetSeat, string skillId, string? instanceId = default) => _resolve().DiamondUseTargetLegal(ownerSeat, card, targetSeat, skillId, instanceId);
    internal bool IsDiamondDelayedChoiceLegal(global::CardGame.Core.PromptChoice choice) => _resolve().IsDiamondDelayedChoiceLegal(choice);
    internal void PublishDiamondDelayed(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishDiamondDelayed(frame);
    internal void ResolveDiamondDelayedChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveDiamondDelayedChoice(choice);
    internal bool ResumeDiamondDelayed(long frameId) => _resolve().ResumeDiamondDelayed(frameId);
}
