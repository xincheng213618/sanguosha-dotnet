#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLastDamageSourceReciprocityEntries
{
    internal SkillProgramStepOutcome BeginLastDamageSourceReciprocity(ProgramSkillFrame supplied, string stateId) => _resolve().BeginLastDamageSourceReciprocity(supplied, stateId);
    internal bool CanContinueLastDamageSourceReciprocity(ProgramSkillFrame f) => _resolve().CanContinueLastDamageSourceReciprocity(f);
    internal bool CanOfferLastDamageSourceEligibility(LastDamageSourceReciprocityEligibleEvent? e) => _resolve().CanOfferLastDamageSourceEligibility(e);
    internal bool CanOfferLastDamageSourceReciprocity(int ownerSeat, string skillId, SkillProgramTrigger trigger, GameEngine.IDamageAttempt attack) => _resolve().CanOfferLastDamageSourceReciprocity(ownerSeat, skillId, trigger, attack);
    internal bool CanOfferLastDamageSourceReciprocity2(int ownerSeat, string skillId, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferLastDamageSourceReciprocity(ownerSeat, skillId, trigger, context);
    internal bool LastDamageSourceAlreadyIssued(LastDamageSourceReciprocityEligibleEvent e) => _resolve().LastDamageSourceAlreadyIssued(e);
    internal IReadOnlyList<PromptChoice> LastDamageSourceDiscardChoices(ProgramSkillFrame f) => _resolve().LastDamageSourceDiscardChoices(f);
    internal LastDamageSourceReciprocityEligibleEvent? LastDamageSourceEligibility(int owner, string skillId, SkillProgramTrigger trigger, long damageId) => _resolve().LastDamageSourceEligibility(owner, skillId, trigger, damageId);
    internal LastDamageSourceReciprocityMaterial[] LastDamageSourceLegalMaterials(int seat) => _resolve().LastDamageSourceLegalMaterials(seat);
    internal void ObserveLastDamageSourceReciprocity(IGameEvent payload) => _resolve().ObserveLastDamageSourceReciprocity(payload);
    internal void PublishLastDamageSourceDiscard(ProgramSkillFrame f) => _resolve().PublishLastDamageSourceDiscard(f);
    internal void QueueLastDamageSourceMovementIssued(ProgramSkillFrame f) => _resolve().QueueLastDamageSourceMovementIssued(f);
    internal int? RecordedLastDamageSource(int owner, string skillId, string stateId) => _resolve().RecordedLastDamageSource(owner, skillId, stateId);
    internal void ResolveLastDamageSourceReciprocityChoice(PromptChoice choice) => _resolve().ResolveLastDamageSourceReciprocityChoice(choice);
    internal bool ResumeLastDamageSourceReciprocity(long frameId) => _resolve().ResumeLastDamageSourceReciprocity(frameId);
    internal bool ReturnLastDamageSourceReciprocityMovement(ProgramSkillFrame f) => _resolve().ReturnLastDamageSourceReciprocityMovement(f);
    internal PromptChoice SelectAiLastDamageSourceReciprocity(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiLastDamageSourceReciprocity(decision, f);
    internal long LastDamageSourceMovementSequence { get => _resolve().LastDamageSourceMovementSequence; }
}
