#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhenJiHappyGodEntries
{
    internal bool AdvanceHappyZjChildren() => _resolve().AdvanceHappyZjChildren();
    internal void AnswerHappyZj(PromptChoice chosen) => _resolve().AnswerHappyZj(chosen);
    internal SkillProgramStepOutcome BeginHappyZj(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginHappyZj(supplied, op);
    internal void FinishHappyZj(ProgramSkillFrame f) => _resolve().FinishHappyZj(f);
    internal CardActionCost? HappyFeifuCost(ProgramCardTriggerWindowFrame w, int owner, string instance) => _resolve().HappyFeifuCost(w, owner, instance);
    internal bool HappyShenfuTarget(int owner, int target) => _resolve().HappyShenfuTarget(owner, target);
    internal IEnumerable<HappyZjStartedEvent> HappyShenfuUses(int owner) => _resolve().HappyShenfuUses(owner);
    internal PromptChoice HappyZjAiFeifu(PendingDecision p, ProgramSkillFrame f) => _resolve().HappyZjAiFeifu(p, f);
    internal PromptChoice HappyZjAiReturn(PendingDecision p, ProgramSkillFrame f) => _resolve().HappyZjAiReturn(p, f);
    internal PromptChoice HappyZjAiShenfu(PendingDecision p, ProgramSkillFrame f) => _resolve().HappyZjAiShenfu(p, f);
    internal PromptChoice HappyZjAiStore(PendingDecision p, ProgramSkillFrame f) => _resolve().HappyZjAiStore(p, f);
    internal IReadOnlyList<PromptChoice> HappyZjChoices(ProgramSkillFrame f) => _resolve().HappyZjChoices(f);
    internal IReadOnlyList<PromptChoice> HappyZjDiscards(ProgramSkillFrame f) => _resolve().HappyZjDiscards(f);
    internal bool HappyZjDying() => _resolve().HappyZjDying();
    internal bool HappyZjEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().HappyZjEligible(c, t, w);
    internal bool HappyZjFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HappyZjFirstChild(f, child);
    internal bool HappyZjMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().HappyZjMovement(f, e, p);
    internal ProgramSkillFrame? HappyZjObserverRoot() => _resolve().HappyZjObserverRoot();
    internal HappyZjEntity[] HappyZjPileEntities(int owner) => _resolve().HappyZjPileEntities(owner);
    internal IEnumerable<PublicPersistentPileSource> HappyZjPiles(int owner) => _resolve().HappyZjPiles(owner);
    internal void MoveHappyZjEntities(ProgramSkillFrame f, CardLocation to) => _resolve().MoveHappyZjEntities(f, to);
    internal void PublishHappyZj(ProgramSkillFrame f) => _resolve().PublishHappyZj(f);
    internal bool ResumeHappyZj(long id) => _resolve().ResumeHappyZj(id);
    internal bool RetainHappyZjPaid(ProgramSkillFrame f) => _resolve().RetainHappyZjPaid(f);
    internal void ReturnHappyZjDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnHappyZjDraw(f, outcome);
    internal bool ReturnHappyZjMovement(ProgramSkillFrame f) => _resolve().ReturnHappyZjMovement(f);
    internal void SaveHappyZjPayment(long id, long before) => _resolve().SaveHappyZjPayment(id, before);
    internal bool HasHappyZhenJi { get => _resolve().HasHappyZhenJi; }
}
