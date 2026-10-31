#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraEffectsAndRankGuessingEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginExtraEffectsAndRankGuess(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginExtraEffectsAndRankGuess(effect, supplied);
    internal bool CanContinueExtraEffectsAndRankGuess(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueExtraEffectsAndRankGuess(f);
    internal bool CanOfferExtraEffectsAndRankGuess(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferExtraEffectsAndRankGuess(candidate, trigger, context);
    internal void CompleteExtraEffectsAndRankGuess(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().CompleteExtraEffectsAndRankGuess(f, completed);
    internal int[] DeathSkillRecipients(int owner) => _resolve().DeathSkillRecipients(owner);
    internal bool DrainExtraEffectsAndRankGuessChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainExtraEffectsAndRankGuessChildren(f);
    internal global::CardGame.Core.Card[] ExtraEffectCosts(int owner, global::CardGame.Core.CardKind kind) => _resolve().ExtraEffectCosts(owner, kind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ExtraEffectsAndRankGuessChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExtraEffectsAndRankGuessChoices(f);
    internal bool ExtraEffectsFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().ExtraEffectsFinalizedAfterCategoryTargetWindow(query);
    internal global::CardGame.Core.CardConversionSource ExtraEffectsSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExtraEffectsSource(f);
    internal void FinishRankGuessGain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishRankGuessGain(f);
    internal bool IsExtraEffectsAndRankGuessMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().IsExtraEffectsAndRankGuessMovement(f, e, movement);
    internal void PublishExtraEffectsAndRankGuessChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishExtraEffectsAndRankGuessChoice(f);
    internal void RequireExtraEffectsAndRankGuess(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireExtraEffectsAndRankGuess(f);
    internal void ResolveExtraEffectsAndRankGuessChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveExtraEffectsAndRankGuessChoice(choice);
    internal bool ResumeExtraEffectsAndRankGuess(long id) => _resolve().ResumeExtraEffectsAndRankGuess(id);
    internal bool ReturnExtraEffectsAndRankGuessMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnExtraEffectsAndRankGuessMovement(f);
    internal void RevealNextRankGuessCard(global::CardGame.Core.ProgramSkillFrame f, bool? guessedGreater) => _resolve().RevealNextRankGuessCard(f, guessedGreater);
    internal bool TryGetExtraEffectUse(int owner, global::CardGame.Core.ProgramSkillWindowContext? context, scoped out global::CardGame.Core.CardUseFrame use) => _resolve().TryGetExtraEffectUse(owner, context, out use);
}
