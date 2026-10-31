#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuRongMouMobileEntries
{
    internal bool CanOfferMouZhuRong(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferMouZhuRong(candidate, t, c);
    internal void CollectIssuedMouZhuRongLieren(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedMouZhuRongLieren(action, window, result);
    internal bool ContinueMouZhuRongPaidLieren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueMouZhuRongPaidLieren(f);
    internal global::CardGame.Core.SkillProgramStepOutcome GiftMouZhuRongOutsideAssault(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().GiftMouZhuRongOutsideAssault(supplied);
    internal bool IsIssuedMouZhuRongLieren(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().IsIssuedMouZhuRongLieren(candidate, c);
    internal bool IsMouZhuRongGiftMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation m) => _resolve().IsMouZhuRongGiftMovement(f, e, m);
    internal global::System.Collections.Generic.IReadOnlyList<int> MouZhuRongLierenOtherTargets(int owner) => _resolve().MouZhuRongLierenOtherTargets(owner);
    internal global::CardGame.Core.MouZhuRongLierenWonEvent? MouZhuRongWin(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().MouZhuRongWin(owner, c);
    internal void ObserveMouZhuRongAssault(global::CardGame.Core.CardUseDeclaredEvent e) => _resolve().ObserveMouZhuRongAssault(e);
    internal void RecordMouZhuRongLieren(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().RecordMouZhuRongLieren(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome SelectMouZhuRongLierenTarget(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().SelectMouZhuRongLierenTarget(supplied);
    internal bool HasMouZhuRong { get => _resolve().HasMouZhuRong; }
    internal int MouZhuRongTurnOrdinal { get => _resolve().MouZhuRongTurnOrdinal; }
}
