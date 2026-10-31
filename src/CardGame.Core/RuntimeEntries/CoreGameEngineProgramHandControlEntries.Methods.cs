#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramHandControlEntries
{
    internal bool AllowsDrawTurnOwnerThenDiscardMaximumHandForDodgeActivation(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsDrawTurnOwnerThenDiscardMaximumHandForDodgeActivation(gate);
    internal void AssertProgramHandControlDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertProgramHandControlDraft(frame, paused);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildProgramHandControlChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BuildProgramHandControlChoices(frame);
    internal bool CanOfferProgramHandControl(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().CanOfferProgramHandControl(owner, trigger);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteProgramHandControl(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExecuteProgramHandControl(effect, frame);
    internal global::CardGame.Core.SkillProgramStepOutcome FinishProgramHandColorTake(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishProgramHandColorTake(frame);
    internal bool HandControlIsRed(int seat, global::CardGame.Core.Card card) => _resolve().HandControlIsRed(seat, card);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CharacterState> HandControlTakeTargets(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HandControlTakeTargets(frame);
    internal void PublishProgramHandControlChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishProgramHandControlChoice(frame);
    internal void ResolveProgramHandControlChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveProgramHandControlChoice(choice);
    internal void ReturnHandColorPublicHand(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnHandColorPublicHand(frame);
}
