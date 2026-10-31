#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhenJiHappyGodEntries
{
    internal bool AdvanceHappyZjChildren() => _resolve().AdvanceHappyZjChildren();
    internal void AnswerHappyZj(global::CardGame.Core.PromptChoice chosen) => _resolve().AnswerHappyZj(chosen);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHappyZj(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginHappyZj(supplied, op);
    internal void FinishHappyZj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHappyZj(f);
    internal global::CardGame.Core.CardActionCost? HappyFeifuCost(global::CardGame.Core.ProgramCardTriggerWindowFrame w, int owner, string instance) => _resolve().HappyFeifuCost(w, owner, instance);
    internal bool HappyShenfuTarget(int owner, int target) => _resolve().HappyShenfuTarget(owner, target);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.HappyZjStartedEvent> HappyShenfuUses(int owner) => _resolve().HappyShenfuUses(owner);
    internal global::CardGame.Core.PromptChoice HappyZjAiFeifu(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyZjAiFeifu(p, f);
    internal global::CardGame.Core.PromptChoice HappyZjAiReturn(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyZjAiReturn(p, f);
    internal global::CardGame.Core.PromptChoice HappyZjAiShenfu(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyZjAiShenfu(p, f);
    internal global::CardGame.Core.PromptChoice HappyZjAiStore(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyZjAiStore(p, f);
    internal bool HappyZjBeforeDamageObserver(long id) => _resolve().HappyZjBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappyZjChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyZjChoices(f);
    internal bool HappyZjDamageObserver(long id) => _resolve().HappyZjDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappyZjDiscards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyZjDiscards(f);
    internal bool HappyZjDying() => _resolve().HappyZjDying();
    internal bool HappyZjEligible(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().HappyZjEligible(c, t, w);
    internal bool HappyZjFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HappyZjFirstChild(f, child);
    internal bool HappyZjMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().HappyZjMovement(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? HappyZjObserverRoot() => _resolve().HappyZjObserverRoot();
    internal global::CardGame.Core.HappyZjEntity[] HappyZjPileEntities(int owner) => _resolve().HappyZjPileEntities(owner);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PublicPersistentPileSource> HappyZjPiles(int owner) => _resolve().HappyZjPiles(owner);
    internal bool IsHappyFeifuClaimedResponseEntity(global::CardGame.Core.CardActionContext action, int cardId) => _resolve().IsHappyFeifuClaimedResponseEntity(action, cardId);
    internal void MoveHappyZjEntities(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardLocation to) => _resolve().MoveHappyZjEntities(f, to);
    internal void PublishHappyZj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHappyZj(f);
    internal void RequireHappyZj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireHappyZj(f);
    internal bool ResumeHappyZj(long id) => _resolve().ResumeHappyZj(id);
    internal bool RetainHappyZjPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainHappyZjPaid(f);
    internal void ReturnHappyZjDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHappyZjDraw(f, outcome);
    internal bool ReturnHappyZjMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHappyZjMovement(f);
    internal void SaveHappyZjPayment(long id, long before) => _resolve().SaveHappyZjPayment(id, before);
    internal bool HasHappyZhenJi { get => _resolve().HasHappyZhenJi; }
}
