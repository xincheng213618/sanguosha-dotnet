#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHouChengAnniversaryEntries
{
    internal void AddHouChengWineActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = default) => _resolve().AddHouChengWineActions(actions, actor, playable, selectedPhysicalCardId);
    internal bool AdvanceHouChengChildren() => _resolve().AdvanceHouChengChildren();
    internal IReadOnlyList<CardConversionSource> AnniversaryHouChengWineSources(CharacterState owner, Card card, CardKind kind, bool forResponse, bool dyingUse) => _resolve().AnniversaryHouChengWineSources(owner, card, kind, forResponse, dyingUse);
    internal void AnswerHouCheng(PromptChoice c) => _resolve().AnswerHouCheng(c);
    internal SkillProgramStepOutcome BeginHouCheng(ProgramSkillFrame supplied) => _resolve().BeginHouCheng(supplied);
    internal bool CanOfferHouCheng(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c) => _resolve().CanOfferHouCheng(candidate, t, c);
    internal void ContinueHouCheng(long id) => _resolve().ContinueHouCheng(id);
    internal bool DrainHouCheng(ProgramSkillFrame f) => _resolve().DrainHouCheng(f);
    internal PromptChoice HouChengAi(PendingDecision p) => _resolve().HouChengAi(p);
    internal IReadOnlyList<PromptChoice> HouChengChoices(ProgramSkillFrame f) => _resolve().HouChengChoices(f);
    internal bool HouChengDyingObserver() => _resolve().HouChengDyingObserver();
    internal bool HouChengFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HouChengFirstChild(f, child);
    internal bool HouChengForeignDiscard(int owner, ProgramSkillWindowContext c) => _resolve().HouChengForeignDiscard(owner, c);
    internal bool HouChengLegalUseBan(ProgramLegalActionQuery q) => _resolve().HouChengLegalUseBan(q);
    internal bool HouChengMaterialBan(ProgramCardMaterialQuery q) => _resolve().HouChengMaterialBan(q);
    internal bool HouChengMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().HouChengMovementProof(f, effect, p);
    internal ProgramSkillFrame? HouChengObserverRoot() => _resolve().HouChengObserverRoot();
    internal bool HouChengParent(ProgramSkillFrame f) => _resolve().HouChengParent(f);
    internal IEnumerable<int> HouChengRecipients(ProgramSkillFrame f) => _resolve().HouChengRecipients(f);
    internal bool HouChengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child) => _resolve().HouChengRecoveryParent(f, child);
    internal bool HouChengResponseBan(ProgramNativeResponseQuery q) => _resolve().HouChengResponseBan(q);
    internal int HouChengRoundGain(int owner, int round) => _resolve().HouChengRoundGain(owner, round);
    internal Card HouChengSlot(ProgramSkillFrame f, PromptChoice c) => _resolve().HouChengSlot(f, c);
    internal IEnumerable<int> HouChengSources(ProgramSkillFrame f) => _resolve().HouChengSources(f);
    internal void HouChengSubmittedBan(ProgramSubmittedUseQuery q) => _resolve().HouChengSubmittedBan(q);
    internal bool HouChengSuffix(int root) => _resolve().HouChengSuffix(root);
    internal bool HouChengUseBanned(int actor, CardKind kind, IEnumerable<int> ids, bool response, bool dying = false) => _resolve().HouChengUseBanned(actor, kind, ids, response, dying);
    internal bool HouChengViewAsBan(ProgramViewAsLegalityQuery q) => _resolve().HouChengViewAsBan(q);
    internal AnniversaryHouChengMovementEvent? HouChengWineMark(int cardId) => _resolve().HouChengWineMark(cardId);
    internal bool IsAnniversaryHouChengWineCard(int cardId) => _resolve().IsAnniversaryHouChengWineCard(cardId);
    internal void MoveHouCheng(ProgramSkillFrame f) => _resolve().MoveHouCheng(f);
    internal void PublishHouCheng(ProgramSkillFrame f) => _resolve().PublishHouCheng(f);
    internal bool ResumeHouCheng(long id) => _resolve().ResumeHouCheng(id);
    internal bool RetainHouCheng(ProgramSkillFrame f) => _resolve().RetainHouCheng(f);
    internal bool ReturnHouChengMovement(ProgramSkillFrame f) => _resolve().ReturnHouChengMovement(f);
    internal void SettleHouCheng(ProgramSkillFrame f) => _resolve().SettleHouCheng(f);
    internal bool ValidHouCheng(ProgramSkillFrame f) => _resolve().ValidHouCheng(f);
    internal bool ValidHouChengMovement(AnniversaryHouChengMovementEvent e) => _resolve().ValidHouChengMovement(e);
    internal bool HasHouChengContent { get => _resolve().HasHouChengContent; }
}
