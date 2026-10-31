#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOwnCardDiscardsEntries
{
    internal IReadOnlyList<PromptChoice> BuildOwnCardDiscardChoices(long frameId, int chooserSeat, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => _resolve().BuildOwnCardDiscardChoices(frameId, chooserSeat, zones, reason);
    internal SkillProgramStepOutcome ChooseProgramOwnCardDiscard(ProgramSkillFrame frame, ProgramParticipantReference? chooser, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => _resolve().ChooseProgramOwnCardDiscard(frame, chooser, zones, reason);
    internal void ResolveProgramOwnCardDiscardChoice(PromptChoice selected) => _resolve().ResolveProgramOwnCardDiscardChoice(selected);
    internal PromptChoice SelectAiProgramOwnCardDiscard(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiProgramOwnCardDiscard(decision, frame);
}
