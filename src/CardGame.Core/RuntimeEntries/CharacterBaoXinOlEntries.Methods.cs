#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBaoXinOlEntries
{
    internal bool AdvanceBaoXinChildren() => _resolve().AdvanceBaoXinChildren();
    internal void AnswerBaoXin(PromptChoice chosen) => _resolve().AnswerBaoXin(chosen);
    internal PromptChoice BaoXinChoice(ProgramSkillFrame f, string branch, string label, int? card = default, int? target = default) => _resolve().BaoXinChoice(f, branch, label, card, target);
    internal IReadOnlyList<PromptChoice> BaoXinChoices(ProgramSkillFrame f) => _resolve().BaoXinChoices(f);
    internal bool BaoXinFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().BaoXinFirstChild(f, child);
    internal bool BaoXinMovementMatches(ProgramSkillFrame f, CardMovementRecord m) => _resolve().BaoXinMovementMatches(f, m);
    internal ProgramSkillFrame? BaoXinObserverRoot() => _resolve().BaoXinObserverRoot();
    internal BaoXinYimouQualification? BaoXinQualification(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().BaoXinQualification(candidate, context);
    internal SkillProgramStepOutcome BeginBaoXin(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginBaoXin(supplied, op);
    internal bool CanOfferBaoXin(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferBaoXin(c, trigger, context);
    internal DamageTriggerWindowFrame CaptureBaoXinYimouWindow(DamageTriggerWindowFrame frame, GameEngine.IDamageAttempt attack) => _resolve().CaptureBaoXinYimouWindow(frame, attack);
    internal PromptChoice ChooseBaoXinAi(PendingDecision prompt, ProgramSkillFrame f) => _resolve().ChooseBaoXinAi(prompt, f);
    internal void CompleteBaoXinMove(long id, int count) => _resolve().CompleteBaoXinMove(id, count);
    internal void ContinueBaoXin(long id) => _resolve().ContinueBaoXin(id);
    internal bool ContinueIssuedBaoXin(ProgramSkillFrame f) => _resolve().ContinueIssuedBaoXin(f);
    internal ProgramSkillWindowContext CreateBaoXinYimouContext(DamageTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext ordinary) => _resolve().CreateBaoXinYimouContext(frame, candidate, ordinary);
    internal bool DrainBaoXin(ProgramSkillFrame f) => _resolve().DrainBaoXin(f);
    internal void FinishBaoXin(ProgramSkillFrame f) => _resolve().FinishBaoXin(f);
    internal bool IsBaoXinMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().IsBaoXinMovement(f, effect, pending);
    internal bool IsIssuedBaoXinMutaoPile(PublicPersistentPileSource source) => _resolve().IsIssuedBaoXinMutaoPile(source);
    internal void MoveBaoXin(ProgramSkillFrame f, IReadOnlyList<BaoXinEntity> entities, BaoXinStage stage, CardLocation destination, string reason) => _resolve().MoveBaoXin(f, entities, stage, destination, reason);
    internal void PrepareBaoXinMove(ref ProgramSkillFrame f, IReadOnlyList<BaoXinEntity> entities, BaoXinStage stage) => _resolve().PrepareBaoXinMove(ref f, entities, stage);
    internal void PublishBaoXin(ProgramSkillFrame f) => _resolve().PublishBaoXin(f);
    internal void RecordBaoXinRandom(ProgramSkillFrame f, int ordinal, int recipient) => _resolve().RecordBaoXinRandom(f, ordinal, recipient);
    internal bool ResumeBaoXin(long id) => _resolve().ResumeBaoXin(id);
    internal bool ReturnBaoXinMovement(ProgramSkillFrame f) => _resolve().ReturnBaoXinMovement(f);
    internal bool ValidBaoXin(ProgramSkillFrame f) => _resolve().ValidBaoXin(f);
    internal bool HasBaoXinCapability { get => _resolve().HasBaoXinCapability; }
}
