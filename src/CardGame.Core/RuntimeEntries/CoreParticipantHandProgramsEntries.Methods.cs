#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreParticipantHandProgramsEntries
{
    internal SkillProgramStepOutcome BeginBoundParticipantGift(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, CardLocation target, CardMoveReason reason) => _resolve().BeginBoundParticipantGift(frame, bind, ids, target, reason);
    internal SkillProgramStepOutcome BeginHandRepayment(long frameId, int sourceSeat) => _resolve().BeginHandRepayment(frameId, sourceSeat);
    internal SkillProgramStepOutcome BeginPrivateHandTake(long frameId, int sourceSeat, int count) => _resolve().BeginPrivateHandTake(frameId, sourceSeat, count);
    internal IEnumerable<CardSnapshot> GetParticipantPrivatelyViewedCards(int viewerSeat) => _resolve().GetParticipantPrivatelyViewedCards(viewerSeat);
    internal IReadOnlyList<PromptChoice> HandRepaymentChoices(ProgramSkillFrame frame) => _resolve().HandRepaymentChoices(frame);
    internal SkillProgramEffect ParticipantHandPaused(ProgramSkillFrame frame) => _resolve().ParticipantHandPaused(frame);
    internal IReadOnlyList<PromptChoice> PrivateHandTakeChoices(ProgramSkillFrame frame) => _resolve().PrivateHandTakeChoices(frame);
    internal void PublishParticipantHandChoice(ProgramSkillFrame frame, int chooser, IReadOnlyList<PromptChoice> choices, string text, int target) => _resolve().PublishParticipantHandChoice(frame, chooser, choices, text, target);
    internal void ResolveParticipantHandChoice(PromptChoice selected) => _resolve().ResolveParticipantHandChoice(selected);
    internal bool ResumeParticipantHandPayment(long frameId) => _resolve().ResumeParticipantHandPayment(frameId);
    internal PromptChoice SelectAiParticipantHandChoice(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiParticipantHandChoice(decision, frame);
    internal void ValidateBoundParticipantGiftWindow(ProgramSkillFrame frame) => _resolve().ValidateBoundParticipantGiftWindow(frame);
    internal void ValidateParticipantHandDrafts(ProgramSkillFrame frame, SkillProgramEffect paused) => _resolve().ValidateParticipantHandDrafts(frame, paused);
}
