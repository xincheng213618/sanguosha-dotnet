#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangRenMobileShiEntries
{
    internal void AnswerShiZr(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiZr(selected);
    internal void ArmShiZrFuan(global::CardGame.Core.ProgramSkillFrame f, string drawReason) => _resolve().ArmShiZrFuan(f, drawReason);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiZrFuan(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiZrFuan(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiZrFuanStrike(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiZrFuanStrike(input);
    internal bool CanIssueShiZrFuanStrike(int actor, int target) => _resolve().CanIssueShiZrFuanStrike(actor, target);
    internal global::CardGame.Core.PromptChoice ChooseShiZrFuanAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiZrFuanAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiZrStrikeAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiZrStrikeAi(decision, f);
    internal void ContinueShiZr(long id) => _resolve().ContinueShiZr(id);
    internal bool DrainShiZr(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiZr(f);
    internal void ExpireShiZrFuanSeals(global::CardGame.Core.IGameEvent payload) => _resolve().ExpireShiZrFuanSeals(payload);
    internal void FinishShiZrOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiZrOperation(f);
    internal bool HasExactShiZrFuanStrikeTarget(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HasExactShiZrFuanStrikeTarget(frame);
    internal bool IsShiZrFuanStrikeUse(long useFrameId) => _resolve().IsShiZrFuanStrikeUse(useFrameId);
    internal void IssueShiZrFuanStrike(global::CardGame.Core.ProgramSkillFrame f, int targetSeat) => _resolve().IssueShiZrFuanStrike(f, targetSeat);
    internal global::CardGame.Core.PromptChoice MakeShiZrChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string? value, string label) => _resolve().MakeShiZrChoice(f, branch, value, label);
    internal void ObserveShiZrFuanDiscards(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveShiZrFuanDiscards(payload);
    internal void PublishShiZr(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiZr(f);
    internal void RecordShiZrFuanSuit(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.Suit suit) => _resolve().RecordShiZrFuanSuit(f, suit);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShiZrYinXian(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveShiZrYinXian(supplied);
    internal bool ResumeShiZr(long id) => _resolve().ResumeShiZr(id);
    internal bool ReturnShiZrMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiZrMovement(f);
    internal void SealShiZrFuanTarget(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SealShiZrFuanTarget(f);
    internal void SettleShiZrFuanStrike(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleShiZrFuanStrike(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiZrChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiZrChoices(f);
    internal bool ShiZrFuanBaseZeroed(global::CardGame.Core.CharacterState player) => _resolve().ShiZrFuanBaseZeroed(player);
    internal bool ShiZrFuanGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().ShiZrFuanGrantQualified(owner, grant);
    internal global::CardGame.Core.GameEngine.ShiZrFuanRecordInfo? ShiZrFuanRecord(int seat) => _resolve().ShiZrFuanRecord(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit> ShiZrFuanRecordableSuits(int seat) => _resolve().ShiZrFuanRecordableSuits(seat);
    internal bool ShiZrFuanSealActive(int target) => _resolve().ShiZrFuanSealActive(target);
    internal bool ShiZrFuanSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery query) => _resolve().ShiZrFuanSlashDistance(query);
    internal bool ShiZrFuanStrikeQualified(int seat) => _resolve().ShiZrFuanStrikeQualified(seat);
    internal bool ShiZrFuanUseDistance(global::CardGame.Core.ProgramCardDistanceQuery query) => _resolve().ShiZrFuanUseDistance(query);
    internal bool ShiZrFuanZeroed(int seat) => _resolve().ShiZrFuanZeroed(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiZrStrikeFollowUps(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiZrStrikeFollowUps(f);
    internal int ShiZrYinXianBonus(int seat) => _resolve().ShiZrYinXianBonus(seat);
    internal int ShiZrYinXianDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ShiZrYinXianDamageBonus(attack);
    internal bool ShiZrYinXianFarthest(int user, int target) => _resolve().ShiZrYinXianFarthest(user, target);
    internal int ShiZrYinXianRangeBonus(global::CardGame.Core.CharacterState player) => _resolve().ShiZrYinXianRangeBonus(player);
    internal bool HasShiZrCapability { get => _resolve().HasShiZrCapability; }
}
