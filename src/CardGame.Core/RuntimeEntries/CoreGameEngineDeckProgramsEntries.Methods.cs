#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeckProgramsEntries
{
    internal bool AllowsUseDeckSlashesThenShuffleTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsUseDeckSlashesThenShuffleTrigger(gate);
    internal void AwaitDeckProgramMovement(long id) => _resolve().AwaitDeckProgramMovement(id);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> BuildDeckEndExchangeActions(global::CardGame.Core.CharacterState provider) => _resolve().BuildDeckEndExchangeActions(provider);
    internal void CleanupDeckSlashSequence(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CleanupDeckSlashSequence(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DeckEndChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DeckEndChoices(frame);
    internal bool DeckProgramSourceValid(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DeckProgramSourceValid(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteDeckProgram(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExecuteDeckProgram(effect, frame);
    internal void FinishDeckEndExchange(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishDeckEndExchange(frame);
    internal void PublishDeckEndChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishDeckEndChoice(frame);
    internal void ResolveDeckEndChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveDeckEndChoice(selected);
    internal void ResumeDeckEndExchange(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ResumeDeckEndExchange(frame);
    internal void ResumeDeckSlashSequence(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ResumeDeckSlashSequence(frame, effect);
    internal void ShuffleDeckSequence(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShuffleDeckSequence(frame);
    internal bool TryExecuteDeckEndContribution(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction action, int cardId) => _resolve().TryExecuteDeckEndContribution(provider, owner, action, cardId);
    internal bool TryResumeDeckPrograms(long frameId) => _resolve().TryResumeDeckPrograms(frameId);
}
