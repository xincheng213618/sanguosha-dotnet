#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangRenMobileShiEntries
{
    internal void AnswerShiZr(PromptChoice selected) => _resolve().AnswerShiZr(selected);
    internal void ArmShiZrFuan(ProgramSkillFrame f, string drawReason) => _resolve().ArmShiZrFuan(f, drawReason);
    internal SkillProgramStepOutcome BeginShiZrFuan(ProgramSkillFrame input) => _resolve().BeginShiZrFuan(input);
    internal SkillProgramStepOutcome BeginShiZrFuanStrike(ProgramSkillFrame input) => _resolve().BeginShiZrFuanStrike(input);
    internal bool CanIssueShiZrFuanStrike(int actor, int target) => _resolve().CanIssueShiZrFuanStrike(actor, target);
    internal PromptChoice ChooseShiZrFuanAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiZrFuanAi(decision, f);
    internal PromptChoice ChooseShiZrStrikeAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiZrStrikeAi(decision, f);
    internal void ContinueShiZr(long id) => _resolve().ContinueShiZr(id);
    internal bool DrainShiZr(ProgramSkillFrame f) => _resolve().DrainShiZr(f);
    internal void ExpireShiZrFuanSeals(IGameEvent payload) => _resolve().ExpireShiZrFuanSeals(payload);
    internal void FinishShiZrOperation(ProgramSkillFrame f) => _resolve().FinishShiZrOperation(f);
    internal bool IsShiZrFuanStrikeUse(long useFrameId) => _resolve().IsShiZrFuanStrikeUse(useFrameId);
    internal void IssueShiZrFuanStrike(ProgramSkillFrame f, int targetSeat) => _resolve().IssueShiZrFuanStrike(f, targetSeat);
    internal PromptChoice MakeShiZrChoice(ProgramSkillFrame f, string branch, string? value, string label) => _resolve().MakeShiZrChoice(f, branch, value, label);
    internal void ObserveShiZrFuanDiscards(IGameEvent payload) => _resolve().ObserveShiZrFuanDiscards(payload);
    internal void PublishShiZr(ProgramSkillFrame f) => _resolve().PublishShiZr(f);
    internal void RecordShiZrFuanSuit(ProgramSkillFrame f, Suit suit) => _resolve().RecordShiZrFuanSuit(f, suit);
    internal SkillProgramStepOutcome ResolveShiZrYinXian(ProgramSkillFrame supplied) => _resolve().ResolveShiZrYinXian(supplied);
    internal bool ResumeShiZr(long id) => _resolve().ResumeShiZr(id);
    internal bool ReturnShiZrMovement(ProgramSkillFrame f) => _resolve().ReturnShiZrMovement(f);
    internal void SealShiZrFuanTarget(ProgramSkillFrame f) => _resolve().SealShiZrFuanTarget(f);
    internal void SettleShiZrFuanStrike(ProgramSkillFrame f) => _resolve().SettleShiZrFuanStrike(f);
    internal IReadOnlyList<PromptChoice> ShiZrChoices(ProgramSkillFrame f) => _resolve().ShiZrChoices(f);
    internal bool ShiZrFuanBaseZeroed(CharacterState player) => _resolve().ShiZrFuanBaseZeroed(player);
    internal bool ShiZrFuanGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().ShiZrFuanGrantQualified(owner, grant);
    internal GameEngine.ShiZrFuanRecordInfo? ShiZrFuanRecord(int seat) => _resolve().ShiZrFuanRecord(seat);
    internal IReadOnlyList<Suit> ShiZrFuanRecordableSuits(int seat) => _resolve().ShiZrFuanRecordableSuits(seat);
    internal bool ShiZrFuanSealActive(int target) => _resolve().ShiZrFuanSealActive(target);
    internal bool ShiZrFuanSlashDistance(ProgramSlashDistanceQuery query) => _resolve().ShiZrFuanSlashDistance(query);
    internal bool ShiZrFuanStrikeQualified(int seat) => _resolve().ShiZrFuanStrikeQualified(seat);
    internal bool ShiZrFuanUseDistance(ProgramCardDistanceQuery query) => _resolve().ShiZrFuanUseDistance(query);
    internal bool ShiZrFuanZeroed(int seat) => _resolve().ShiZrFuanZeroed(seat);
    internal IReadOnlyList<PromptChoice> ShiZrStrikeFollowUps(ProgramSkillFrame f) => _resolve().ShiZrStrikeFollowUps(f);
    internal int ShiZrYinXianBonus(int seat) => _resolve().ShiZrYinXianBonus(seat);
    internal int ShiZrYinXianDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ShiZrYinXianDamageBonus(attack);
    internal bool ShiZrYinXianFarthest(int user, int target) => _resolve().ShiZrYinXianFarthest(user, target);
    internal int ShiZrYinXianRangeBonus(CharacterState player) => _resolve().ShiZrYinXianRangeBonus(player);
    internal bool HasShiZrCapability { get => _resolve().HasShiZrCapability; }
}
