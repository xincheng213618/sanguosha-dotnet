#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOwnCardDiscardsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildOwnCardDiscardChoices(long frameId, int chooserSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.CardMoveReason reason) => _resolve().BuildOwnCardDiscardChoices(frameId, chooserSeat, zones, reason);
    internal global::CardGame.Core.SkillProgramStepOutcome ChooseProgramOwnCardDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference? chooser, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.CardMoveReason reason) => _resolve().ChooseProgramOwnCardDiscard(frame, chooser, zones, reason);
    internal void ResolveProgramOwnCardDiscardChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramOwnCardDiscardChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiProgramOwnCardDiscard(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiProgramOwnCardDiscard(decision, frame);
}
