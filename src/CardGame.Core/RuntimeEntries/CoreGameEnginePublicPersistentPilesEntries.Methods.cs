#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPersistentPilesEntries
{
    internal void AssertPublicPileDraft(ProgramSkillFrame frame, SkillProgramEffect paused) => _resolve().AssertPublicPileDraft(frame, paused);
    internal void CleanupLostPublicPersistentPiles() => _resolve().CleanupLostPublicPersistentPiles();
    internal ProgramPublicPileDraft CreatePublicPileDraft(ProgramSkillFrame frame, SkillProgramEffect effect, PublicPersistentPileSource source) => _resolve().CreatePublicPileDraft(frame, effect, source);
    internal SkillProgramStepOutcome ExecutePublicPile(SkillProgramEffect effect, ProgramSkillFrame frame) => _resolve().ExecutePublicPile(effect, frame);
    internal IReadOnlyList<Card> PublicPileExchangeOwnedCards(ProgramSkillFrame frame) => _resolve().PublicPileExchangeOwnedCards(frame);
    internal bool PublicPileSourceSatisfiesFrozenTrigger(ProgramSkillFrame frame, PublicPersistentPileSource source) => _resolve().PublicPileSourceSatisfiesFrozenTrigger(frame, source);
    internal void PublishPublicPileChoice(ProgramSkillFrame frame) => _resolve().PublishPublicPileChoice(frame);
    internal void ResolvePublicPileChoice(PromptChoice selected) => _resolve().ResolvePublicPileChoice(selected);
    internal PromptChoice SelectAiPublicPileChoice(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiPublicPileChoice(decision, frame);
}
