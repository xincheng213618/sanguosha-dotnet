#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHouChengAnniversaryEntries
{
    internal void AddHouChengWineActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playable, int? selectedPhysicalCardId = default) => _resolve().AddHouChengWineActions(actions, actor, playable, selectedPhysicalCardId);
    internal bool AdvanceHouChengChildren() => _resolve().AdvanceHouChengChildren();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> AnniversaryHouChengWineSources(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool forResponse, bool dyingUse) => _resolve().AnniversaryHouChengWineSources(owner, card, kind, forResponse, dyingUse);
    internal void AnswerHouCheng(global::CardGame.Core.PromptChoice c) => _resolve().AnswerHouCheng(c);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHouCheng(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHouCheng(supplied);
    internal bool CanOfferHouCheng(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferHouCheng(candidate, t, c);
    internal void ContinueHouCheng(long id) => _resolve().ContinueHouCheng(id);
    internal bool DrainHouCheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainHouCheng(f);
    internal global::CardGame.Core.PromptChoice HouChengAi(global::CardGame.Core.PendingDecision p) => _resolve().HouChengAi(p);
    internal bool HouChengBeforeDamageObserver(long id) => _resolve().HouChengBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HouChengChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HouChengChoices(f);
    internal bool HouChengDamageObserver(long id) => _resolve().HouChengDamageObserver(id);
    internal bool HouChengDyingObserver() => _resolve().HouChengDyingObserver();
    internal bool HouChengFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HouChengFirstChild(f, child);
    internal bool HouChengForeignDiscard(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().HouChengForeignDiscard(owner, c);
    internal bool HouChengLegalUseBan(global::CardGame.Core.ProgramLegalActionQuery q) => _resolve().HouChengLegalUseBan(q);
    internal bool HouChengMaterialBan(global::CardGame.Core.ProgramCardMaterialQuery q) => _resolve().HouChengMaterialBan(q);
    internal bool HouChengMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().HouChengMovementProof(f, effect, p);
    internal global::CardGame.Core.ProgramSkillFrame? HouChengObserverRoot() => _resolve().HouChengObserverRoot();
    internal bool HouChengParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HouChengParent(f);
    internal global::System.Collections.Generic.IEnumerable<int> HouChengRecipients(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HouChengRecipients(f);
    internal bool HouChengRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame child) => _resolve().HouChengRecoveryParent(f, child);
    internal bool HouChengResponseBan(global::CardGame.Core.ProgramNativeResponseQuery q) => _resolve().HouChengResponseBan(q);
    internal int HouChengRoundGain(int owner, int round) => _resolve().HouChengRoundGain(owner, round);
    internal global::CardGame.Core.Card HouChengSlot(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice c) => _resolve().HouChengSlot(f, c);
    internal global::System.Collections.Generic.IEnumerable<int> HouChengSources(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HouChengSources(f);
    internal void HouChengSubmittedBan(global::CardGame.Core.ProgramSubmittedUseQuery q) => _resolve().HouChengSubmittedBan(q);
    internal bool HouChengSuffix(int root) => _resolve().HouChengSuffix(root);
    internal bool HouChengUseBanned(int actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IEnumerable<int> ids, bool response, bool dying = false) => _resolve().HouChengUseBanned(actor, kind, ids, response, dying);
    internal bool HouChengViewAsBan(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().HouChengViewAsBan(q);
    internal global::CardGame.Core.AnniversaryHouChengMovementEvent? HouChengWineMark(int cardId) => _resolve().HouChengWineMark(cardId);
    internal bool IsAnniversaryHouChengWineCard(int cardId) => _resolve().IsAnniversaryHouChengWineCard(cardId);
    internal void MoveHouCheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MoveHouCheng(f);
    internal void PublishHouCheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHouCheng(f);
    internal bool ResumeHouCheng(long id) => _resolve().ResumeHouCheng(id);
    internal bool RetainHouCheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainHouCheng(f);
    internal bool ReturnHouChengMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHouChengMovement(f);
    internal void SettleHouCheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleHouCheng(f);
    internal bool ValidHouCheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHouCheng(f);
    internal bool ValidHouChengMovement(global::CardGame.Core.AnniversaryHouChengMovementEvent e) => _resolve().ValidHouChengMovement(e);
    internal bool HasHouChengContent { get => _resolve().HasHouChengContent; }
}
