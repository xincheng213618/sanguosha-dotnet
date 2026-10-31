#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoHuaiMouMobileEntries
{
    internal bool AdvanceMouGhChildren() => _resolve().AdvanceMouGhChildren();
    internal void AnswerMouGh(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMouGh(choice);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PlayerMarkerSnapshot>? AppendMouGuoHuaiJingce(int seat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PlayerMarkerSnapshot>? ordinary) => _resolve().AppendMouGuoHuaiJingce(seat, ordinary);
    internal void AssertMouGh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMouGh(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouGh(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMouGh(supplied, op);
    internal bool CanOfferMouGh(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferMouGh(c, t, context);
    internal void CaptureMouGuoHuaiGainBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureMouGuoHuaiGainBatch(batch);
    internal global::CardGame.Core.PromptChoice ChooseMouGhAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseMouGhAi(p, f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectMouGuoHuaiGainCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectMouGuoHuaiGainCandidates(batch);
    internal void CollectWindow_CollectMouGuoHuaiGainCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectMouGuoHuaiGainCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureMouGuoHuaiGainBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouGuoHuaiGainBatch(batch, completed);
    internal void ContinueMouGh(long id) => _resolve().ContinueMouGh(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateMouGuoHuaiGainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame parent, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateMouGuoHuaiGainContext(parent, c);
    internal bool DrainMouGh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouGh(f);
    internal bool ExcludeOrdinaryMovement_IsMouGuoHuaiGainTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsMouGuoHuaiGainTrigger(candidate, trigger);
    internal void FinishMouGh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouGh(f);
    internal void FreezeMouGhActualStart(global::CardGame.Core.TurnStartedEvent turn) => _resolve().FreezeMouGhActualStart(turn);
    internal bool HasMouGhBeforeDamage(long id) => _resolve().HasMouGhBeforeDamage(id);
    internal bool HasMouGhDamage(long id) => _resolve().HasMouGhDamage(id);
    internal bool IsMouGhDying() => _resolve().IsMouGhDying();
    internal bool IsMouGhMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsMouGhMovement(f, e, p);
    internal bool KeepMouGhPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepMouGhPaid(f);
    internal bool LiveMouGhSource(global::CardGame.Core.CardConversionSource source, string hash) => _resolve().LiveMouGhSource(source, hash);
    internal global::CardGame.Core.PromptChoice MouGhChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string description, int? card = default, int? target = default) => _resolve().MouGhChoice(f, branch, description, card, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouGhChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouGhChoices(f);
    internal bool MouGhFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouGhFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? MouGhObserverRoot() => _resolve().MouGhObserverRoot();
    internal bool MouGhRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouGhRecoveryParent(f, recovery);
    internal bool MouGhSuffix(int index) => _resolve().MouGhSuffix(index);
    internal global::CardGame.Core.MouGuoHuaiGainQualifiedEvent? MouGuoHuaiGainQualification(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().MouGuoHuaiGainQualification(c, context);
    internal void PlaceMouGhPredictions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PlaceMouGhPredictions(f);
    internal bool? ProveMouGuoHuaiMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveMouGuoHuaiMovement(trigger, candidate, context);
    internal void PublishMouGh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouGh(f);
    internal bool ResumeMouGh(long id) => _resolve().ResumeMouGh(id);
    internal void ReturnMouGhDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnMouGhDraw(f, o);
    internal bool ReturnMouGhMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouGhMovement(f);
    internal void SealMouGh(long id) => _resolve().SealMouGh(id);
    internal void StoreMouGhTop(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StoreMouGhTop(f);
    internal bool TryBeginMouGuoHuaiActualStart(global::CardGame.Core.CharacterState owner) => _resolve().TryBeginMouGuoHuaiActualStart(owner);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateMouGuoHuaiGainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateMouGuoHuaiGainContext(frame, candidate);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MouGuoHuaiPredictionIssuedEvent> UnverifiedMouGhPredictions() => _resolve().UnverifiedMouGhPredictions();
    internal bool ValidMouGh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouGh(f);
    internal bool HasMouGuoHuaiMobile { get => _resolve().HasMouGuoHuaiMobile; }
}
