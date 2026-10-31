#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuBuHappyGodEntries
{
    internal bool AdvanceHappyLuBuObserver() => _resolve().AdvanceHappyLuBuObserver();
    internal void AnswerHappyLuBuTarget(PromptChoice chosen) => _resolve().AnswerHappyLuBuTarget(chosen);
    internal void ApplyHappyLuBuUse(ProgramSkillFrame f, int? added) => _resolve().ApplyHappyLuBuUse(f, added);
    internal void AssertHappyLuBuUses() => _resolve().AssertHappyLuBuUses();
    internal SkillProgramStepOutcome BeginHappyLuBu(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginHappyLuBu(supplied, op);
    internal bool CompleteHappyLuBuActualStart(ProgramLifecycleTriggerWindowFrame p) => _resolve().CompleteHappyLuBuActualStart(p);
    internal bool ContinueHappyLuBu(long id) => _resolve().ContinueHappyLuBu(id);
    internal PromptChoice HappyLuBuAi(PendingDecision p) => _resolve().HappyLuBuAi(p);
    internal IReadOnlyList<PromptChoice> HappyLuBuChoices(ProgramSkillFrame f) => _resolve().HappyLuBuChoices(f);
    internal bool HappyLuBuDying() => _resolve().HappyLuBuDying();
    internal bool HappyLuBuEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().HappyLuBuEligible(c, t, w);
    internal bool HappyLuBuExtraTarget(CardUseFrame use, CharacterState target) => _resolve().HappyLuBuExtraTarget(use, target);
    internal bool HappyLuBuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HappyLuBuFirstChild(f, child);
    internal bool HappyLuBuFirstUse(CardUseFrame use) => _resolve().HappyLuBuFirstUse(use);
    internal bool HappyLuBuMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().HappyLuBuMovement(f, effect, pending);
    internal ProgramSkillFrame? HappyLuBuObserverRoot() => _resolve().HappyLuBuObserverRoot();
    internal int HappyLuBuRequiredResponseCount(int skillOwner, int source, int responder, CardKind incoming, CardKind required) => _resolve().HappyLuBuRequiredResponseCount(skillOwner, source, responder, incoming, required);
    internal SkillProgramActivation HappyLuBuShenfenActivation() => _resolve().HappyLuBuShenfenActivation();
    internal bool HasHappyLuBuWuqianCapability(CharacterState source) => _resolve().HasHappyLuBuWuqianCapability(source);
    internal bool IsHappyLuBuActualStart(ProgramLifecycleTriggerWindowFrame p) => _resolve().IsHappyLuBuActualStart(p);
    internal bool IsHappyLuBuTrueVirtualProducer(CardUseFrame use, ProgramSkillFrame parent, SkillProgramEffect paused) => _resolve().IsHappyLuBuTrueVirtualProducer(use, parent, paused);
    internal bool RetainHappyLuBuPaid(ProgramSkillFrame f) => _resolve().RetainHappyLuBuPaid(f);
    internal bool ReturnHappyLuBuMovement(ProgramSkillFrame f) => _resolve().ReturnHappyLuBuMovement(f);
    internal bool TryBeginHappyLuBuActualStart(CharacterState owner) => _resolve().TryBeginHappyLuBuActualStart(owner);
    internal bool TryHappyLuBuUse(int owner, ProgramSkillWindowContext w, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryHappyLuBuUse(owner, w, out use, out window);
}
