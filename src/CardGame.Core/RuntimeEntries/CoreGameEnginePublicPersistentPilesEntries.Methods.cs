#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPersistentPilesEntries
{
    internal void AssertPublicPileDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertPublicPileDraft(frame, paused);
    internal void CleanupLostPublicPersistentPiles() => _resolve().CleanupLostPublicPersistentPiles();
    internal global::CardGame.Core.ProgramPublicPileDraft CreatePublicPileDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PublicPersistentPileSource source) => _resolve().CreatePublicPileDraft(frame, effect, source);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecutePublicPile(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExecutePublicPile(effect, frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileExchangeOwnedCards(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublicPileExchangeOwnedCards(frame);
    internal bool PublicPileSourceSatisfiesFrozenTrigger(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PublicPersistentPileSource source) => _resolve().PublicPileSourceSatisfiesFrozenTrigger(frame, source);
    internal void PublishPublicPileChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishPublicPileChoice(frame);
    internal void ResolvePublicPileChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolvePublicPileChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiPublicPileChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiPublicPileChoice(decision, frame);
}
