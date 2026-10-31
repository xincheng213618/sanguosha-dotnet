#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSharedPhaseSkillsEntries
{
    internal void AnswerRandomPhaseSkillOffer(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerRandomPhaseSkillOffer(choice);
    internal void AssertSharedPhaseSkillFrame(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertSharedPhaseSkillFrame(frame);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> BuildSharedProgramActivations(global::CardGame.Core.CharacterState actor) => _resolve().BuildSharedProgramActivations(actor);
    internal bool IsLiveRandomPhaseSkillRecipient(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, global::CardGame.Core.SkillGrantPhaseExpiry phase) => _resolve().IsLiveRandomPhaseSkillRecipient(frame, recipientSeat, phase);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferRandomPhaseSkill(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect, int recipientSeat) => _resolve().OfferRandomPhaseSkill(supplied, effect, recipientSeat);
    internal void PresentRandomPhaseSkillOffer(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PresentRandomPhaseSkillOffer(frame);
    internal bool RejectUnboundSharedProgramActivation(global::CardGame.Core.UseProgramSkillCommand command, scoped out global::CardGame.Core.CommandResult result) => _resolve().RejectUnboundSharedProgramActivation(command, out result);
    internal global::CardGame.Core.PromptChoice SelectRandomPhaseSkillForAi(global::CardGame.Core.PendingDecision decision) => _resolve().SelectRandomPhaseSkillForAi(decision);
    internal bool TryExecuteSharedProgramActivation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().TryExecuteSharedProgramActivation(actor, action, cards, targets);
    internal global::CardGame.Core.SkillGrantPhaseExpiry CurrentSharedSkillPhase { get => _resolve().CurrentSharedSkillPhase; }
}
