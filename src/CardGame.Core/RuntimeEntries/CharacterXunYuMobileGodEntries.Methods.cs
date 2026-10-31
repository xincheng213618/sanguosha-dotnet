#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXunYuMobileGodEntries
{
    internal void AddQizhengNaturalActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playable, int? selectedPhysicalCardId = default) => _resolve().AddQizhengNaturalActions(actions, actor, playable, selectedPhysicalCardId);
    internal bool AdvanceQizhengAi() => _resolve().AdvanceQizhengAi();
    internal void AnswerDinghanEdit(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerDinghanEdit(selected);
    internal void AnswerQizheng(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerQizheng(selected);
    internal void AssertQizheng(global::CardGame.Core.CardUseFrame use) => _resolve().AssertQizheng(use);
    internal bool CanLingce(int seat, global::CardGame.Core.CardActionContext? a) => _resolve().CanLingce(seat, a);
    internal bool CanTriggerShenXunYu(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanTriggerShenXunYu(gate);
    internal global::CardGame.Core.PromptChoice ChooseDinghanEdit(global::CardGame.Core.PendingDecision prompt) => _resolve().ChooseDinghanEdit(prompt);
    internal void ContinueQizheng(long id) => _resolve().ContinueQizheng(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DinghanEditChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DinghanEditChoices(f);
    internal global::System.Collections.Generic.HashSet<global::CardGame.Core.CardKind> DinghanNames(int seat) => _resolve().DinghanNames(seat);
    internal bool DinghanRequiresFinalizedTrickWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().DinghanRequiresFinalizedTrickWindow(query);
    internal void FinishQizhengWithoutDamage(global::CardGame.Core.CardUseFrame supplied) => _resolve().FinishQizhengWithoutDamage(supplied);
    internal bool FinishVirtualQizhengAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishVirtualQizhengAttack(attack, out paused);
    internal bool HasDinghanCancellation(global::CardGame.Core.CardUseFrame use) => _resolve().HasDinghanCancellation(use);
    internal bool HasQizhengPrompt() => _resolve().HasQizhengPrompt();
    internal bool IsDinghanCancellationFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DinghanTargetCanceledEvent e, global::System.Collections.Generic.IReadOnlyList<int> before) => _resolve().IsDinghanCancellationFact(use, e, before);
    internal bool IsTianzuoMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsTianzuoMovement(f, effect, pending);
    internal void PayQizhengSingleResponse(long id, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => _resolve().PayQizhengSingleResponse(id, responder, card, kind);
    internal void PublishDinghanEdit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishDinghanEdit(f);
    internal void PublishQizheng(global::CardGame.Core.CardUseFrame use) => _resolve().PublishQizheng(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> QizhengChoices(global::CardGame.Core.CardUseFrame use) => _resolve().QizhengChoices(use);
    internal global::System.Collections.Generic.Dictionary<string, string> QizhengParameters(string branch, string value = "") => _resolve().QizhengParameters(branch, value);
    internal void ResolveQizhengAfterResponse(long id) => _resolve().ResolveQizhengAfterResponse(id);
    internal void ResolveQizhengXiangshengEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveQizhengXiangshengEffect(pending);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShenXunYu(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ResolveShenXunYu(supplied, op);
    internal bool ResumeDinghanEdit(long id) => _resolve().ResumeDinghanEdit(id);
    internal global::CardGame.Core.CardActionContext? ShenXunYuAction(global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().ShenXunYuAction(context);
    internal bool TryCompleteQizhengAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryCompleteQizhengAttack(attack);
    internal bool TryContinueQizhengResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().TryContinueQizhengResponse(attack, action, continuation);
    internal bool TryExecuteQizhengNaturalUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => _resolve().TryExecuteQizhengNaturalUse(actor, card, action);
    internal bool HasDinghanCapability { get => _resolve().HasDinghanCapability; }
}
