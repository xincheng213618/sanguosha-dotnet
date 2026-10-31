#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhongHuiMobileShiEntries
{
    internal void AnswerShiZhHui(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiZhHui(selected);
    internal void ApplyShiZhHuiFactionChange(global::CardGame.Core.ProgramSkillFrame frame, string faction) => _resolve().ApplyShiZhHuiFactionChange(frame, faction);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiZhHui(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiZhHui(input);
    internal bool CanOfferShiZhHui(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShiZhHui(candidate, trigger, context);
    internal bool CanStartShiZhHuiSizi(global::CardGame.Core.CharacterState owner, string skill) => _resolve().CanStartShiZhHuiSizi(owner, skill);
    internal int ChangeShiZhHuiCharge(long frameId, int owner, int amount, string reason, scoped out int excess) => _resolve().ChangeShiZhHuiCharge(frameId, owner, amount, reason, out excess);
    internal global::CardGame.Core.PromptChoice ChooseShiZhHuiAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiZhHuiAi(decision, f);
    internal bool ContinuePaidShiZhHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidShiZhHui(f);
    internal void ContinueShiZhHui(long id) => _resolve().ContinueShiZhHui(id);
    internal void ContinueShiZhHuiEnding(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiZhHuiEnding(f);
    internal void ContinueShiZhHuiEndingLosses(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ShiZhHuiSiziActivatedEvent activation) => _resolve().ContinueShiZhHuiEndingLosses(f, activation);
    internal void ContinueShiZhHuiXiezhi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiZhHuiXiezhi(f);
    internal void ContinueShiZhHuiYunan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiZhHuiYunan(f);
    internal bool DistanceCardUseExemption67700ShiZhHuiKechang(global::CardGame.Core.ProgramCardDistanceQuery query) => _resolve().DistanceCardUseExemption67700ShiZhHuiKechang(query);
    internal bool DrainShiZhHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiZhHui(f);
    internal void FinishShiZhHuiOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiZhHuiOperation(f);
    internal bool HasShiZhHuiKechang(int seat) => _resolve().HasShiZhHuiKechang(seat);
    internal void ObserveShiZhHuiTurnFacts(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveShiZhHuiTurnFacts(payload);
    internal void PublishShiZhHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiZhHui(f);
    internal bool ResumeShiZhHui(long id) => _resolve().ResumeShiZhHui(id);
    internal bool ReturnShiZhHuiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiZhHuiMovement(f);
    internal int ShiZhHuiCharge(int owner) => _resolve().ShiZhHuiCharge(owner);
    internal global::System.Collections.Generic.IReadOnlyList<int> ShiZhHuiEndingVictims(int turnNumber, int ownerHp, int consumed) => _resolve().ShiZhHuiEndingVictims(turnNumber, ownerHp, consumed);
    internal bool ShiZhHuiKeChangCannotRespond(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => _resolve().ShiZhHuiKeChangCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats);
    internal int ShiZhHuiKechangLevel(int seat) => _resolve().ShiZhHuiKechangLevel(seat);
    internal int ShiZhHuiKechangLevelForResponses(int sourceSeat) => _resolve().ShiZhHuiKechangLevelForResponses(sourceSeat);
    internal int ShiZhHuiOverflowCount(int seat) => _resolve().ShiZhHuiOverflowCount(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiZhHuiSiziChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiZhHuiSiziChoices(f);
    internal int ShiZhHuiSiziDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ShiZhHuiSiziDamageBonus(attack);
    internal global::CardGame.Core.ShiZhHuiReceipt StartShiZhHuiSizi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartShiZhHuiSizi(f);
    internal global::CardGame.Core.ShiZhHuiReceipt StartShiZhHuiSiziEnding(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartShiZhHuiSiziEnding(f);
    internal global::CardGame.Core.ShiZhHuiReceipt StartShiZhHuiXiezhi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartShiZhHuiXiezhi(f);
    internal global::CardGame.Core.ShiZhHuiReceipt StartShiZhHuiYunan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartShiZhHuiYunan(f);
    internal bool HasShiZhHuiCapability { get => _resolve().HasShiZhHuiCapability; }
    internal global::CardGame.Core.ShiZhHuiSiziActivatedEvent? ShiZhHuiActiveSizi { get => _resolve().ShiZhHuiActiveSizi; }
    internal bool ShiZhHuiRoundHasDeath { get => _resolve().ShiZhHuiRoundHasDeath; }
    internal bool ShiZhHuiSiziActive { get => _resolve().ShiZhHuiSiziActive; }
}
