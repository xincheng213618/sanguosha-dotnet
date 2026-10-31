#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOtherOwnedCardDiscardsEntries
{
    internal IReadOnlyList<PromptChoice> BuildOtherOwnedCardDiscardChoices(long frameId, int chooserSeat, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => _resolve().BuildOtherOwnedCardDiscardChoices(frameId, chooserSeat, zones, reason);
    internal SkillProgramStepOutcome ChooseProgramOtherOwnedCardDiscard(ProgramSkillFrame frame, ProgramParticipantReference chooser, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => _resolve().ChooseProgramOtherOwnedCardDiscard(frame, chooser, zones, reason);
    internal void ResolveProgramOtherOwnedCardDiscardChoice(PromptChoice selected) => _resolve().ResolveProgramOtherOwnedCardDiscardChoice(selected);
    internal PromptChoice SelectAiProgramOtherOwnedCardDiscard(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiProgramOtherOwnedCardDiscard(decision, frame);
}
