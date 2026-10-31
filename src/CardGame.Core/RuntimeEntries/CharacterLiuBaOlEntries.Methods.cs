#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuBaOlEntries
{
    internal bool AdvanceLiuBaChildren() => _resolve().AdvanceLiuBaChildren();
    internal bool AllowsLiuBaRecast(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().AllowsLiuBaRecast(c);
    internal void AnswerLiuBa(global::CardGame.Core.PromptChoice supplied) => _resolve().AnswerLiuBa(supplied);
    internal void AppendLiuBaCoinEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => _resolve().AppendLiuBaCoinEndingItems(items);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLiuBa(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginLiuBa(supplied, op);
    internal bool CanOfferLiuBa(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferLiuBa(c, t, context);
    internal global::CardGame.Core.PromptChoice ChooseLiuBa(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseLiuBa(p, f);
    internal void CompleteLiuBaMovement(global::CardGame.Core.ProgramSkillFrame f, int count) => _resolve().CompleteLiuBaMovement(f, count);
    internal void ContinueLiuBa(long id) => _resolve().ContinueLiuBa(id);
    internal bool ContinuePaidLiuBa(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidLiuBa(f);
    internal global::CardGame.Core.TongduGiftIssuedEvent? CurrentTongduDebt(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CurrentTongduDebt(c, context);
    internal bool DrainLiuBa(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainLiuBa(f);
    internal void FinishLiuBa(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishLiuBa(f);
    internal bool HasLiuBaBeforeDamageObserver(long id) => _resolve().HasLiuBaBeforeDamageObserver(id);
    internal bool HasLiuBaCoinEntity(int cardId) => _resolve().HasLiuBaCoinEntity(cardId);
    internal bool HasLiuBaDamageObserver(long id) => _resolve().HasLiuBaDamageObserver(id);
    internal bool IsIssuedLiuBaCoinEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedLiuBaCoinEndingCandidate(c, context);
    internal bool IsIssuedTongduCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedTongduCandidate(c, context);
    internal bool IsLiuBaMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsLiuBaMovement(f, e, p);
    internal bool IsLiuBaRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsLiuBaRecoveryParent(f, recovery);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedTongduPlayEndingCandidates(global::CardGame.Core.CharacterState owner) => _resolve().IssuedTongduPlayEndingCandidates(owner);
    internal global::CardGame.Core.PromptChoice LiuBaChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, int? card = default, int? seat = default) => _resolve().LiuBaChoice(f, branch, label, card, seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LiuBaChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiuBaChoices(f);
    internal int LiuBaChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiuBaChooser(f);
    internal global::CardGame.Core.LiuBaCoinMintedEvent? LiuBaCoin(int id) => _resolve().LiuBaCoin(id);
    internal global::CardGame.Core.LiuBaEntity[] LiuBaCoins(int seat) => _resolve().LiuBaCoins(seat);
    internal bool LiuBaFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().LiuBaFirstChild(f, child);
    internal bool LiuBaLion(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().LiuBaLion(f, seat);
    internal bool LiuBaMovementMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().LiuBaMovementMatches(f, m);
    internal global::CardGame.Core.ProgramSkillFrame? LiuBaObserverRoot() => _resolve().LiuBaObserverRoot();
    internal global::CardGame.Core.LiuBaEntity[] LiuBaOwnedEntities(int seat) => _resolve().LiuBaOwnedEntities(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> LiuBaPrivatelyViewedBottomCards(int viewerSeat) => _resolve().LiuBaPrivatelyViewedBottomCards(viewerSeat);
    internal global::CardGame.Core.LiuBaEntity[] LiuBaRecastEntities(int seat) => _resolve().LiuBaRecastEntities(seat);
    internal void PayLiuBaExchange(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayLiuBaExchange(f);
    internal void PayLiuBaRecast(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LiuBaEntity material) => _resolve().PayLiuBaRecast(f, material);
    internal void PayTongduGift(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LiuBaEntity card) => _resolve().PayTongduGift(f, card);
    internal void PayTongduReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayTongduReturn(f);
    internal void PrepareLiuBaMovement(ref global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LiuBaStage stage, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LiuBaEntity> entities) => _resolve().PrepareLiuBaMovement(ref f, stage, entities);
    internal void ProjectLiuBaCoinCardSkillMark(int cardId, ref global::CardGame.Core.CardSkillMarkProjection projection) => _resolve().ProjectLiuBaCoinCardSkillMark(cardId, ref projection);
    internal void PublishLiuBa(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishLiuBa(f);
    internal void RecordLiuBaMovement(global::CardGame.Core.ProgramSkillFrame f, long batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> records) => _resolve().RecordLiuBaMovement(f, batch, records);
    internal bool ResumeLiuBa(long id) => _resolve().ResumeLiuBa(id);
    internal bool ReturnLiuBaMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnLiuBaMovement(f);
    internal bool TongduConsumed(long id) => _resolve().TongduConsumed(id);
    internal bool ValidLiuBaCoin(global::CardGame.Core.LiuBaCoinMintedEvent e) => _resolve().ValidLiuBaCoin(e);
    internal bool ValidTongduGift(global::CardGame.Core.TongduGiftIssuedEvent e) => _resolve().ValidTongduGift(e);
    internal bool HasLiuBaCapability { get => _resolve().HasLiuBaCapability; }
}
