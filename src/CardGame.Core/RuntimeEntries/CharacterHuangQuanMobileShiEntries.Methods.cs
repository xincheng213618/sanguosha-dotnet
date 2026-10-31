#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangQuanMobileShiEntries
{
    internal void AnswerShiQuan(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiQuan(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiQuanCuixian(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiQuanCuixian(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiQuanGushi(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiQuanGushi(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiQuanTurn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginShiQuanTurn(f);
    internal bool CanIssueShiQuanDuel(int ownerSeat, int holderSeat) => _resolve().CanIssueShiQuanDuel(ownerSeat, holderSeat);
    internal bool CanOfferShiQuanCuixian(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShiQuanCuixian(candidate, trigger, context);
    internal bool CanOfferShiQuanGushi(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShiQuanGushi(candidate, trigger, context);
    internal global::CardGame.Core.PromptChoice ChooseShiQuanCuixianAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiQuanCuixianAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiQuanGushiAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiQuanGushiAi(decision, f);
    internal void ContinueShiQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiQuan(f);
    internal bool ContinueShiQuanAfterLoss(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiQuanAfterLoss(f);
    internal void ContinueShiQuanCuixianChoice(global::CardGame.Core.ProgramSkillFrame f, string branch) => _resolve().ContinueShiQuanCuixianChoice(f, branch);
    internal void ContinueShiQuanGushiReplace(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiQuanGushiReplace(f);
    internal global::System.Collections.Generic.IReadOnlyList<int> DiscardShiQuanHolderDeathPiles(global::CardGame.Core.DeathFrame death) => _resolve().DiscardShiQuanHolderDeathPiles(death);
    internal bool DrainShiQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiQuan(f);
    internal void FinishShiQuanOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiQuanOperation(f);
    internal bool IsShiQuanDuelUse(long id) => _resolve().IsShiQuanDuelUse(id);
    internal void IssueShiQuanDuel(global::CardGame.Core.ProgramSkillFrame f, string branch) => _resolve().IssueShiQuanDuel(f, branch);
    internal void MoveShiQuanCards(global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, string reason) => _resolve().MoveShiQuanCards(cardIds, from, to, reason);
    internal void PlaceShiQuanCards(global::CardGame.Core.ProgramSkillFrame f, int theirsId) => _resolve().PlaceShiQuanCards(f, theirsId);
    internal void PublishShiQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiQuan(f);
    internal void ResolveShiHqGushiGain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ResolveShiHqGushiGain(f);
    internal void ResolveShiHqGushiReplace(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ResolveShiHqGushiReplace(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShiQuanClaim(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ResolveShiQuanClaim(f);
    internal bool ResumeShiQuan(long id) => _resolve().ResumeShiQuan(id);
    internal void ReturnShiQuanDuel(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnShiQuanDuel(use);
    internal bool ReturnShiQuanDuelAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnShiQuanDuelAttack(completion);
    internal bool ReturnShiQuanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiQuanMovement(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiQuanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiQuanChoices(f);
    internal bool ShiQuanClaimAvailable(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().ShiQuanClaimAvailable(ownerSeat, context);
    internal bool ShiQuanGainsAvailable(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().ShiQuanGainsAvailable(ownerSeat, context);
    internal (global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation From)? ShiQuanGushiGainCandidate() => _resolve().ShiQuanGushiGainCandidate();
    internal int[] ShiQuanGushiReplaceCandidates(int ownerSeat) => _resolve().ShiQuanGushiReplaceCandidates(ownerSeat);
    internal bool ShiQuanGushiSlashDebitExemption(long useId, int actorSeat) => _resolve().ShiQuanGushiSlashDebitExemption(useId, actorSeat);
    internal bool ShiQuanGushiSlashQuotaBeforeLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().ShiQuanGushiSlashQuotaBeforeLimit(query);
    internal bool ShiQuanGushiUnlimitedCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().ShiQuanGushiUnlimitedCard(actor, kind, ids);
    internal global::CardGame.Core.CardZoneKind? ShiQuanLostZone(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().ShiQuanLostZone(ownerSeat, context);
    internal global::System.Collections.Generic.IReadOnlyList<int> ShiQuanOrderedPileIds(int holderSeat) => _resolve().ShiQuanOrderedPileIds(holderSeat);
    internal global::CardGame.Core.ShiQuanCuixianPlacedEvent? ShiQuanPendingPlacement(long useId) => _resolve().ShiQuanPendingPlacement(useId);
    internal bool ShiQuanTurnAvailable(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().ShiQuanTurnAvailable(ownerSeat, context);
    internal bool ShiQuanUseDamaged(long useId) => _resolve().ShiQuanUseDamaged(useId);
    internal bool HasShiQuan { get => _resolve().HasShiQuan; }
}
