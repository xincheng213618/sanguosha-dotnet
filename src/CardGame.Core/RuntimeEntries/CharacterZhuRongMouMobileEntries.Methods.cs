#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuRongMouMobileEntries
{
    internal bool CanOfferMouZhuRong(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c) => _resolve().CanOfferMouZhuRong(candidate, t, c);
    internal void CollectIssuedMouZhuRongLieren(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedMouZhuRongLieren(action, window, result);
    internal bool ContinueMouZhuRongPaidLieren(ProgramSkillFrame f) => _resolve().ContinueMouZhuRongPaidLieren(f);
    internal SkillProgramStepOutcome GiftMouZhuRongOutsideAssault(ProgramSkillFrame supplied) => _resolve().GiftMouZhuRongOutsideAssault(supplied);
    internal bool IsIssuedMouZhuRongLieren(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().IsIssuedMouZhuRongLieren(candidate, c);
    internal IReadOnlyList<int> MouZhuRongLierenOtherTargets(int owner) => _resolve().MouZhuRongLierenOtherTargets(owner);
    internal MouZhuRongLierenWonEvent? MouZhuRongWin(int owner, ProgramSkillWindowContext c) => _resolve().MouZhuRongWin(owner, c);
    internal void ObserveMouZhuRongAssault(CardUseDeclaredEvent e) => _resolve().ObserveMouZhuRongAssault(e);
    internal void RecordMouZhuRongLieren(ProgramSkillFrame supplied) => _resolve().RecordMouZhuRongLieren(supplied);
    internal SkillProgramStepOutcome SelectMouZhuRongLierenTarget(ProgramSkillFrame supplied) => _resolve().SelectMouZhuRongLierenTarget(supplied);
    internal bool HasMouZhuRong { get => _resolve().HasMouZhuRong; }
    internal int MouZhuRongTurnOrdinal { get => _resolve().MouZhuRongTurnOrdinal; }
}
