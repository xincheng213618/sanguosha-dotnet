#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDengAiMouOlEntries
{
    internal bool AdvanceMouDengAiChildren() => _resolve().AdvanceMouDengAiChildren();
    internal PromptChoice AiMouDengAiOl(PendingDecision decision, ProgramSkillFrame f) => _resolve().AiMouDengAiOl(decision, f);
    internal void AnswerMouDengAiOl(PromptChoice choice) => _resolve().AnswerMouDengAiOl(choice);
    internal bool CanOfferMouDengAiOl(ProgramTriggerLegalityContext c) => _resolve().CanOfferMouDengAiOl(c);
    internal void CaptureMouDengAiUsedDiscard(CardMovementBatchContext batch) => _resolve().CaptureMouDengAiUsedDiscard(batch);
    internal void ConfirmMouDengAiExchange(ProgramSkillFrame f) => _resolve().ConfirmMouDengAiExchange(f);
    internal void ContinueMouDengAiOl(long id) => _resolve().ContinueMouDengAiOl(id);
    internal bool ContinuePaidMouDengAi(ProgramSkillFrame f) => _resolve().ContinuePaidMouDengAi(f);
    internal bool DrainMouDengAiOl(long id) => _resolve().DrainMouDengAiOl(id);
    internal void FinishMouDengAiOl(ProgramSkillFrame f) => _resolve().FinishMouDengAiOl(f);
    internal bool IsMouDengAiDying() => _resolve().IsMouDengAiDying();
    internal bool IsMouDengAiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation m) => _resolve().IsMouDengAiMovement(f, effect, m);
    internal bool IsMouDengAiNativeSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => _resolve().IsMouDengAiNativeSettlementBatch(f, b);
    internal bool IsMouDengAiSnatch(long id) => _resolve().IsMouDengAiSnatch(id);
    internal void IssueMouDengAiSnatch(ProgramSkillFrame f, GameEngine.ProgramOrdinaryTrickUseOption selected) => _resolve().IssueMouDengAiSnatch(f, selected);
    internal bool MouDengAiCanGrow(int owner) => _resolve().MouDengAiCanGrow(owner);
    internal PromptChoice MouDengAiChoice(ProgramSkillFrame f, string branch, string label, int? card = default, GameEngine.ProgramOrdinaryTrickUseOption? option = default) => _resolve().MouDengAiChoice(f, branch, label, card, option);
    internal IReadOnlyList<PromptChoice> MouDengAiChoices(ProgramSkillFrame f) => _resolve().MouDengAiChoices(f);
    internal int[] MouDengAiDiscardIndexes(CardMovementBatchContext batch) => _resolve().MouDengAiDiscardIndexes(batch);
    internal bool MouDengAiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouDengAiFirstChild(f, child);
    internal Card[] MouDengAiGu(int owner) => _resolve().MouDengAiGu(owner);
    internal bool MouDengAiIsGu(int owner, int id) => _resolve().MouDengAiIsGu(owner, id);
    internal bool MouDengAiJiewanDistance(ProgramCardDistanceQuery q) => _resolve().MouDengAiJiewanDistance(q);
    internal bool MouDengAiNativeEdge(int n) => _resolve().MouDengAiNativeEdge(n);
    internal bool MouDengAiObserverEdge(int n) => _resolve().MouDengAiObserverEdge(n);
    internal ProgramSkillFrame? MouDengAiObserverRoot() => _resolve().MouDengAiObserverRoot();
    internal bool MouDengAiObserverSuffix(int n) => _resolve().MouDengAiObserverSuffix(n);
    internal PublicPersistentPileSource[] MouDengAiPiles(int owner) => _resolve().MouDengAiPiles(owner);
    internal IReadOnlyList<GameEngine.ProgramOrdinaryTrickUseOption> MouDengAiSnatchOptions(ProgramSkillFrame f, int id) => _resolve().MouDengAiSnatchOptions(f, id);
    internal void ObserveMouDengAiEntry(IGameEvent fact) => _resolve().ObserveMouDengAiEntry(fact);
    internal void ObserveMouDengAiSnatchDeclaration(IGameEvent fact) => _resolve().ObserveMouDengAiSnatchDeclaration(fact);
    internal void PayMouDengAiGu(ProgramSkillFrame f) => _resolve().PayMouDengAiGu(f);
    internal void PublishMouDengAiOl(ProgramSkillFrame f) => _resolve().PublishMouDengAiOl(f);
    internal bool ResumeMouDengAiOl(long id) => _resolve().ResumeMouDengAiOl(id);
    internal bool ReturnMouDengAiMovement(ProgramSkillFrame f) => _resolve().ReturnMouDengAiMovement(f);
    internal void ReturnMouDengAiSnatch(CardUseFrame u) => _resolve().ReturnMouDengAiSnatch(u);
    internal void SealMouDengAiMovement(long id) => _resolve().SealMouDengAiMovement(id);
    internal int[]? SelectMouDengAiDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features) => _resolve().SelectMouDengAiDiscardIndexes(batch, candidate, features);
    internal SkillProgramStepOutcome StartMouDengAiOl(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().StartMouDengAiOl(supplied, op);
    internal bool ValidMouDengAiOl(ProgramSkillFrame f) => _resolve().ValidMouDengAiOl(f);
    internal void ValidateMouDengAiSnatch(CardUseFrame u) => _resolve().ValidateMouDengAiSnatch(u);
    internal bool WindowSettlement_IsMouDengAiNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().WindowSettlement_IsMouDengAiNativeSettlementBatch(frame, batch);
    internal bool HasMouDengAiOl { get => _resolve().HasMouDengAiOl; }
}
