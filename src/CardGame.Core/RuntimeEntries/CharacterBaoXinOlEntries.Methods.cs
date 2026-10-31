#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBaoXinOlEntries
{
    internal bool AdvanceBaoXinChildren() => _resolve().AdvanceBaoXinChildren();
    internal void AnswerBaoXin(global::CardGame.Core.PromptChoice chosen) => _resolve().AnswerBaoXin(chosen);
    internal global::CardGame.Core.PromptChoice BaoXinChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, int? card = default, int? target = default) => _resolve().BaoXinChoice(f, branch, label, card, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BaoXinChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BaoXinChoices(f);
    internal bool BaoXinFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().BaoXinFirstChild(f, child);
    internal bool BaoXinMovementMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().BaoXinMovementMatches(f, m);
    internal global::CardGame.Core.ProgramSkillFrame? BaoXinObserverRoot() => _resolve().BaoXinObserverRoot();
    internal global::CardGame.Core.BaoXinYimouQualification? BaoXinQualification(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().BaoXinQualification(candidate, context);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBaoXin(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginBaoXin(supplied, op);
    internal bool CanOfferBaoXin(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferBaoXin(c, trigger, context);
    internal global::CardGame.Core.DamageTriggerWindowFrame CaptureBaoXinYimouWindow(global::CardGame.Core.DamageTriggerWindowFrame frame, global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CaptureBaoXinYimouWindow(frame, attack);
    internal global::CardGame.Core.PromptChoice ChooseBaoXinAi(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseBaoXinAi(prompt, f);
    internal void CompleteBaoXinMove(long id, int count) => _resolve().CompleteBaoXinMove(id, count);
    internal void ContinueBaoXin(long id) => _resolve().ContinueBaoXin(id);
    internal bool ContinueIssuedBaoXin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedBaoXin(f);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateBaoXinYimouContext(global::CardGame.Core.DamageTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext ordinary) => _resolve().CreateBaoXinYimouContext(frame, candidate, ordinary);
    internal bool DrainBaoXin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainBaoXin(f);
    internal void FinishBaoXin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishBaoXin(f);
    internal bool HasBaoXinBeforeDamageObserver(long id) => _resolve().HasBaoXinBeforeDamageObserver(id);
    internal bool HasBaoXinDamageObserver(long id) => _resolve().HasBaoXinDamageObserver(id);
    internal bool IsBaoXinMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsBaoXinMovement(f, effect, pending);
    internal bool IsIssuedBaoXinMutaoPile(global::CardGame.Core.PublicPersistentPileSource source) => _resolve().IsIssuedBaoXinMutaoPile(source);
    internal void MoveBaoXin(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.BaoXinEntity> entities, global::CardGame.Core.BaoXinStage stage, global::CardGame.Core.CardLocation destination, string reason) => _resolve().MoveBaoXin(f, entities, stage, destination, reason);
    internal void PrepareBaoXinMove(ref global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.BaoXinEntity> entities, global::CardGame.Core.BaoXinStage stage) => _resolve().PrepareBaoXinMove(ref f, entities, stage);
    internal void PublishBaoXin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBaoXin(f);
    internal void RecordBaoXinRandom(global::CardGame.Core.ProgramSkillFrame f, int ordinal, int recipient) => _resolve().RecordBaoXinRandom(f, ordinal, recipient);
    internal bool ResumeBaoXin(long id) => _resolve().ResumeBaoXin(id);
    internal bool ReturnBaoXinMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBaoXinMovement(f);
    internal bool ValidBaoXin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidBaoXin(f);
    internal bool HasBaoXinCapability { get => _resolve().HasBaoXinCapability; }
}
