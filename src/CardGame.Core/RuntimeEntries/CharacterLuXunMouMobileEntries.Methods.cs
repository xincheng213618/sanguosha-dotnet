#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuXunMouMobileEntries
{
    internal void AnswerMouLuXun(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMouLuXun(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouLuXun(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginMouLuXun(input, operation);
    internal void BeginMouLuXunView(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginMouLuXunView(f);
    internal bool CanMouLuXunRecordNativeTarget(int target, global::CardGame.Core.CardActionContext action) => _resolve().CanMouLuXunRecordNativeTarget(target, action);
    internal bool CanOfferMouLuXun(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferMouLuXun(candidate, trigger, context);
    internal global::CardGame.Core.PromptChoice ChooseMouLuXunAi(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseMouLuXunAi(prompt, f);
    internal bool ContinueIssuedMouLuXun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedMouLuXun(f);
    internal bool ContinueMouLuXun(long id) => _resolve().ContinueMouLuXun(id);
    internal bool DrainMouLuXun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouLuXun(f);
    internal void FinishMouLuXun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouLuXun(f);
    internal bool HasPerTargetTrickEffectBinding(global::CardGame.Core.ProgramCardTriggerCandidate c) => _resolve().HasPerTargetTrickEffectBinding(c);
    internal bool HideMouLuXunPrivateEntity(int id) => _resolve().HideMouLuXunPrivateEntity(id);
    internal bool IsMouLuXunMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsMouLuXunMovement(f, effect, pending);
    internal bool IsMouLuXunPrivateEntity(int id) => _resolve().IsMouLuXunPrivateEntity(id);
    internal bool IsMouLuXunVirtualParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsMouLuXunVirtualParent(f);
    internal void IssueMouLuXunUse(global::CardGame.Core.ProgramSkillFrame f, int index) => _resolve().IssueMouLuXunUse(f, index);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouLuXunChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouLuXunChoices(f);
    internal int? MouLuXunEffectUser(global::CardGame.Core.CardEffectBeforeApplyFrame window) => _resolve().MouLuXunEffectUser(window);
    internal int MouLuXunLostCount(int owner, int turn) => _resolve().MouLuXunLostCount(owner, turn);
    internal bool MouLuXunParentMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouLuXunParentMatches(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> MouLuXunPrivateCards(int viewer) => _resolve().MouLuXunPrivateCards(viewer);
    internal global::CardGame.Core.CardKind[] MouLuXunRecordedNames(int owner) => _resolve().MouLuXunRecordedNames(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> MouLuXunUseOptions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouLuXunUseOptions(f);
    internal global::CardGame.Core.SkillProgramEffect MouLuXunVirtualInstruction(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouLuXunVirtualInstruction(f);
    internal void PayMouLuXunGift(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().PayMouLuXunGift(f, target);
    internal void PayMouLuXunHold(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayMouLuXunHold(f);
    internal void PublishMouLuXun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouLuXun(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> RemoveMouLuXunPrivateProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().RemoveMouLuXunPrivateProcessing(cards);
    internal bool ResolveMouLuXunMultiIronChain(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveMouLuXunMultiIronChain(pending);
    internal bool ResumeMouLuXun(long id) => _resolve().ResumeMouLuXun(id);
    internal bool ResumeMouLuXunMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ResumeMouLuXunMovement(frame);
    internal bool ResumeMouLuXunNativeAttack() => _resolve().ResumeMouLuXunNativeAttack();
    internal bool ReturnMouLuXunNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMouLuXunNativeUse(use);
    internal void ReturnMouLuXunUnassigned(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouLuXunUnassigned(f);
    internal void ReturnMouLuXunWenheTarget(global::CardGame.Core.NativeTrickTargetEffectReturn receipt) => _resolve().ReturnMouLuXunWenheTarget(receipt);
    internal void ReturnNativeTrickTargetEffect(global::CardGame.Core.CardEffectBeforeApplyFrame window) => _resolve().ReturnNativeTrickTargetEffect(window);
    internal bool TryBeginMouLuXunGroupEffect(global::CardGame.Core.NativeOwners.GroupCardHandle group) => _resolve().TryBeginMouLuXunGroupEffect(group);
    internal bool TryBeginMouLuXunWenheTarget(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, int hpAmount) => _resolve().TryBeginMouLuXunWenheTarget(frame, actorSeat, hpAmount);
    internal bool ValidMouLuXun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouLuXun(f);
    internal bool ValidMouLuXunWenheTarget(global::CardGame.Core.NativeTrickTargetEffectReturn receipt, global::CardGame.Core.CardActionContext action) => _resolve().ValidMouLuXunWenheTarget(receipt, action);
    internal bool ValidNativeTrickTargetEffect(global::CardGame.Core.CardEffectBeforeApplyFrame window) => _resolve().ValidNativeTrickTargetEffect(window);
    internal bool HasMouLuXun { get => _resolve().HasMouLuXun; }
}
