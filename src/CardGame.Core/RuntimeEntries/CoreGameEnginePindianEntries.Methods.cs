#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianEntries
{
    internal void AssertPindianInvariant() => _resolve().AssertPindianInvariant();
    internal int[] AvailablePindianCards(global::CardGame.Core.PindianResult result) => _resolve().AvailablePindianCards(result);
    internal void BeginSharedPindian(long parentId, global::CardGame.Core.SkillPromptPresentation presentation, int sourceSeat, int? opponentSeat = default, int? sourceCardId = default, string? programResultBind = default, global::CardGame.Core.SkillProgramCardSetVisibility programResultVisibility = (global::CardGame.Core.SkillProgramCardSetVisibility)1, global::CardGame.Core.PindianOwnedPileSource? ownedPileSource = default) => _resolve().BeginSharedPindian(parentId, presentation, sourceSeat, opponentSeat, sourceCardId, programResultBind, programResultVisibility, ownedPileSource);
    internal void CompletePindian(global::CardGame.Core.PindianFrame frame) => _resolve().CompletePindian(frame);
    internal bool HasPindianChild(long parentId) => _resolve().HasPindianChild(parentId);
    internal void PublishPindianSelection(global::CardGame.Core.PindianFrame frame) => _resolve().PublishPindianSelection(frame);
    internal void ResolvePendingAiPindian() => _resolve().ResolvePendingAiPindian();
    internal void ResolvePindianChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolvePindianChoice(selected);
    internal void RevealPindian(global::CardGame.Core.PindianFrame frame, int opponentCardId, bool opponentTop = false) => _resolve().RevealPindian(frame, opponentCardId, opponentTop);
    internal void SetPindianPrompt(global::CardGame.Core.PindianFrame frame, int playerSeat, global::CardGame.Core.DecisionKind kind, string prompt, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.SkillPromptPresentation presentation) => _resolve().SetPindianPrompt(frame, playerSeat, kind, prompt, choices, cards, targets, presentation);
}
