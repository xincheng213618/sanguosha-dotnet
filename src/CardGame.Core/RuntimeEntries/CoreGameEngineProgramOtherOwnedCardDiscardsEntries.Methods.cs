#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOtherOwnedCardDiscardsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildOtherOwnedCardDiscardChoices(long frameId, int chooserSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.CardMoveReason reason) => _resolve().BuildOtherOwnedCardDiscardChoices(frameId, chooserSeat, zones, reason);
    internal global::CardGame.Core.SkillProgramStepOutcome ChooseProgramOtherOwnedCardDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference chooser, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.CardMoveReason reason) => _resolve().ChooseProgramOtherOwnedCardDiscard(frame, chooser, zones, reason);
    internal void ResolveProgramOtherOwnedCardDiscardChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramOtherOwnedCardDiscardChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiProgramOtherOwnedCardDiscard(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiProgramOtherOwnedCardDiscard(decision, frame);
}
