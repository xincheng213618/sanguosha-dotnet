#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAssistedPhysicalCardsEntries
{
    internal IReadOnlyList<PromptChoice> AssistedPhysicalSlashChoices(ProgramSkillFrame frame) => _resolve().AssistedPhysicalSlashChoices(frame);
    internal int[] AssistedPhysicalSlashTargets(int actorSeat) => _resolve().AssistedPhysicalSlashTargets(actorSeat);
    internal bool AssistedSlashPaymentHasRange(CharacterState actor, int targetSeat, IReadOnlyList<Card> cards, CardConversionSource? conversionSource = default) => _resolve().AssistedSlashPaymentHasRange(actor, targetSeat, cards, conversionSource);
    internal IReadOnlyList<PromptChoice> OtherCardSelectionChoices(ProgramSkillFrame frame) => _resolve().OtherCardSelectionChoices(frame);
    internal void PublishAssistedPhysicalSlashPrompt(ProgramSkillFrame frame) => _resolve().PublishAssistedPhysicalSlashPrompt(frame);
    internal void PublishOtherCardSelection(ProgramSkillFrame frame) => _resolve().PublishOtherCardSelection(frame);
    internal SkillProgramStepOutcome RequestProgramSlashAgainstChosenTarget(ProgramSkillFrame frame, int actorSeat, string bind, bool actorChoosesTarget) => _resolve().RequestProgramSlashAgainstChosenTarget(frame, actorSeat, bind, actorChoosesTarget);
    internal void ResolveAssistedPhysicalSlashChoice(PromptChoice selected) => _resolve().ResolveAssistedPhysicalSlashChoice(selected);
    internal void ResolveOtherCardSelectionChoice(PromptChoice selected) => _resolve().ResolveOtherCardSelectionChoice(selected);
    internal SkillProgramStepOutcome TakeProgramSelectedTargetCards(ProgramSkillFrame frame, int sourceSeat, int count) => _resolve().TakeProgramSelectedTargetCards(frame, sourceSeat, count);
}
