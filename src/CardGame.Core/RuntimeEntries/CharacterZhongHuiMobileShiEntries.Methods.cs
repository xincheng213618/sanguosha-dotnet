#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhongHuiMobileShiEntries
{
    internal void AnswerShiZhHui(PromptChoice selected) => _resolve().AnswerShiZhHui(selected);
    internal void ApplyShiZhHuiFactionChange(ProgramSkillFrame frame, string faction) => _resolve().ApplyShiZhHuiFactionChange(frame, faction);
    internal SkillProgramStepOutcome BeginShiZhHui(ProgramSkillFrame input) => _resolve().BeginShiZhHui(input);
    internal bool CanOfferShiZhHui(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShiZhHui(candidate, trigger, context);
    internal bool CanStartShiZhHuiSizi(CharacterState owner, string skill) => _resolve().CanStartShiZhHuiSizi(owner, skill);
    internal int ChangeShiZhHuiCharge(long frameId, int owner, int amount, string reason, scoped out int excess) => _resolve().ChangeShiZhHuiCharge(frameId, owner, amount, reason, out excess);
    internal PromptChoice ChooseShiZhHuiAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiZhHuiAi(decision, f);
    internal bool ContinuePaidShiZhHui(ProgramSkillFrame f) => _resolve().ContinuePaidShiZhHui(f);
    internal void ContinueShiZhHui(long id) => _resolve().ContinueShiZhHui(id);
    internal void ContinueShiZhHuiEnding(ProgramSkillFrame f) => _resolve().ContinueShiZhHuiEnding(f);
    internal void ContinueShiZhHuiEndingLosses(ProgramSkillFrame f, ShiZhHuiSiziActivatedEvent activation) => _resolve().ContinueShiZhHuiEndingLosses(f, activation);
    internal void ContinueShiZhHuiXiezhi(ProgramSkillFrame f) => _resolve().ContinueShiZhHuiXiezhi(f);
    internal void ContinueShiZhHuiYunan(ProgramSkillFrame f) => _resolve().ContinueShiZhHuiYunan(f);
    internal bool DistanceCardUseExemption67700ShiZhHuiKechang(ProgramCardDistanceQuery query) => _resolve().DistanceCardUseExemption67700ShiZhHuiKechang(query);
    internal bool DrainShiZhHui(ProgramSkillFrame f) => _resolve().DrainShiZhHui(f);
    internal void FinishShiZhHuiOperation(ProgramSkillFrame f) => _resolve().FinishShiZhHuiOperation(f);
    internal bool HasShiZhHuiKechang(int seat) => _resolve().HasShiZhHuiKechang(seat);
    internal void ObserveShiZhHuiTurnFacts(IGameEvent payload) => _resolve().ObserveShiZhHuiTurnFacts(payload);
    internal void PublishShiZhHui(ProgramSkillFrame f) => _resolve().PublishShiZhHui(f);
    internal bool ResumeShiZhHui(long id) => _resolve().ResumeShiZhHui(id);
    internal bool ReturnShiZhHuiMovement(ProgramSkillFrame f) => _resolve().ReturnShiZhHuiMovement(f);
    internal int ShiZhHuiCharge(int owner) => _resolve().ShiZhHuiCharge(owner);
    internal IReadOnlyList<int> ShiZhHuiEndingVictims(int turnNumber, int ownerHp, int consumed) => _resolve().ShiZhHuiEndingVictims(turnNumber, ownerHp, consumed);
    internal bool ShiZhHuiKeChangCannotRespond(int sourceSeat, int responderSeat, CardKind incomingKind, IReadOnlyList<int> targetSeats) => _resolve().ShiZhHuiKeChangCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats);
    internal int ShiZhHuiKechangLevel(int seat) => _resolve().ShiZhHuiKechangLevel(seat);
    internal int ShiZhHuiKechangLevelForResponses(int sourceSeat) => _resolve().ShiZhHuiKechangLevelForResponses(sourceSeat);
    internal int ShiZhHuiOverflowCount(int seat) => _resolve().ShiZhHuiOverflowCount(seat);
    internal IReadOnlyList<PromptChoice> ShiZhHuiSiziChoices(ProgramSkillFrame f) => _resolve().ShiZhHuiSiziChoices(f);
    internal int ShiZhHuiSiziDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ShiZhHuiSiziDamageBonus(attack);
    internal ShiZhHuiReceipt StartShiZhHuiSizi(ProgramSkillFrame f) => _resolve().StartShiZhHuiSizi(f);
    internal ShiZhHuiReceipt StartShiZhHuiSiziEnding(ProgramSkillFrame f) => _resolve().StartShiZhHuiSiziEnding(f);
    internal ShiZhHuiReceipt StartShiZhHuiXiezhi(ProgramSkillFrame f) => _resolve().StartShiZhHuiXiezhi(f);
    internal ShiZhHuiReceipt StartShiZhHuiYunan(ProgramSkillFrame f) => _resolve().StartShiZhHuiYunan(f);
    internal bool HasShiZhHuiCapability { get => _resolve().HasShiZhHuiCapability; }
    internal ShiZhHuiSiziActivatedEvent? ShiZhHuiActiveSizi { get => _resolve().ShiZhHuiActiveSizi; }
    internal bool ShiZhHuiRoundHasDeath { get => _resolve().ShiZhHuiRoundHasDeath; }
    internal bool ShiZhHuiSiziActive { get => _resolve().ShiZhHuiSiziActive; }
}
