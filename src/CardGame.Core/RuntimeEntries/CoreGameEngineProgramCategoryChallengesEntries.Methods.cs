#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCategoryChallengesEntries
{
    internal bool AreProgramBoundCardsSameColor(ProgramSkillFrame frame, string sourceBind) => _resolve().AreProgramBoundCardsSameColor(frame, sourceBind);
    internal IReadOnlyList<PromptChoice> BuildDifferentCategoryDiscardChoices(long frameId, int chooserSeat, int cardOwnerSeat, IReadOnlyList<CardZoneKind> zones, string sourceBind, string resultBind, CardMoveReason reason, IReadOnlyCollection<SkillProgramCardCategory> excludedCategories) => _resolve().BuildDifferentCategoryDiscardChoices(frameId, chooserSeat, cardOwnerSeat, zones, sourceBind, resultBind, reason, excludedCategories);
    internal void CaptureProgramSelectedCards(ProgramSkillFrame frame, string resultBind) => _resolve().CaptureProgramSelectedCards(frame, resultBind);
    internal SkillProgramStepOutcome ChooseProgramDifferentCategoryDiscard(ProgramSkillFrame frame, ProgramParticipantReference chooser, ProgramParticipantReference cardOwner, IReadOnlyList<CardZoneKind> zones, string sourceBind, string resultBind, CardMoveReason reason) => _resolve().ChooseProgramDifferentCategoryDiscard(frame, chooser, cardOwner, zones, sourceBind, resultBind, reason);
    internal void CommitProgramCategoryDiscard(ProgramSkillFrame frame, int chooserSeat, string sourceBind, string resultBind, int? discardedCardId) => _resolve().CommitProgramCategoryDiscard(frame, chooserSeat, sourceBind, resultBind, discardedCardId);
    internal bool DoProgramBoundCardCategoryMatchAction(ProgramSkillFrame frame, string sourceBind) => _resolve().DoProgramBoundCardCategoryMatchAction(frame, sourceBind);
    internal bool DoProgramBoundCardsMatchCategories(ProgramSkillFrame frame, string sourceBind, IReadOnlyList<SkillProgramCardCategory> categories) => _resolve().DoProgramBoundCardsMatchCategories(frame, sourceBind, categories);
    internal bool DoProgramBoundCardsMatchKinds(ProgramSkillFrame frame, string sourceBind, IReadOnlyList<CardKind> kinds) => _resolve().DoProgramBoundCardsMatchKinds(frame, sourceBind, kinds);
    internal bool DoProgramBoundCardsMatchSuits(ProgramSkillFrame frame, string sourceBind, IReadOnlyList<Suit> suits) => _resolve().DoProgramBoundCardsMatchSuits(frame, sourceBind, suits);
    internal void ResolveProgramDifferentCategoryDiscardChoice(PromptChoice selected) => _resolve().ResolveProgramDifferentCategoryDiscardChoice(selected);
    internal void RevealProgramBoundCards(ProgramSkillFrame frame, string sourceBind) => _resolve().RevealProgramBoundCards(frame, sourceBind);
    internal PromptChoice SelectAiProgramCategoryDiscard(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiProgramCategoryDiscard(decision, frame);
}
