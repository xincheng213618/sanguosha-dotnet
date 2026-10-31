#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLastDamageSourceReciprocityEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLastDamageSourceReciprocity(global::CardGame.Core.ProgramSkillFrame supplied, string stateId) => _resolve().BeginLastDamageSourceReciprocity(supplied, stateId);
    internal bool CanContinueLastDamageSourceReciprocity(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueLastDamageSourceReciprocity(f);
    internal bool CanOfferLastDamageSourceEligibility(global::CardGame.Core.LastDamageSourceReciprocityEligibleEvent? e) => _resolve().CanOfferLastDamageSourceEligibility(e);
    internal bool CanOfferLastDamageSourceReciprocity(int ownerSeat, string skillId, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CanOfferLastDamageSourceReciprocity(ownerSeat, skillId, trigger, attack);
    internal bool CanOfferLastDamageSourceReciprocity2(int ownerSeat, string skillId, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferLastDamageSourceReciprocity(ownerSeat, skillId, trigger, context);
    internal bool LastDamageSourceAlreadyIssued(global::CardGame.Core.LastDamageSourceReciprocityEligibleEvent e) => _resolve().LastDamageSourceAlreadyIssued(e);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LastDamageSourceDiscardChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LastDamageSourceDiscardChoices(f);
    internal global::CardGame.Core.LastDamageSourceReciprocityEligibleEvent? LastDamageSourceEligibility(int owner, string skillId, global::CardGame.Core.SkillProgramTrigger trigger, long damageId) => _resolve().LastDamageSourceEligibility(owner, skillId, trigger, damageId);
    internal global::CardGame.Core.LastDamageSourceReciprocityMaterial[] LastDamageSourceLegalMaterials(int seat) => _resolve().LastDamageSourceLegalMaterials(seat);
    internal void ObserveLastDamageSourceReciprocity(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveLastDamageSourceReciprocity(payload);
    internal void PublishLastDamageSourceDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishLastDamageSourceDiscard(f);
    internal void QueueLastDamageSourceMovementIssued(global::CardGame.Core.ProgramSkillFrame f) => _resolve().QueueLastDamageSourceMovementIssued(f);
    internal int? RecordedLastDamageSource(int owner, string skillId, string stateId) => _resolve().RecordedLastDamageSource(owner, skillId, stateId);
    internal void ResolveLastDamageSourceReciprocityChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveLastDamageSourceReciprocityChoice(choice);
    internal bool ResumeLastDamageSourceReciprocity(long frameId) => _resolve().ResumeLastDamageSourceReciprocity(frameId);
    internal bool ReturnLastDamageSourceReciprocityMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnLastDamageSourceReciprocityMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiLastDamageSourceReciprocity(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiLastDamageSourceReciprocity(decision, f);
    internal long LastDamageSourceMovementSequence { get => _resolve().LastDamageSourceMovementSequence; }
}
