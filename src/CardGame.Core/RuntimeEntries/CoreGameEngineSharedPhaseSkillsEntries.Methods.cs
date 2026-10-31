#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSharedPhaseSkillsEntries
{
    internal void AnswerRandomPhaseSkillOffer(PromptChoice choice) => _resolve().AnswerRandomPhaseSkillOffer(choice);
    internal IEnumerable<LegalAction> BuildSharedProgramActivations(CharacterState actor) => _resolve().BuildSharedProgramActivations(actor);
    internal bool IsLiveRandomPhaseSkillRecipient(ProgramSkillFrame frame, int recipientSeat, SkillGrantPhaseExpiry phase) => _resolve().IsLiveRandomPhaseSkillRecipient(frame, recipientSeat, phase);
    internal SkillProgramStepOutcome OfferRandomPhaseSkill(ProgramSkillFrame supplied, SkillProgramEffect effect, int recipientSeat) => _resolve().OfferRandomPhaseSkill(supplied, effect, recipientSeat);
    internal void PresentRandomPhaseSkillOffer(ProgramSkillFrame frame) => _resolve().PresentRandomPhaseSkillOffer(frame);
    internal bool RejectUnboundSharedProgramActivation(UseProgramSkillCommand command, scoped out CommandResult result) => _resolve().RejectUnboundSharedProgramActivation(command, out result);
    internal PromptChoice SelectRandomPhaseSkillForAi(PendingDecision decision) => _resolve().SelectRandomPhaseSkillForAi(decision);
    internal bool TryExecuteSharedProgramActivation(CharacterState actor, LegalAction action, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => _resolve().TryExecuteSharedProgramActivation(actor, action, cards, targets);
    internal SkillGrantPhaseExpiry CurrentSharedSkillPhase { get => _resolve().CurrentSharedSkillPhase; }
}
