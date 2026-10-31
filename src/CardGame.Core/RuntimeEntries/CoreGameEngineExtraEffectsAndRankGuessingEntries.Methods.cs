#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraEffectsAndRankGuessingEntries
{
    internal SkillProgramStepOutcome BeginExtraEffectsAndRankGuess(SkillProgramEffect effect, ProgramSkillFrame supplied) => _resolve().BeginExtraEffectsAndRankGuess(effect, supplied);
    internal bool CanContinueExtraEffectsAndRankGuess(ProgramSkillFrame f) => _resolve().CanContinueExtraEffectsAndRankGuess(f);
    internal bool CanOfferExtraEffectsAndRankGuess(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferExtraEffectsAndRankGuess(candidate, trigger, context);
    internal void CompleteExtraEffectsAndRankGuess(ProgramSkillFrame f, bool completed) => _resolve().CompleteExtraEffectsAndRankGuess(f, completed);
    internal int[] DeathSkillRecipients(int owner) => _resolve().DeathSkillRecipients(owner);
    internal bool DrainExtraEffectsAndRankGuessChildren(ProgramSkillFrame f) => _resolve().DrainExtraEffectsAndRankGuessChildren(f);
    internal Card[] ExtraEffectCosts(int owner, CardKind kind) => _resolve().ExtraEffectCosts(owner, kind);
    internal IReadOnlyList<PromptChoice> ExtraEffectsAndRankGuessChoices(ProgramSkillFrame f) => _resolve().ExtraEffectsAndRankGuessChoices(f);
    internal bool ExtraEffectsFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().ExtraEffectsFinalizedAfterCategoryTargetWindow(query);
    internal CardConversionSource ExtraEffectsSource(ProgramSkillFrame f) => _resolve().ExtraEffectsSource(f);
    internal void FinishRankGuessGain(ProgramSkillFrame f) => _resolve().FinishRankGuessGain(f);
    internal bool IsExtraEffectsAndRankGuessMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation movement) => _resolve().IsExtraEffectsAndRankGuessMovement(f, e, movement);
    internal void PublishExtraEffectsAndRankGuessChoice(ProgramSkillFrame f) => _resolve().PublishExtraEffectsAndRankGuessChoice(f);
    internal void RequireExtraEffectsAndRankGuess(ProgramSkillFrame f) => _resolve().RequireExtraEffectsAndRankGuess(f);
    internal void ResolveExtraEffectsAndRankGuessChoice(PromptChoice choice) => _resolve().ResolveExtraEffectsAndRankGuessChoice(choice);
    internal bool ResumeExtraEffectsAndRankGuess(long id) => _resolve().ResumeExtraEffectsAndRankGuess(id);
    internal bool ReturnExtraEffectsAndRankGuessMovement(ProgramSkillFrame f) => _resolve().ReturnExtraEffectsAndRankGuessMovement(f);
    internal void RevealNextRankGuessCard(ProgramSkillFrame f, bool? guessedGreater) => _resolve().RevealNextRankGuessCard(f, guessedGreater);
    internal bool TryGetExtraEffectUse(int owner, ProgramSkillWindowContext? context, scoped out CardUseFrame use) => _resolve().TryGetExtraEffectUse(owner, context, out use);
}
