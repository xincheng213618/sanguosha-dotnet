#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalHandEntitiesEntries
{
    internal void AddGetOriginalHandAttackRangeBonusAttackRangeContribution(CharacterState player, List<RuleQueryContribution> contributions) => _resolve().AddGetOriginalHandAttackRangeBonusAttackRangeContribution(player, contributions);
    internal bool AllowsRegisteredCanOfferOriginalHandEntityProgram(ProgramTriggerLegalityContext gate) => _resolve().AllowsRegisteredCanOfferOriginalHandEntityProgram(gate);
    internal SkillProgramStepOutcome BeginOriginalHandBenefit(ProgramSkillFrame supplied, string stateId) => _resolve().BeginOriginalHandBenefit(supplied, stateId);
    internal SkillProgramStepOutcome BeginOriginalHandInheritance(ProgramSkillFrame supplied, string stateId) => _resolve().BeginOriginalHandInheritance(supplied, stateId);
    internal bool CanContinueOriginalHandEntityProgram(ProgramSkillFrame f) => _resolve().CanContinueOriginalHandEntityProgram(f);
    internal bool CanOfferOriginalHandEntityProgram(int ownerSeat, string skillId, SkillProgramTrigger trigger, ProgramSkillWindowContext context, string? skillInstanceId = default) => _resolve().CanOfferOriginalHandEntityProgram(ownerSeat, skillId, trigger, context, skillInstanceId);
    internal void CompleteOriginalHandBenefit(ProgramSkillFrame f) => _resolve().CompleteOriginalHandBenefit(f);
    internal int GetOriginalHandAttackRangeBonus(int seat) => _resolve().GetOriginalHandAttackRangeBonus(seat);
    internal int GetOriginalHandHandLimitBonus(int seat) => _resolve().GetOriginalHandHandLimitBonus(seat);
    internal SkillProgramStepOutcome InitializeOriginalHandEntities(ProgramSkillFrame supplied, string stateId) => _resolve().InitializeOriginalHandEntities(supplied, stateId);
    internal void ObserveOriginalHandMovement(CardMovementRecord movement) => _resolve().ObserveOriginalHandMovement(movement);
    internal IReadOnlyList<PromptChoice> OriginalHandEntityChoices(ProgramSkillFrame f) => _resolve().OriginalHandEntityChoices(f);
    internal OriginalHandPermanentBonus[] OriginalHandInheritableBonuses(int owner, string skill, string stateId) => _resolve().OriginalHandInheritableBonuses(owner, skill, stateId);
    internal OriginalHandLossEligibleEvent? OriginalHandLossEligibility(int owner, string skill, string state, string binding, long sequence) => _resolve().OriginalHandLossEligibility(owner, skill, state, binding, sequence);
    internal IReadOnlyList<OriginalHandEntitySnapshot>? ProjectOriginalHandEntities(int ownerSeat, int viewerSeat, bool revealAll) => _resolve().ProjectOriginalHandEntities(ownerSeat, viewerSeat, revealAll);
    internal void PublishOriginalHandEntityChoice(ProgramSkillFrame f) => _resolve().PublishOriginalHandEntityChoice(f);
    internal void ResolveOriginalHandEntityChoice(PromptChoice choice) => _resolve().ResolveOriginalHandEntityChoice(choice);
    internal bool ResumeOriginalHandEntityProgram(long id) => _resolve().ResumeOriginalHandEntityProgram(id);
    internal bool ReturnOriginalHandEntityMovement(ProgramSkillFrame f) => _resolve().ReturnOriginalHandEntityMovement(f);
    internal long OriginalHandMovementSequence { get => _resolve().OriginalHandMovementSequence; }
    internal bool TracksOriginalHandEntities { get => _resolve().TracksOriginalHandEntities; }
}
