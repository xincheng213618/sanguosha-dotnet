#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoShuangMobileShiEntries
{
    internal void AnswerShiCaoShuang(PromptChoice selected) => _resolve().AnswerShiCaoShuang(selected);
    internal SkillProgramStepOutcome BeginShiCaoShuang(ProgramSkillFrame input) => _resolve().BeginShiCaoShuang(input);
    internal bool CanOfferShiCaoShuang(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShiCaoShuang(candidate, trigger, context);
    internal bool CanStartShiCaoShuangNiZunUse(CharacterState owner, string skill) => _resolve().CanStartShiCaoShuangNiZunUse(owner, skill);
    internal PromptChoice ChooseShiCaoShuangJianZhuanAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiCaoShuangJianZhuanAi(decision, f);
    internal PromptChoice ChooseShiCaoShuangNiZunAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiCaoShuangNiZunAi(decision, f);
    internal PromptChoice ChooseShiCaoShuangSheQuanAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiCaoShuangSheQuanAi(decision, f);
    internal bool ContinuePaidShiCaoShuang(ProgramSkillFrame f) => _resolve().ContinuePaidShiCaoShuang(f);
    internal void ContinueShiCaoShuang(long id) => _resolve().ContinueShiCaoShuang(id);
    internal void ContinueShiCaoShuangDianYi(ProgramSkillFrame f) => _resolve().ContinueShiCaoShuangDianYi(f);
    internal void ContinueShiCaoShuangDianYiLoss(ProgramSkillFrame f) => _resolve().ContinueShiCaoShuangDianYiLoss(f);
    internal void ContinueShiCaoShuangStash(ProgramSkillFrame f) => _resolve().ContinueShiCaoShuangStash(f);
    internal bool DrainShiCaoShuang(ProgramSkillFrame f) => _resolve().DrainShiCaoShuang(f);
    internal void FinishShiCaoShuangOperation(ProgramSkillFrame f) => _resolve().FinishShiCaoShuangOperation(f);
    internal PromptChoice MakeSkillChoice(ProgramSkillFrame f, string branch, string skillId, string label) => _resolve().MakeSkillChoice(f, branch, skillId, label);
    internal void ObserveShiCaoShuangTurnFacts(IGameEvent payload) => _resolve().ObserveShiCaoShuangTurnFacts(payload);
    internal void PublishShiCaoShuang(ProgramSkillFrame f) => _resolve().PublishShiCaoShuang(f);
    internal void ResolveShiCaoShuangAwakening(ProgramSkillFrame f, string lostSkillId) => _resolve().ResolveShiCaoShuangAwakening(f, lostSkillId);
    internal void ResolveShiCaoShuangMark(ProgramSkillFrame f, int cardId) => _resolve().ResolveShiCaoShuangMark(f, cardId);
    internal void ResolveShiCaoShuangNiZunCard(ProgramSkillFrame f, int cardId) => _resolve().ResolveShiCaoShuangNiZunCard(f, cardId);
    internal void ResolveShiCaoShuangTiming(ProgramSkillFrame f, string skillId) => _resolve().ResolveShiCaoShuangTiming(f, skillId);
    internal bool ResumeShiCaoShuang(long id) => _resolve().ResumeShiCaoShuang(id);
    internal bool ReturnShiCaoShuangMovement(ProgramSkillFrame f) => _resolve().ReturnShiCaoShuangMovement(f);
    internal IReadOnlyList<PromptChoice> ShiCaoShuangChoices(ProgramSkillFrame f) => _resolve().ShiCaoShuangChoices(f);
    internal int ShiCaoShuangDianYiGainsThisRound(int owner) => _resolve().ShiCaoShuangDianYiGainsThisRound(owner);
    internal int ShiCaoShuangDianYiUsesThisRound(int owner) => _resolve().ShiCaoShuangDianYiUsesThisRound(owner);
    internal bool ShiCaoShuangHasSkill(int seat, string skillId) => _resolve().ShiCaoShuangHasSkill(seat, skillId);
    internal bool ShiCaoShuangIsMarkedCard(int cardId) => _resolve().ShiCaoShuangIsMarkedCard(cardId);
    internal IReadOnlyList<int> ShiCaoShuangMarkedInOtherHands(int owner) => _resolve().ShiCaoShuangMarkedInOtherHands(owner);
    internal int ShiCaoShuangNiZunDamagePenalty(GameEngine.IDamageAttempt attack) => _resolve().ShiCaoShuangNiZunDamagePenalty(attack);
    internal bool ShiCaoShuangNiZunDamagePenaltyPending(int target) => _resolve().ShiCaoShuangNiZunDamagePenaltyPending(target);
    internal IReadOnlyList<ShiCaoShuangNiZunUsedEvent> ShiCaoShuangNiZunUseEntriesThisRound(int owner) => _resolve().ShiCaoShuangNiZunUseEntriesThisRound(owner);
    internal int ShiCaoShuangNiZunUsesThisRound(int owner) => _resolve().ShiCaoShuangNiZunUsesThisRound(owner);
    internal int ShiCaoShuangRoundTriggers(bool dianyi, int owner, string windowKind) => _resolve().ShiCaoShuangRoundTriggers(dianyi, owner, windowKind);
    internal int ShiCaoShuangSheQuanRank(int holderSeat, int cardId) => _resolve().ShiCaoShuangSheQuanRank(holderSeat, cardId);
    internal IReadOnlyList<int> ShiCaoShuangStashableIds(int owner) => _resolve().ShiCaoShuangStashableIds(owner);
    internal IReadOnlyList<int> ShiCaoShuangStashedIds(int owner) => _resolve().ShiCaoShuangStashedIds(owner);
    internal int ShiCaoShuangTimings(string skillId, string? windowKind) => _resolve().ShiCaoShuangTimings(skillId, windowKind);
    internal ShiCaoShuangReceipt StartShiCaoShuangDianYi(ProgramSkillFrame f) => _resolve().StartShiCaoShuangDianYi(f);
    internal ShiCaoShuangReceipt StartShiCaoShuangJianZhuan(ProgramSkillFrame f) => _resolve().StartShiCaoShuangJianZhuan(f);
    internal ShiCaoShuangReceipt StartShiCaoShuangNiZunStash(ProgramSkillFrame f) => _resolve().StartShiCaoShuangNiZunStash(f);
    internal ShiCaoShuangReceipt StartShiCaoShuangNiZunUse(ProgramSkillFrame f) => _resolve().StartShiCaoShuangNiZunUse(f);
    internal ShiCaoShuangReceipt StartShiCaoShuangSheQuan(ProgramSkillFrame f) => _resolve().StartShiCaoShuangSheQuan(f);
    internal void SyncShiCaoShuangMarker(int seat) => _resolve().SyncShiCaoShuangMarker(seat);
    internal bool HasShiCaoShuangCapability { get => _resolve().HasShiCaoShuangCapability; }
    internal IReadOnlyList<int> ShiCaoShuangMarkedIds { get => _resolve().ShiCaoShuangMarkedIds; }
    internal int? ShiCaoShuangOwnerSeat { get => _resolve().ShiCaoShuangOwnerSeat; }
}
