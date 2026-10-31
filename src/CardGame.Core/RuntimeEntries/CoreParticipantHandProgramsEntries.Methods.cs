#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreParticipantHandProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundParticipantGift(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation target, global::CardGame.Core.CardMoveReason reason) => _resolve().BeginBoundParticipantGift(frame, bind, ids, target, reason);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHandRepayment(long frameId, int sourceSeat) => _resolve().BeginHandRepayment(frameId, sourceSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPrivateHandTake(long frameId, int sourceSeat, int count) => _resolve().BeginPrivateHandTake(frameId, sourceSeat, count);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> GetParticipantPrivatelyViewedCards(int viewerSeat) => _resolve().GetParticipantPrivatelyViewedCards(viewerSeat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HandRepaymentChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HandRepaymentChoices(frame);
    internal global::CardGame.Core.SkillProgramEffect ParticipantHandPaused(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ParticipantHandPaused(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PrivateHandTakeChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PrivateHandTakeChoices(frame);
    internal void PublishParticipantHandChoice(global::CardGame.Core.ProgramSkillFrame frame, int chooser, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string text, int target) => _resolve().PublishParticipantHandChoice(frame, chooser, choices, text, target);
    internal void ResolveParticipantHandChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveParticipantHandChoice(selected);
    internal bool ResumeParticipantHandPayment(long frameId) => _resolve().ResumeParticipantHandPayment(frameId);
    internal global::CardGame.Core.PromptChoice SelectAiParticipantHandChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiParticipantHandChoice(decision, frame);
    internal void ValidateBoundParticipantGiftWindow(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidateBoundParticipantGiftWindow(frame);
    internal void ValidateParticipantHandDrafts(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().ValidateParticipantHandDrafts(frame, paused);
}
