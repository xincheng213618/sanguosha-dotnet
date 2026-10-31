#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenJiaoMobileShiEntries
{
    internal void AnswerChenJiao(PromptChoice selected) => _resolve().AnswerChenJiao(selected);
    internal SkillProgramStepOutcome BeginChenJiao(ProgramSkillFrame input) => _resolve().BeginChenJiao(input);
    internal bool CanIssueChenJiaoSlash(int actor, int target) => _resolve().CanIssueChenJiaoSlash(actor, target);
    internal bool CanStartChenJiaoQingYan(CharacterState owner, string skill) => _resolve().CanStartChenJiaoQingYan(owner, skill);
    internal int ChenJiaoCeDuanDamageCount(int sourceSeat, int targetSeat) => _resolve().ChenJiaoCeDuanDamageCount(sourceSeat, targetSeat);
    internal IReadOnlyList<int> ChenJiaoCeDuanShownIdsThisActivation(long frameId) => _resolve().ChenJiaoCeDuanShownIdsThisActivation(frameId);
    internal IReadOnlyList<PromptChoice> ChenJiaoChoices(ProgramSkillFrame f) => _resolve().ChenJiaoChoices(f);
    internal bool ChenJiaoHasSkill(int seat, string skillId) => _resolve().ChenJiaoHasSkill(seat, skillId);
    internal bool ChenJiaoQingYanLocked(int owner) => _resolve().ChenJiaoQingYanLocked(owner);
    internal IReadOnlyList<int> ChenJiaoQingYanPendingIds(int owner) => _resolve().ChenJiaoQingYanPendingIds(owner);
    internal int ChenJiaoQingYanUsesThisRound(int owner) => _resolve().ChenJiaoQingYanUsesThisRound(owner);
    internal PromptChoice ChooseChenJiaoCeDuanAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseChenJiaoCeDuanAi(decision, f);
    internal PromptChoice ChooseChenJiaoQingYanAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseChenJiaoQingYanAi(decision, f);
    internal void CollectChenJiaoCeDuanMaterials(ProgramSkillFrame f) => _resolve().CollectChenJiaoCeDuanMaterials(f);
    internal void ContinueChenJiao(long id) => _resolve().ContinueChenJiao(id);
    internal void ContinueChenJiaoCeDuanColor(ProgramSkillFrame f) => _resolve().ContinueChenJiaoCeDuanColor(f);
    internal bool DrainChenJiao(ProgramSkillFrame f) => _resolve().DrainChenJiao(f);
    internal void FinishChenJiaoOperation(ProgramSkillFrame f) => _resolve().FinishChenJiaoOperation(f);
    internal void IssueChenJiaoCeDuanSlash(ProgramSkillFrame f) => _resolve().IssueChenJiaoCeDuanSlash(f);
    internal PromptChoice MakeChenJiaoChoice(ProgramSkillFrame f, string branch, string value, string label) => _resolve().MakeChenJiaoChoice(f, branch, value, label);
    internal void ObserveChenJiaoFacts(IGameEvent payload) => _resolve().ObserveChenJiaoFacts(payload);
    internal bool PrepareChenJiaoQingYanUse(CardUseFrame use) => _resolve().PrepareChenJiaoQingYanUse(use);
    internal void PublishChenJiao(ProgramSkillFrame f) => _resolve().PublishChenJiao(f);
    internal void ResolveChenJiaoCeDuanColor(ProgramSkillFrame f, string value) => _resolve().ResolveChenJiaoCeDuanColor(f, value);
    internal void ResolveChenJiaoCeDuanShow(ProgramSkillFrame f, int holder, int cardId) => _resolve().ResolveChenJiaoCeDuanShow(f, holder, cardId);
    internal void ResolveChenJiaoQingYanKind(ProgramSkillFrame f, string value) => _resolve().ResolveChenJiaoQingYanKind(f, value);
    internal void ResolveChenJiaoQingYanShow(ProgramSkillFrame f, int cardId) => _resolve().ResolveChenJiaoQingYanShow(f, cardId);
    internal bool ResumeChenJiao(long id) => _resolve().ResumeChenJiao(id);
    internal bool ReturnChenJiaoMovement(ProgramSkillFrame f) => _resolve().ReturnChenJiaoMovement(f);
    internal void ReturnChenJiaoQingYanUse(CardUseFrame use) => _resolve().ReturnChenJiaoQingYanUse(use);
    internal SkillProgramStepOutcome SettleChenJiaoCeDuan(ProgramSkillFrame input) => _resolve().SettleChenJiaoCeDuan(input);
    internal ChenJiaoReceipt StartChenJiaoCeDuan(ProgramSkillFrame f) => _resolve().StartChenJiaoCeDuan(f);
    internal ChenJiaoReceipt StartChenJiaoQingYan(ProgramSkillFrame f) => _resolve().StartChenJiaoQingYan(f);
    internal void SyncChenJiaoMarker(int seat) => _resolve().SyncChenJiaoMarker(seat);
    internal bool HasChenJiaoCapability { get => _resolve().HasChenJiaoCapability; }
}
