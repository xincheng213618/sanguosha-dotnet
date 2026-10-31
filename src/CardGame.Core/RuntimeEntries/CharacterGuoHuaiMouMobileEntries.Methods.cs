#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoHuaiMouMobileEntries
{
    internal bool AdvanceMouGhChildren() => _resolve().AdvanceMouGhChildren();
    internal void AnswerMouGh(PromptChoice choice) => _resolve().AnswerMouGh(choice);
    internal IReadOnlyList<PlayerMarkerSnapshot>? AppendMouGuoHuaiJingce(int seat, IReadOnlyList<PlayerMarkerSnapshot>? ordinary) => _resolve().AppendMouGuoHuaiJingce(seat, ordinary);
    internal SkillProgramStepOutcome BeginMouGh(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMouGh(supplied, op);
    internal bool CanOfferMouGh(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferMouGh(c, t, context);
    internal void CaptureMouGuoHuaiGainBatch(CardMovementBatchContext batch) => _resolve().CaptureMouGuoHuaiGainBatch(batch);
    internal PromptChoice ChooseMouGhAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseMouGhAi(p, f);
    internal IEnumerable<ProgramTriggerCandidate> CollectMouGuoHuaiGainCandidates(CardMovementBatchContext batch) => _resolve().CollectMouGuoHuaiGainCandidates(batch);
    internal void CollectWindow_CollectMouGuoHuaiGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectMouGuoHuaiGainCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouGuoHuaiGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouGuoHuaiGainBatch(batch, completed);
    internal void ContinueMouGh(long id) => _resolve().ContinueMouGh(id);
    internal ProgramSkillWindowContext CreateMouGuoHuaiGainContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate c) => _resolve().CreateMouGuoHuaiGainContext(parent, c);
    internal bool DrainMouGh(ProgramSkillFrame f) => _resolve().DrainMouGh(f);
    internal bool ExcludeOrdinaryMovement_IsMouGuoHuaiGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsMouGuoHuaiGainTrigger(candidate, trigger);
    internal void FinishMouGh(ProgramSkillFrame f) => _resolve().FinishMouGh(f);
    internal void FreezeMouGhActualStart(TurnStartedEvent turn) => _resolve().FreezeMouGhActualStart(turn);
    internal bool IsMouGhDying() => _resolve().IsMouGhDying();
    internal bool KeepMouGhPaid(ProgramSkillFrame f) => _resolve().KeepMouGhPaid(f);
    internal bool LiveMouGhSource(CardConversionSource source, string hash) => _resolve().LiveMouGhSource(source, hash);
    internal PromptChoice MouGhChoice(ProgramSkillFrame f, string branch, string description, int? card = default, int? target = default) => _resolve().MouGhChoice(f, branch, description, card, target);
    internal IReadOnlyList<PromptChoice> MouGhChoices(ProgramSkillFrame f) => _resolve().MouGhChoices(f);
    internal bool MouGhFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouGhFirstChild(f, child);
    internal ProgramSkillFrame? MouGhObserverRoot() => _resolve().MouGhObserverRoot();
    internal bool MouGhRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouGhRecoveryParent(f, recovery);
    internal bool MouGhSuffix(int index) => _resolve().MouGhSuffix(index);
    internal MouGuoHuaiGainQualifiedEvent? MouGuoHuaiGainQualification(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().MouGuoHuaiGainQualification(c, context);
    internal void PlaceMouGhPredictions(ProgramSkillFrame f) => _resolve().PlaceMouGhPredictions(f);
    internal bool? ProveMouGuoHuaiMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveMouGuoHuaiMovement(trigger, candidate, context);
    internal void PublishMouGh(ProgramSkillFrame f) => _resolve().PublishMouGh(f);
    internal bool ResumeMouGh(long id) => _resolve().ResumeMouGh(id);
    internal void ReturnMouGhDraw(ProgramSkillFrame f, NativeDrawOutcome o) => _resolve().ReturnMouGhDraw(f, o);
    internal bool ReturnMouGhMovement(ProgramSkillFrame f) => _resolve().ReturnMouGhMovement(f);
    internal void SealMouGh(long id) => _resolve().SealMouGh(id);
    internal void StoreMouGhTop(ProgramSkillFrame f) => _resolve().StoreMouGhTop(f);
    internal bool TryBeginMouGuoHuaiActualStart(CharacterState owner) => _resolve().TryBeginMouGuoHuaiActualStart(owner);
    internal ProgramSkillWindowContext? TryCreateMouGuoHuaiGainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateMouGuoHuaiGainContext(frame, candidate);
    internal IEnumerable<MouGuoHuaiPredictionIssuedEvent> UnverifiedMouGhPredictions() => _resolve().UnverifiedMouGhPredictions();
    internal bool ValidMouGh(ProgramSkillFrame f) => _resolve().ValidMouGh(f);
    internal bool HasMouGuoHuaiMobile { get => _resolve().HasMouGuoHuaiMobile; }
}
