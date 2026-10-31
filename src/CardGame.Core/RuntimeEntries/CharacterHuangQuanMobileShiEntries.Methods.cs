#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangQuanMobileShiEntries
{
    internal void AnswerShiQuan(PromptChoice selected) => _resolve().AnswerShiQuan(selected);
    internal SkillProgramStepOutcome BeginShiQuanCuixian(ProgramSkillFrame input) => _resolve().BeginShiQuanCuixian(input);
    internal SkillProgramStepOutcome BeginShiQuanGushi(ProgramSkillFrame input) => _resolve().BeginShiQuanGushi(input);
    internal SkillProgramStepOutcome BeginShiQuanTurn(ProgramSkillFrame f) => _resolve().BeginShiQuanTurn(f);
    internal bool CanIssueShiQuanDuel(int ownerSeat, int holderSeat) => _resolve().CanIssueShiQuanDuel(ownerSeat, holderSeat);
    internal bool CanOfferShiQuanCuixian(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShiQuanCuixian(candidate, trigger, context);
    internal bool CanOfferShiQuanGushi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShiQuanGushi(candidate, trigger, context);
    internal PromptChoice ChooseShiQuanCuixianAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiQuanCuixianAi(decision, f);
    internal PromptChoice ChooseShiQuanGushiAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiQuanGushiAi(decision, f);
    internal void ContinueShiQuan(ProgramSkillFrame f) => _resolve().ContinueShiQuan(f);
    internal bool ContinueShiQuanAfterLoss(ProgramSkillFrame f) => _resolve().ContinueShiQuanAfterLoss(f);
    internal void ContinueShiQuanCuixianChoice(ProgramSkillFrame f, string branch) => _resolve().ContinueShiQuanCuixianChoice(f, branch);
    internal void ContinueShiQuanGushiReplace(ProgramSkillFrame f) => _resolve().ContinueShiQuanGushiReplace(f);
    internal IReadOnlyList<int> DiscardShiQuanHolderDeathPiles(DeathFrame death) => _resolve().DiscardShiQuanHolderDeathPiles(death);
    internal bool DrainShiQuan(ProgramSkillFrame f) => _resolve().DrainShiQuan(f);
    internal void FinishShiQuanOperation(ProgramSkillFrame f) => _resolve().FinishShiQuanOperation(f);
    internal bool IsShiQuanDuelUse(long id) => _resolve().IsShiQuanDuelUse(id);
    internal void IssueShiQuanDuel(ProgramSkillFrame f, string branch) => _resolve().IssueShiQuanDuel(f, branch);
    internal void MoveShiQuanCards(IReadOnlyList<int> cardIds, CardLocation from, CardLocation to, string reason) => _resolve().MoveShiQuanCards(cardIds, from, to, reason);
    internal void PlaceShiQuanCards(ProgramSkillFrame f, int theirsId) => _resolve().PlaceShiQuanCards(f, theirsId);
    internal void PublishShiQuan(ProgramSkillFrame f) => _resolve().PublishShiQuan(f);
    internal void ResolveShiHqGushiGain(ProgramSkillFrame f) => _resolve().ResolveShiHqGushiGain(f);
    internal void ResolveShiHqGushiReplace(ProgramSkillFrame f) => _resolve().ResolveShiHqGushiReplace(f);
    internal SkillProgramStepOutcome ResolveShiQuanClaim(ProgramSkillFrame f) => _resolve().ResolveShiQuanClaim(f);
    internal bool ResumeShiQuan(long id) => _resolve().ResumeShiQuan(id);
    internal void ReturnShiQuanDuel(CardUseFrame use) => _resolve().ReturnShiQuanDuel(use);
    internal bool ReturnShiQuanDuelAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnShiQuanDuelAttack(completion);
    internal bool ReturnShiQuanMovement(ProgramSkillFrame f) => _resolve().ReturnShiQuanMovement(f);
    internal IReadOnlyList<PromptChoice> ShiQuanChoices(ProgramSkillFrame f) => _resolve().ShiQuanChoices(f);
    internal bool ShiQuanClaimAvailable(int ownerSeat, ProgramSkillWindowContext? context) => _resolve().ShiQuanClaimAvailable(ownerSeat, context);
    internal bool ShiQuanGainsAvailable(int ownerSeat, ProgramSkillWindowContext? context) => _resolve().ShiQuanGainsAvailable(ownerSeat, context);
    internal (Card Card, CardLocation From)? ShiQuanGushiGainCandidate() => _resolve().ShiQuanGushiGainCandidate();
    internal int[] ShiQuanGushiReplaceCandidates(int ownerSeat) => _resolve().ShiQuanGushiReplaceCandidates(ownerSeat);
    internal bool ShiQuanGushiSlashDebitExemption(long useId, int actorSeat) => _resolve().ShiQuanGushiSlashDebitExemption(useId, actorSeat);
    internal bool ShiQuanGushiSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => _resolve().ShiQuanGushiSlashQuotaBeforeLimit(query);
    internal bool ShiQuanGushiUnlimitedCard(CharacterState actor, CardKind kind, IReadOnlyList<int> ids) => _resolve().ShiQuanGushiUnlimitedCard(actor, kind, ids);
    internal CardZoneKind? ShiQuanLostZone(int ownerSeat, ProgramSkillWindowContext? context) => _resolve().ShiQuanLostZone(ownerSeat, context);
    internal IReadOnlyList<int> ShiQuanOrderedPileIds(int holderSeat) => _resolve().ShiQuanOrderedPileIds(holderSeat);
    internal ShiQuanCuixianPlacedEvent? ShiQuanPendingPlacement(long useId) => _resolve().ShiQuanPendingPlacement(useId);
    internal bool ShiQuanTurnAvailable(int ownerSeat, ProgramSkillWindowContext? context) => _resolve().ShiQuanTurnAvailable(ownerSeat, context);
    internal bool ShiQuanUseDamaged(long useId) => _resolve().ShiQuanUseDamaged(useId);
    internal bool HasShiQuan { get => _resolve().HasShiQuan; }
}
