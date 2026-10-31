#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoShuangMobileShiEntries
{
    internal void AnswerShiCaoShuang(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiCaoShuang(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiCaoShuang(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiCaoShuang(input);
    internal bool CanOfferShiCaoShuang(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShiCaoShuang(candidate, trigger, context);
    internal bool CanStartShiCaoShuangNiZunUse(global::CardGame.Core.CharacterState owner, string skill) => _resolve().CanStartShiCaoShuangNiZunUse(owner, skill);
    internal global::CardGame.Core.PromptChoice ChooseShiCaoShuangJianZhuanAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiCaoShuangJianZhuanAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiCaoShuangNiZunAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiCaoShuangNiZunAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiCaoShuangSheQuanAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiCaoShuangSheQuanAi(decision, f);
    internal bool ContinuePaidShiCaoShuang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidShiCaoShuang(f);
    internal void ContinueShiCaoShuang(long id) => _resolve().ContinueShiCaoShuang(id);
    internal void ContinueShiCaoShuangDianYi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiCaoShuangDianYi(f);
    internal void ContinueShiCaoShuangDianYiLoss(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiCaoShuangDianYiLoss(f);
    internal void ContinueShiCaoShuangStash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiCaoShuangStash(f);
    internal bool DrainShiCaoShuang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiCaoShuang(f);
    internal void FinishShiCaoShuangOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiCaoShuangOperation(f);
    internal global::CardGame.Core.PromptChoice MakeSkillChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string skillId, string label) => _resolve().MakeSkillChoice(f, branch, skillId, label);
    internal void ObserveShiCaoShuangTurnFacts(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveShiCaoShuangTurnFacts(payload);
    internal void PublishShiCaoShuang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiCaoShuang(f);
    internal void ResolveShiCaoShuangAwakening(global::CardGame.Core.ProgramSkillFrame f, string lostSkillId) => _resolve().ResolveShiCaoShuangAwakening(f, lostSkillId);
    internal void ResolveShiCaoShuangMark(global::CardGame.Core.ProgramSkillFrame f, int cardId) => _resolve().ResolveShiCaoShuangMark(f, cardId);
    internal void ResolveShiCaoShuangNiZunCard(global::CardGame.Core.ProgramSkillFrame f, int cardId) => _resolve().ResolveShiCaoShuangNiZunCard(f, cardId);
    internal void ResolveShiCaoShuangTiming(global::CardGame.Core.ProgramSkillFrame f, string skillId) => _resolve().ResolveShiCaoShuangTiming(f, skillId);
    internal bool ResumeShiCaoShuang(long id) => _resolve().ResumeShiCaoShuang(id);
    internal bool ReturnShiCaoShuangMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiCaoShuangMovement(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiCaoShuangChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiCaoShuangChoices(f);
    internal int ShiCaoShuangDianYiGainsThisRound(int owner) => _resolve().ShiCaoShuangDianYiGainsThisRound(owner);
    internal int ShiCaoShuangDianYiUsesThisRound(int owner) => _resolve().ShiCaoShuangDianYiUsesThisRound(owner);
    internal bool ShiCaoShuangHasSkill(int seat, string skillId) => _resolve().ShiCaoShuangHasSkill(seat, skillId);
    internal bool ShiCaoShuangIsMarkedCard(int cardId) => _resolve().ShiCaoShuangIsMarkedCard(cardId);
    internal global::System.Collections.Generic.IReadOnlyList<int> ShiCaoShuangMarkedInOtherHands(int owner) => _resolve().ShiCaoShuangMarkedInOtherHands(owner);
    internal int ShiCaoShuangNiZunDamagePenalty(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ShiCaoShuangNiZunDamagePenalty(attack);
    internal bool ShiCaoShuangNiZunDamagePenaltyPending(int target) => _resolve().ShiCaoShuangNiZunDamagePenaltyPending(target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ShiCaoShuangNiZunUsedEvent> ShiCaoShuangNiZunUseEntriesThisRound(int owner) => _resolve().ShiCaoShuangNiZunUseEntriesThisRound(owner);
    internal int ShiCaoShuangNiZunUsesThisRound(int owner) => _resolve().ShiCaoShuangNiZunUsesThisRound(owner);
    internal int ShiCaoShuangRoundTriggers(bool dianyi, int owner, string windowKind) => _resolve().ShiCaoShuangRoundTriggers(dianyi, owner, windowKind);
    internal int ShiCaoShuangSheQuanRank(int holderSeat, int cardId) => _resolve().ShiCaoShuangSheQuanRank(holderSeat, cardId);
    internal global::System.Collections.Generic.IReadOnlyList<int> ShiCaoShuangStashableIds(int owner) => _resolve().ShiCaoShuangStashableIds(owner);
    internal global::System.Collections.Generic.IReadOnlyList<int> ShiCaoShuangStashedIds(int owner) => _resolve().ShiCaoShuangStashedIds(owner);
    internal int ShiCaoShuangTimings(string skillId, string? windowKind) => _resolve().ShiCaoShuangTimings(skillId, windowKind);
    internal global::CardGame.Core.ShiCaoShuangReceipt StartShiCaoShuangDianYi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartShiCaoShuangDianYi(f);
    internal global::CardGame.Core.ShiCaoShuangReceipt StartShiCaoShuangJianZhuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartShiCaoShuangJianZhuan(f);
    internal global::CardGame.Core.ShiCaoShuangReceipt StartShiCaoShuangNiZunStash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartShiCaoShuangNiZunStash(f);
    internal global::CardGame.Core.ShiCaoShuangReceipt StartShiCaoShuangNiZunUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartShiCaoShuangNiZunUse(f);
    internal global::CardGame.Core.ShiCaoShuangReceipt StartShiCaoShuangSheQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartShiCaoShuangSheQuan(f);
    internal void SyncShiCaoShuangMarker(int seat) => _resolve().SyncShiCaoShuangMarker(seat);
    internal bool HasShiCaoShuangCapability { get => _resolve().HasShiCaoShuangCapability; }
    internal global::System.Collections.Generic.IReadOnlyList<int> ShiCaoShuangMarkedIds { get => _resolve().ShiCaoShuangMarkedIds; }
    internal int? ShiCaoShuangOwnerSeat { get => _resolve().ShiCaoShuangOwnerSeat; }
}
