#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSkillProgramsEntries
{
    internal SkillProgramStepOutcome BeginProgramSkillDamage(ProgramSkillFrame frame, int targetSeat, int amount, ProgramParticipantReference? sourceReference = default, DamageNature? nature = default, bool sourceLess = false, int? explicitSourceSeat = default) => _resolve().BeginProgramSkillDamage(frame, targetSeat, amount, sourceReference, nature, sourceLess, explicitSourceSeat);
    internal void BeginProgramSkillDying(long parentFrameId, CharacterState victim) => _resolve().BeginProgramSkillDying(parentFrameId, victim);
    internal SkillProgramStepOutcome BeginProgramSkillPindian(ProgramSkillFrame frame, int targetSeat) => _resolve().BeginProgramSkillPindian(frame, targetSeat);
    internal IEnumerable<LegalAction> BuildProgramActions(CharacterState owner) => _resolve().BuildProgramActions(owner);
    internal IEnumerable<LegalAction> BuildProgramActionsCore(CharacterState owner) => _resolve().BuildProgramActionsCore(owner);
    internal bool CanSelectProgramActivationCard(ProgramExecutionPlan plan, Card card, int ownerSeat, string skillId) => _resolve().CanSelectProgramActivationCard(plan, card, ownerSeat, skillId);
    internal void CompleteProgramSkillAfterDying(GameEngine.DyingCompletionReceipt dying) => _resolve().CompleteProgramSkillAfterDying(dying);
    internal void ContinueAfterSelectedNativeUse(NativeUseReturnContext context) => _resolve().ContinueAfterSelectedNativeUse(context);
    internal void ContinueProgramAfterBoundEquipmentUse(CardUseFrame completed) => _resolve().ContinueProgramAfterBoundEquipmentUse(completed);
    internal void ContinueProgramAfterSelectedCardUse() => _resolve().ContinueProgramAfterSelectedCardUse();
    internal SkillProgramAiHint CreateProgramAiHint(SkillProgram program, SkillProgramActivation activation, PlayerSkillContext context, bool faceDown) => _resolve().CreateProgramAiHint(program, activation, context, faceDown);
    internal IReadOnlyList<SkillProgram> EnabledActivationPrograms(CharacterState player) => _resolve().EnabledActivationPrograms(player);
    internal IReadOnlyList<SkillProgram> EnabledCardIdentityPrograms(CharacterState player) => _resolve().EnabledCardIdentityPrograms(player);
    internal IReadOnlyList<SkillProgram> EnabledContributionPrograms(CharacterState player) => _resolve().EnabledContributionPrograms(player);
    internal IReadOnlyList<SkillProgram> EnabledPassiveRulePrograms(CharacterState player) => _resolve().EnabledPassiveRulePrograms(player);
    internal IReadOnlyList<SkillProgram> EnabledSkillPrograms(CharacterState player) => _resolve().EnabledSkillPrograms(player);
    internal IReadOnlyList<IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(CharacterState player, SkillProgramTriggerWindow window) => _resolve().EnabledUniqueProgramTriggers(player, window);
    internal IReadOnlyList<SkillProgram> EnabledViewAsPrograms(CharacterState player) => _resolve().EnabledViewAsPrograms(player);
    internal bool EvaluateProgramActivationCondition(SkillProgramCondition condition, PlayerSkillContext context, string skillId, string skillInstanceId) => _resolve().EvaluateProgramActivationCondition(condition, context, skillId, skillInstanceId);
    internal void ExecuteProgramContribution(CharacterState provider, CharacterState skillOwner, LegalAction action, int cardId) => _resolve().ExecuteProgramContribution(provider, skillOwner, action, cardId);
    internal void ExecuteProgramSkill(CharacterState owner, LegalAction action, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => _resolve().ExecuteProgramSkill(owner, action, cards, targets);
    internal void ExecuteValidatedProgramSkill(CharacterState owner, LegalAction action, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => _resolve().ExecuteValidatedProgramSkill(owner, action, cards, targets);
    internal void FinishProgramSkill(ProgramSkillFrame frame, bool completed) => _resolve().FinishProgramSkill(frame, completed);
    internal SkillProgram GetEnabledSkillProgram(CharacterState player, string skillId) => _resolve().GetEnabledSkillProgram(player, skillId);
    internal int GetProgramActivationMinimumCards(int ownerSeat, string skillId, SkillProgramActivation activation) => _resolve().GetProgramActivationMinimumCards(ownerSeat, skillId, activation);
    internal bool HasFreeEquipmentSlotForOwnedHandEquipment(CharacterState owner, CharacterState target) => _resolve().HasFreeEquipmentSlotForOwnedHandEquipment(owner, target);
    internal void PendProgramExtraTurn(ProgramSkillFrame frame, int? targetSeat) => _resolve().PendProgramExtraTurn(frame, targetSeat);
    internal Suit ProgramSameSuitInputSuit(CharacterState owner, int cardId, bool useEffectiveSuit) => _resolve().ProgramSameSuitInputSuit(owner, cardId, useEffectiveSuit);
    internal void ResetProgramContributionUsesForPlayPhase(int providerSeat) => _resolve().ResetProgramContributionUsesForPlayPhase(providerSeat);
    internal CommandResult SubmitUseProgramSkill(UseProgramSkillCommand command) => _resolve().SubmitUseProgramSkill(command);
    internal SkillProgramStepOutcome UseProgramBoundCardByTarget(ProgramSkillFrame frame, int userSeat, string sourceBind) => _resolve().UseProgramBoundCardByTarget(frame, userSeat, sourceBind);
    internal SkillProgramStepOutcome UseProgramSelectedCardsAs(ProgramSkillFrame frame, int targetSeat, string viewAsId, CardKind outputKind) => _resolve().UseProgramSelectedCardsAs(frame, targetSeat, viewAsId, outputKind);
    internal CommandError? ValidateProgramSelection(LegalAction action, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => _resolve().ValidateProgramSelection(action, cards, targets);
}
