#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuXunMouMobileEntries
{
    internal void AnswerMouLuXun(PromptChoice choice) => _resolve().AnswerMouLuXun(choice);
    internal SkillProgramStepOutcome BeginMouLuXun(ProgramSkillFrame input, SkillProgramEffectOp operation) => _resolve().BeginMouLuXun(input, operation);
    internal void BeginMouLuXunView(ProgramSkillFrame f) => _resolve().BeginMouLuXunView(f);
    internal bool CanMouLuXunRecordNativeTarget(int target, CardActionContext action) => _resolve().CanMouLuXunRecordNativeTarget(target, action);
    internal bool CanOfferMouLuXun(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferMouLuXun(candidate, trigger, context);
    internal PromptChoice ChooseMouLuXunAi(PendingDecision prompt, ProgramSkillFrame f) => _resolve().ChooseMouLuXunAi(prompt, f);
    internal bool ContinueIssuedMouLuXun(ProgramSkillFrame f) => _resolve().ContinueIssuedMouLuXun(f);
    internal bool ContinueMouLuXun(long id) => _resolve().ContinueMouLuXun(id);
    internal bool DrainMouLuXun(ProgramSkillFrame f) => _resolve().DrainMouLuXun(f);
    internal void FinishMouLuXun(ProgramSkillFrame f) => _resolve().FinishMouLuXun(f);
    internal bool HasPerTargetTrickEffectBinding(ProgramCardTriggerCandidate c) => _resolve().HasPerTargetTrickEffectBinding(c);
    internal bool HideMouLuXunPrivateEntity(int id) => _resolve().HideMouLuXunPrivateEntity(id);
    internal bool IsMouLuXunPrivateEntity(int id) => _resolve().IsMouLuXunPrivateEntity(id);
    internal bool IsMouLuXunVirtualParent(ProgramSkillFrame f) => _resolve().IsMouLuXunVirtualParent(f);
    internal void IssueMouLuXunUse(ProgramSkillFrame f, int index) => _resolve().IssueMouLuXunUse(f, index);
    internal IReadOnlyList<PromptChoice> MouLuXunChoices(ProgramSkillFrame f) => _resolve().MouLuXunChoices(f);
    internal int? MouLuXunEffectUser(CardEffectBeforeApplyFrame window) => _resolve().MouLuXunEffectUser(window);
    internal int MouLuXunLostCount(int owner, int turn) => _resolve().MouLuXunLostCount(owner, turn);
    internal bool MouLuXunParentMatches(ProgramSkillFrame f) => _resolve().MouLuXunParentMatches(f);
    internal IEnumerable<CardSnapshot> MouLuXunPrivateCards(int viewer) => _resolve().MouLuXunPrivateCards(viewer);
    internal CardKind[] MouLuXunRecordedNames(int owner) => _resolve().MouLuXunRecordedNames(owner);
    internal IReadOnlyList<GameEngine.ProgramOrdinaryTrickUseOption> MouLuXunUseOptions(ProgramSkillFrame f) => _resolve().MouLuXunUseOptions(f);
    internal SkillProgramEffect MouLuXunVirtualInstruction(ProgramSkillFrame f) => _resolve().MouLuXunVirtualInstruction(f);
    internal void PayMouLuXunGift(ProgramSkillFrame f, int target) => _resolve().PayMouLuXunGift(f, target);
    internal void PayMouLuXunHold(ProgramSkillFrame f) => _resolve().PayMouLuXunHold(f);
    internal void PublishMouLuXun(ProgramSkillFrame f) => _resolve().PublishMouLuXun(f);
    internal IReadOnlyList<Card> RemoveMouLuXunPrivateProcessing(IReadOnlyList<Card> cards) => _resolve().RemoveMouLuXunPrivateProcessing(cards);
    internal bool ResolveMouLuXunMultiIronChain(NullificationWindowFrame pending) => _resolve().ResolveMouLuXunMultiIronChain(pending);
    internal bool ResumeMouLuXun(long id) => _resolve().ResumeMouLuXun(id);
    internal bool ResumeMouLuXunMovement(ProgramSkillFrame frame) => _resolve().ResumeMouLuXunMovement(frame);
    internal bool ResumeMouLuXunNativeAttack() => _resolve().ResumeMouLuXunNativeAttack();
    internal bool ReturnMouLuXunNativeUse(CardUseFrame use) => _resolve().ReturnMouLuXunNativeUse(use);
    internal void ReturnMouLuXunUnassigned(ProgramSkillFrame f) => _resolve().ReturnMouLuXunUnassigned(f);
    internal void ReturnMouLuXunWenheTarget(NativeTrickTargetEffectReturn receipt) => _resolve().ReturnMouLuXunWenheTarget(receipt);
    internal void ReturnNativeTrickTargetEffect(CardEffectBeforeApplyFrame window) => _resolve().ReturnNativeTrickTargetEffect(window);
    internal bool TryBeginMouLuXunGroupEffect(NativeOwners.GroupCardHandle group) => _resolve().TryBeginMouLuXunGroupEffect(group);
    internal bool TryBeginMouLuXunWenheTarget(ProgramSkillFrame frame, int actorSeat, int hpAmount) => _resolve().TryBeginMouLuXunWenheTarget(frame, actorSeat, hpAmount);
    internal bool ValidMouLuXun(ProgramSkillFrame f) => _resolve().ValidMouLuXun(f);
    internal bool ValidMouLuXunWenheTarget(NativeTrickTargetEffectReturn receipt, CardActionContext action) => _resolve().ValidMouLuXunWenheTarget(receipt, action);
    internal bool ValidNativeTrickTargetEffect(CardEffectBeforeApplyFrame window) => _resolve().ValidNativeTrickTargetEffect(window);
    internal bool HasMouLuXun { get => _resolve().HasMouLuXun; }
}
