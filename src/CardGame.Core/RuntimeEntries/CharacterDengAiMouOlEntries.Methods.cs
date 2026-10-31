#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDengAiMouOlEntries
{
    internal bool AdvanceMouDengAiChildren() => _resolve().AdvanceMouDengAiChildren();
    internal global::CardGame.Core.PromptChoice AiMouDengAiOl(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouDengAiOl(decision, f);
    internal void AnswerMouDengAiOl(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMouDengAiOl(choice);
    internal bool CanOfferMouDengAiOl(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferMouDengAiOl(c);
    internal void CaptureMouDengAiUsedDiscard(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureMouDengAiUsedDiscard(batch);
    internal void ConfirmMouDengAiExchange(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ConfirmMouDengAiExchange(f);
    internal void ContinueMouDengAiOl(long id) => _resolve().ContinueMouDengAiOl(id);
    internal bool ContinuePaidMouDengAi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidMouDengAi(f);
    internal bool DrainMouDengAiOl(long id) => _resolve().DrainMouDengAiOl(id);
    internal void FinishMouDengAiOl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouDengAiOl(f);
    internal bool HasMouDengAiBeforeDamage(long id) => _resolve().HasMouDengAiBeforeDamage(id);
    internal bool HasMouDengAiDamage(long id) => _resolve().HasMouDengAiDamage(id);
    internal bool IsMouDengAiDying() => _resolve().IsMouDengAiDying();
    internal bool IsMouDengAiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation m) => _resolve().IsMouDengAiMovement(f, effect, m);
    internal bool IsMouDengAiNativeSettlementBatch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementBatchContext b) => _resolve().IsMouDengAiNativeSettlementBatch(f, b);
    internal bool IsMouDengAiSnatch(long id) => _resolve().IsMouDengAiSnatch(id);
    internal void IssueMouDengAiSnatch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption selected) => _resolve().IssueMouDengAiSnatch(f, selected);
    internal bool MouDengAiCanGrow(int owner) => _resolve().MouDengAiCanGrow(owner);
    internal global::CardGame.Core.PromptChoice MouDengAiChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, int? card = default, global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption? option = default) => _resolve().MouDengAiChoice(f, branch, label, card, option);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouDengAiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouDengAiChoices(f);
    internal int[] MouDengAiDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().MouDengAiDiscardIndexes(batch);
    internal bool MouDengAiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouDengAiFirstChild(f, child);
    internal global::CardGame.Core.Card[] MouDengAiGu(int owner) => _resolve().MouDengAiGu(owner);
    internal bool MouDengAiIsGu(int owner, int id) => _resolve().MouDengAiIsGu(owner, id);
    internal bool MouDengAiJiewanDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().MouDengAiJiewanDistance(q);
    internal bool MouDengAiNativeEdge(int n) => _resolve().MouDengAiNativeEdge(n);
    internal bool MouDengAiObserverEdge(int n) => _resolve().MouDengAiObserverEdge(n);
    internal global::CardGame.Core.ProgramSkillFrame? MouDengAiObserverRoot() => _resolve().MouDengAiObserverRoot();
    internal bool MouDengAiObserverSuffix(int n) => _resolve().MouDengAiObserverSuffix(n);
    internal global::CardGame.Core.PublicPersistentPileSource[] MouDengAiPiles(int owner) => _resolve().MouDengAiPiles(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> MouDengAiSnatchOptions(global::CardGame.Core.ProgramSkillFrame f, int id) => _resolve().MouDengAiSnatchOptions(f, id);
    internal void ObserveMouDengAiEntry(global::CardGame.Core.IGameEvent fact) => _resolve().ObserveMouDengAiEntry(fact);
    internal void ObserveMouDengAiSnatchDeclaration(global::CardGame.Core.IGameEvent fact) => _resolve().ObserveMouDengAiSnatchDeclaration(fact);
    internal void PayMouDengAiGu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayMouDengAiGu(f);
    internal void PublishMouDengAiOl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouDengAiOl(f);
    internal bool ResumeMouDengAiOl(long id) => _resolve().ResumeMouDengAiOl(id);
    internal bool ReturnMouDengAiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouDengAiMovement(f);
    internal void ReturnMouDengAiSnatch(global::CardGame.Core.CardUseFrame u) => _resolve().ReturnMouDengAiSnatch(u);
    internal void SealMouDengAiMovement(long id) => _resolve().SealMouDengAiMovement(id);
    internal int[]? SelectMouDengAiDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramInstructionFeatures features) => _resolve().SelectMouDengAiDiscardIndexes(batch, candidate, features);
    internal global::CardGame.Core.SkillProgramStepOutcome StartMouDengAiOl(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().StartMouDengAiOl(supplied, op);
    internal bool ValidMouDengAiOl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouDengAiOl(f);
    internal void ValidateMouDengAiSnatch(global::CardGame.Core.CardUseFrame u) => _resolve().ValidateMouDengAiSnatch(u);
    internal bool WindowSettlement_IsMouDengAiNativeSettlementBatch(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().WindowSettlement_IsMouDengAiNativeSettlementBatch(frame, batch);
    internal bool HasMouDengAiOl { get => _resolve().HasMouDengAiOl; }
}
