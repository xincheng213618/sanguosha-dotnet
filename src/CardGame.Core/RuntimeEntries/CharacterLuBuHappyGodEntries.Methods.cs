#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuBuHappyGodEntries
{
    internal bool AdvanceHappyLuBuObserver() => _resolve().AdvanceHappyLuBuObserver();
    internal void AnswerHappyLuBuTarget(global::CardGame.Core.PromptChoice chosen) => _resolve().AnswerHappyLuBuTarget(chosen);
    internal void ApplyHappyLuBuUse(global::CardGame.Core.ProgramSkillFrame f, int? added) => _resolve().ApplyHappyLuBuUse(f, added);
    internal void AssertHappyLuBu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertHappyLuBu(f);
    internal void AssertHappyLuBuUses() => _resolve().AssertHappyLuBuUses();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHappyLuBu(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginHappyLuBu(supplied, op);
    internal bool CompleteHappyLuBuActualStart(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame p) => _resolve().CompleteHappyLuBuActualStart(p);
    internal bool ContinueHappyLuBu(long id) => _resolve().ContinueHappyLuBu(id);
    internal global::CardGame.Core.Card FindCardById(int id) => _resolve().FindCardById(id);
    internal global::CardGame.Core.PromptChoice HappyLuBuAi(global::CardGame.Core.PendingDecision p) => _resolve().HappyLuBuAi(p);
    internal bool HappyLuBuBeforeDamage(long id) => _resolve().HappyLuBuBeforeDamage(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappyLuBuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyLuBuChoices(f);
    internal bool HappyLuBuDamage(long id) => _resolve().HappyLuBuDamage(id);
    internal bool HappyLuBuDying() => _resolve().HappyLuBuDying();
    internal bool HappyLuBuEligible(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().HappyLuBuEligible(c, t, w);
    internal bool HappyLuBuExtraTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => _resolve().HappyLuBuExtraTarget(use, target);
    internal bool HappyLuBuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HappyLuBuFirstChild(f, child);
    internal bool HappyLuBuFirstUse(global::CardGame.Core.CardUseFrame use) => _resolve().HappyLuBuFirstUse(use);
    internal bool HappyLuBuMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().HappyLuBuMovement(f, effect, pending);
    internal global::CardGame.Core.ProgramSkillFrame? HappyLuBuObserverRoot() => _resolve().HappyLuBuObserverRoot();
    internal int HappyLuBuRequiredResponseCount(int skillOwner, int source, int responder, global::CardGame.Core.CardKind incoming, global::CardGame.Core.CardKind required) => _resolve().HappyLuBuRequiredResponseCount(skillOwner, source, responder, incoming, required);
    internal global::CardGame.Core.SkillProgramActivation HappyLuBuShenfenActivation() => _resolve().HappyLuBuShenfenActivation();
    internal bool HasHappyLuBuWuqianCapability(global::CardGame.Core.CharacterState source) => _resolve().HasHappyLuBuWuqianCapability(source);
    internal bool IsHappyLuBuActualStart(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame p) => _resolve().IsHappyLuBuActualStart(p);
    internal bool IsHappyLuBuTrueVirtualProducer(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.SkillProgramEffect paused) => _resolve().IsHappyLuBuTrueVirtualProducer(use, parent, paused);
    internal bool RetainHappyLuBuPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainHappyLuBuPaid(f);
    internal bool ReturnHappyLuBuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHappyLuBuMovement(f);
    internal bool TryBeginHappyLuBuActualStart(global::CardGame.Core.CharacterState owner) => _resolve().TryBeginHappyLuBuActualStart(owner);
    internal bool TryHappyLuBuUse(int owner, global::CardGame.Core.ProgramSkillWindowContext w, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryHappyLuBuUse(owner, w, out use, out window);
}
