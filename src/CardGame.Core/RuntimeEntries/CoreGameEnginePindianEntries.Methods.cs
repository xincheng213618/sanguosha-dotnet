#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianEntries
{
    internal void AssertPindianInvariant() => _resolve().AssertPindianInvariant();
    internal int[] AvailablePindianCards(PindianResult result) => _resolve().AvailablePindianCards(result);
    internal void BeginSharedPindian(long parentId, SkillPromptPresentation presentation, int sourceSeat, int? opponentSeat = default, int? sourceCardId = default, string? programResultBind = default, SkillProgramCardSetVisibility programResultVisibility = (SkillProgramCardSetVisibility)1, PindianOwnedPileSource? ownedPileSource = default) => _resolve().BeginSharedPindian(parentId, presentation, sourceSeat, opponentSeat, sourceCardId, programResultBind, programResultVisibility, ownedPileSource);
    internal void CompletePindian(PindianFrame frame) => _resolve().CompletePindian(frame);
    internal bool HasPindianChild(long parentId) => _resolve().HasPindianChild(parentId);
    internal void PublishPindianSelection(PindianFrame frame) => _resolve().PublishPindianSelection(frame);
    internal void ResolvePendingAiPindian() => _resolve().ResolvePendingAiPindian();
    internal void ResolvePindianChoice(PromptChoice selected) => _resolve().ResolvePindianChoice(selected);
    internal void RevealPindian(PindianFrame frame, int opponentCardId, bool opponentTop = false) => _resolve().RevealPindian(frame, opponentCardId, opponentTop);
    internal void SetPindianPrompt(PindianFrame frame, int playerSeat, DecisionKind kind, string prompt, IReadOnlyList<PromptChoice> choices, IReadOnlyList<int> cards, IReadOnlyList<int> targets, SkillPromptPresentation presentation) => _resolve().SetPindianPrompt(frame, playerSeat, kind, prompt, choices, cards, targets, presentation);
}
