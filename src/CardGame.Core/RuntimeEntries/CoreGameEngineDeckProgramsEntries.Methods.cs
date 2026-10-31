#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeckProgramsEntries
{
    internal bool AllowsUseDeckSlashesThenShuffleTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsUseDeckSlashesThenShuffleTrigger(gate);
    internal void AwaitDeckProgramMovement(long id) => _resolve().AwaitDeckProgramMovement(id);
    internal IEnumerable<LegalAction> BuildDeckEndExchangeActions(CharacterState provider) => _resolve().BuildDeckEndExchangeActions(provider);
    internal void CleanupDeckSlashSequence(ProgramSkillFrame frame) => _resolve().CleanupDeckSlashSequence(frame);
    internal IReadOnlyList<PromptChoice> DeckEndChoices(ProgramSkillFrame frame) => _resolve().DeckEndChoices(frame);
    internal bool DeckProgramSourceValid(ProgramSkillFrame frame) => _resolve().DeckProgramSourceValid(frame);
    internal SkillProgramStepOutcome ExecuteDeckProgram(SkillProgramEffect effect, ProgramSkillFrame frame) => _resolve().ExecuteDeckProgram(effect, frame);
    internal void FinishDeckEndExchange(ProgramSkillFrame frame) => _resolve().FinishDeckEndExchange(frame);
    internal void PublishDeckEndChoice(ProgramSkillFrame frame) => _resolve().PublishDeckEndChoice(frame);
    internal void ResolveDeckEndChoice(PromptChoice selected) => _resolve().ResolveDeckEndChoice(selected);
    internal void ResumeDeckEndExchange(ProgramSkillFrame frame) => _resolve().ResumeDeckEndExchange(frame);
    internal void ResumeDeckSlashSequence(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ResumeDeckSlashSequence(frame, effect);
    internal void ShuffleDeckSequence(ProgramSkillFrame frame) => _resolve().ShuffleDeckSequence(frame);
    internal bool TryExecuteDeckEndContribution(CharacterState provider, CharacterState owner, LegalAction action, int cardId) => _resolve().TryExecuteDeckEndContribution(provider, owner, action, cardId);
    internal bool TryResumeDeckPrograms(long frameId) => _resolve().TryResumeDeckPrograms(frameId);
}
