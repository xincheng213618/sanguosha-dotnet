#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalHandEntitiesEntries
{
    internal void AddGetOriginalHandAttackRangeBonusAttackRangeContribution(global::CardGame.Core.CharacterState player, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryContribution> contributions) => _resolve().AddGetOriginalHandAttackRangeBonusAttackRangeContribution(player, contributions);
    internal bool AllowsRegisteredCanOfferOriginalHandEntityProgram(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsRegisteredCanOfferOriginalHandEntityProgram(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOriginalHandBenefit(global::CardGame.Core.ProgramSkillFrame supplied, string stateId) => _resolve().BeginOriginalHandBenefit(supplied, stateId);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOriginalHandInheritance(global::CardGame.Core.ProgramSkillFrame supplied, string stateId) => _resolve().BeginOriginalHandInheritance(supplied, stateId);
    internal bool CanContinueOriginalHandEntityProgram(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueOriginalHandEntityProgram(f);
    internal bool CanOfferOriginalHandEntityProgram(int ownerSeat, string skillId, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, string? skillInstanceId = default) => _resolve().CanOfferOriginalHandEntityProgram(ownerSeat, skillId, trigger, context, skillInstanceId);
    internal void CompleteOriginalHandBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteOriginalHandBenefit(f);
    internal int GetOriginalHandAttackRangeBonus(int seat) => _resolve().GetOriginalHandAttackRangeBonus(seat);
    internal int GetOriginalHandHandLimitBonus(int seat) => _resolve().GetOriginalHandHandLimitBonus(seat);
    internal global::CardGame.Core.SkillProgramStepOutcome InitializeOriginalHandEntities(global::CardGame.Core.ProgramSkillFrame supplied, string stateId) => _resolve().InitializeOriginalHandEntities(supplied, stateId);
    internal void ObserveOriginalHandMovement(global::CardGame.Core.CardMovementRecord movement) => _resolve().ObserveOriginalHandMovement(movement);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> OriginalHandEntityChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OriginalHandEntityChoices(f);
    internal global::CardGame.Core.OriginalHandPermanentBonus[] OriginalHandInheritableBonuses(int owner, string skill, string stateId) => _resolve().OriginalHandInheritableBonuses(owner, skill, stateId);
    internal global::CardGame.Core.OriginalHandLossEligibleEvent? OriginalHandLossEligibility(int owner, string skill, string state, string binding, long sequence) => _resolve().OriginalHandLossEligibility(owner, skill, state, binding, sequence);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.OriginalHandEntitySnapshot>? ProjectOriginalHandEntities(int ownerSeat, int viewerSeat, bool revealAll) => _resolve().ProjectOriginalHandEntities(ownerSeat, viewerSeat, revealAll);
    internal void PublishOriginalHandEntityChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishOriginalHandEntityChoice(f);
    internal void ResolveOriginalHandEntityChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveOriginalHandEntityChoice(choice);
    internal bool ResumeOriginalHandEntityProgram(long id) => _resolve().ResumeOriginalHandEntityProgram(id);
    internal bool ReturnOriginalHandEntityMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnOriginalHandEntityMovement(f);
    internal long OriginalHandMovementSequence { get => _resolve().OriginalHandMovementSequence; }
    internal bool TracksOriginalHandEntities { get => _resolve().TracksOriginalHandEntities; }
}
