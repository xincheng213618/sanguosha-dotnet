#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenJiaoMobileShiEntries
{
    internal void AnswerChenJiao(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerChenJiao(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginChenJiao(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginChenJiao(input);
    internal bool CanIssueChenJiaoSlash(int actor, int target) => _resolve().CanIssueChenJiaoSlash(actor, target);
    internal bool CanStartChenJiaoQingYan(global::CardGame.Core.CharacterState owner, string skill) => _resolve().CanStartChenJiaoQingYan(owner, skill);
    internal int ChenJiaoCeDuanDamageCount(int sourceSeat, int targetSeat) => _resolve().ChenJiaoCeDuanDamageCount(sourceSeat, targetSeat);
    internal global::System.Collections.Generic.IReadOnlyList<int> ChenJiaoCeDuanShownIdsThisActivation(long frameId) => _resolve().ChenJiaoCeDuanShownIdsThisActivation(frameId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ChenJiaoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChenJiaoChoices(f);
    internal bool ChenJiaoHasSkill(int seat, string skillId) => _resolve().ChenJiaoHasSkill(seat, skillId);
    internal bool ChenJiaoQingYanLocked(int owner) => _resolve().ChenJiaoQingYanLocked(owner);
    internal global::System.Collections.Generic.IReadOnlyList<int> ChenJiaoQingYanPendingIds(int owner) => _resolve().ChenJiaoQingYanPendingIds(owner);
    internal int ChenJiaoQingYanUsesThisRound(int owner) => _resolve().ChenJiaoQingYanUsesThisRound(owner);
    internal global::CardGame.Core.PromptChoice ChooseChenJiaoCeDuanAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseChenJiaoCeDuanAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseChenJiaoQingYanAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseChenJiaoQingYanAi(decision, f);
    internal void CollectChenJiaoCeDuanMaterials(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CollectChenJiaoCeDuanMaterials(f);
    internal void ContinueChenJiao(long id) => _resolve().ContinueChenJiao(id);
    internal void ContinueChenJiaoCeDuanColor(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueChenJiaoCeDuanColor(f);
    internal bool DrainChenJiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainChenJiao(f);
    internal void FinishChenJiaoOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishChenJiaoOperation(f);
    internal void IssueChenJiaoCeDuanSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueChenJiaoCeDuanSlash(f);
    internal global::CardGame.Core.PromptChoice MakeChenJiaoChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string value, string label) => _resolve().MakeChenJiaoChoice(f, branch, value, label);
    internal void ObserveChenJiaoFacts(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveChenJiaoFacts(payload);
    internal bool PrepareChenJiaoQingYanUse(global::CardGame.Core.CardUseFrame use) => _resolve().PrepareChenJiaoQingYanUse(use);
    internal void PublishChenJiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishChenJiao(f);
    internal void ResolveChenJiaoCeDuanColor(global::CardGame.Core.ProgramSkillFrame f, string value) => _resolve().ResolveChenJiaoCeDuanColor(f, value);
    internal void ResolveChenJiaoCeDuanShow(global::CardGame.Core.ProgramSkillFrame f, int holder, int cardId) => _resolve().ResolveChenJiaoCeDuanShow(f, holder, cardId);
    internal void ResolveChenJiaoQingYanKind(global::CardGame.Core.ProgramSkillFrame f, string value) => _resolve().ResolveChenJiaoQingYanKind(f, value);
    internal void ResolveChenJiaoQingYanShow(global::CardGame.Core.ProgramSkillFrame f, int cardId) => _resolve().ResolveChenJiaoQingYanShow(f, cardId);
    internal bool ResumeChenJiao(long id) => _resolve().ResumeChenJiao(id);
    internal bool ReturnChenJiaoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnChenJiaoMovement(f);
    internal void ReturnChenJiaoQingYanUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnChenJiaoQingYanUse(use);
    internal global::CardGame.Core.SkillProgramStepOutcome SettleChenJiaoCeDuan(global::CardGame.Core.ProgramSkillFrame input) => _resolve().SettleChenJiaoCeDuan(input);
    internal global::CardGame.Core.ChenJiaoReceipt StartChenJiaoCeDuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartChenJiaoCeDuan(f);
    internal global::CardGame.Core.ChenJiaoReceipt StartChenJiaoQingYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartChenJiaoQingYan(f);
    internal void SyncChenJiaoMarker(int seat) => _resolve().SyncChenJiaoMarker(seat);
    internal bool HasChenJiaoCapability { get => _resolve().HasChenJiaoCapability; }
}
