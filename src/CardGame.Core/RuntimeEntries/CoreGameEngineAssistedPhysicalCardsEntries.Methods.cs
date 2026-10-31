#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAssistedPhysicalCardsEntries
{
    internal void AssertAssistedPhysicalCardDrafts(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertAssistedPhysicalCardDrafts(frame, paused);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AssistedPhysicalSlashChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssistedPhysicalSlashChoices(frame);
    internal int[] AssistedPhysicalSlashTargets(int actorSeat) => _resolve().AssistedPhysicalSlashTargets(actorSeat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AssistedPhysicalSlashWithKenshangChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssistedPhysicalSlashWithKenshangChoices(frame);
    internal bool AssistedSlashPaymentHasRange(global::CardGame.Core.CharacterState actor, int targetSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource? conversionSource = default) => _resolve().AssistedSlashPaymentHasRange(actor, targetSeat, cards, conversionSource);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> OtherCardSelectionChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().OtherCardSelectionChoices(frame);
    internal void PublishAssistedPhysicalSlashPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishAssistedPhysicalSlashPrompt(frame);
    internal void PublishOtherCardSelection(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishOtherCardSelection(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome RequestProgramSlashAgainstChosenTarget(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, string bind, bool actorChoosesTarget) => _resolve().RequestProgramSlashAgainstChosenTarget(frame, actorSeat, bind, actorChoosesTarget);
    internal void ResolveAssistedPhysicalSlashChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveAssistedPhysicalSlashChoice(selected);
    internal void ResolveOtherCardSelectionChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveOtherCardSelectionChoice(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome TakeProgramSelectedTargetCards(global::CardGame.Core.ProgramSkillFrame frame, int sourceSeat, int count) => _resolve().TakeProgramSelectedTargetCards(frame, sourceSeat, count);
}
