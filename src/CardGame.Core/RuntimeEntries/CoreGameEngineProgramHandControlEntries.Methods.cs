#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramHandControlEntries
{
    internal bool AllowsDrawTurnOwnerThenDiscardMaximumHandForDodgeActivation(ProgramActivationLegalityContext gate) => _resolve().AllowsDrawTurnOwnerThenDiscardMaximumHandForDodgeActivation(gate);
    internal IReadOnlyList<PromptChoice> BuildProgramHandControlChoices(ProgramSkillFrame frame) => _resolve().BuildProgramHandControlChoices(frame);
    internal bool CanOfferProgramHandControl(CharacterState owner, SkillProgramTrigger trigger) => _resolve().CanOfferProgramHandControl(owner, trigger);
    internal SkillProgramStepOutcome ExecuteProgramHandControl(SkillProgramEffect effect, ProgramSkillFrame frame) => _resolve().ExecuteProgramHandControl(effect, frame);
    internal SkillProgramStepOutcome FinishProgramHandColorTake(ProgramSkillFrame frame) => _resolve().FinishProgramHandColorTake(frame);
    internal bool HandControlIsRed(int seat, Card card) => _resolve().HandControlIsRed(seat, card);
    internal IEnumerable<CharacterState> HandControlTakeTargets(ProgramSkillFrame frame) => _resolve().HandControlTakeTargets(frame);
    internal void PublishProgramHandControlChoice(ProgramSkillFrame frame) => _resolve().PublishProgramHandControlChoice(frame);
    internal void ResolveProgramHandControlChoice(PromptChoice choice) => _resolve().ResolveProgramHandControlChoice(choice);
    internal void ReturnHandColorPublicHand(ProgramSkillFrame frame) => _resolve().ReturnHandColorPublicHand(frame);
}
