#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuBaOlEntries
{
    internal bool AdvanceLiuBaChildren() => _resolve().AdvanceLiuBaChildren();
    internal bool AllowsLiuBaRecast(ProgramActivationLegalityContext c) => _resolve().AllowsLiuBaRecast(c);
    internal void AnswerLiuBa(PromptChoice supplied) => _resolve().AnswerLiuBa(supplied);
    internal void AppendLiuBaCoinEndingItems(List<TurnEndingBoundaryItem> items) => _resolve().AppendLiuBaCoinEndingItems(items);
    internal SkillProgramStepOutcome BeginLiuBa(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginLiuBa(supplied, op);
    internal bool CanOfferLiuBa(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferLiuBa(c, t, context);
    internal PromptChoice ChooseLiuBa(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseLiuBa(p, f);
    internal void CompleteLiuBaMovement(ProgramSkillFrame f, int count) => _resolve().CompleteLiuBaMovement(f, count);
    internal void ContinueLiuBa(long id) => _resolve().ContinueLiuBa(id);
    internal bool ContinuePaidLiuBa(ProgramSkillFrame f) => _resolve().ContinuePaidLiuBa(f);
    internal TongduGiftIssuedEvent? CurrentTongduDebt(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CurrentTongduDebt(c, context);
    internal bool DrainLiuBa(ProgramSkillFrame f) => _resolve().DrainLiuBa(f);
    internal void FinishLiuBa(ProgramSkillFrame f) => _resolve().FinishLiuBa(f);
    internal bool HasLiuBaCoinEntity(int cardId) => _resolve().HasLiuBaCoinEntity(cardId);
    internal bool IsIssuedLiuBaCoinEndingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedLiuBaCoinEndingCandidate(c, context);
    internal bool IsIssuedTongduCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedTongduCandidate(c, context);
    internal bool IsLiuBaRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsLiuBaRecoveryParent(f, recovery);
    internal IEnumerable<ProgramTriggerCandidate> IssuedTongduPlayEndingCandidates(CharacterState owner) => _resolve().IssuedTongduPlayEndingCandidates(owner);
    internal PromptChoice LiuBaChoice(ProgramSkillFrame f, string branch, string label, int? card = default, int? seat = default) => _resolve().LiuBaChoice(f, branch, label, card, seat);
    internal IReadOnlyList<PromptChoice> LiuBaChoices(ProgramSkillFrame f) => _resolve().LiuBaChoices(f);
    internal int LiuBaChooser(ProgramSkillFrame f) => _resolve().LiuBaChooser(f);
    internal LiuBaCoinMintedEvent? LiuBaCoin(int id) => _resolve().LiuBaCoin(id);
    internal LiuBaEntity[] LiuBaCoins(int seat) => _resolve().LiuBaCoins(seat);
    internal bool LiuBaFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().LiuBaFirstChild(f, child);
    internal bool LiuBaLion(ProgramSkillFrame f, int seat) => _resolve().LiuBaLion(f, seat);
    internal bool LiuBaMovementMatches(ProgramSkillFrame f, CardMovementRecord m) => _resolve().LiuBaMovementMatches(f, m);
    internal ProgramSkillFrame? LiuBaObserverRoot() => _resolve().LiuBaObserverRoot();
    internal LiuBaEntity[] LiuBaOwnedEntities(int seat) => _resolve().LiuBaOwnedEntities(seat);
    internal IReadOnlyList<CardSnapshot> LiuBaPrivatelyViewedBottomCards(int viewerSeat) => _resolve().LiuBaPrivatelyViewedBottomCards(viewerSeat);
    internal LiuBaEntity[] LiuBaRecastEntities(int seat) => _resolve().LiuBaRecastEntities(seat);
    internal void PayLiuBaExchange(ProgramSkillFrame f) => _resolve().PayLiuBaExchange(f);
    internal void PayLiuBaRecast(ProgramSkillFrame f, LiuBaEntity material) => _resolve().PayLiuBaRecast(f, material);
    internal void PayTongduGift(ProgramSkillFrame f, LiuBaEntity card) => _resolve().PayTongduGift(f, card);
    internal void PayTongduReturn(ProgramSkillFrame f) => _resolve().PayTongduReturn(f);
    internal void PrepareLiuBaMovement(ref ProgramSkillFrame f, LiuBaStage stage, IReadOnlyList<LiuBaEntity> entities) => _resolve().PrepareLiuBaMovement(ref f, stage, entities);
    internal void ProjectLiuBaCoinCardSkillMark(int cardId, ref CardSkillMarkProjection projection) => _resolve().ProjectLiuBaCoinCardSkillMark(cardId, ref projection);
    internal void PublishLiuBa(ProgramSkillFrame f) => _resolve().PublishLiuBa(f);
    internal void RecordLiuBaMovement(ProgramSkillFrame f, long batch, IReadOnlyList<CardMovementRecord> records) => _resolve().RecordLiuBaMovement(f, batch, records);
    internal bool ResumeLiuBa(long id) => _resolve().ResumeLiuBa(id);
    internal bool ReturnLiuBaMovement(ProgramSkillFrame f) => _resolve().ReturnLiuBaMovement(f);
    internal bool TongduConsumed(long id) => _resolve().TongduConsumed(id);
    internal bool ValidLiuBaCoin(LiuBaCoinMintedEvent e) => _resolve().ValidLiuBaCoin(e);
    internal bool ValidTongduGift(TongduGiftIssuedEvent e) => _resolve().ValidTongduGift(e);
    internal bool HasLiuBaCapability { get => _resolve().HasLiuBaCapability; }
}
