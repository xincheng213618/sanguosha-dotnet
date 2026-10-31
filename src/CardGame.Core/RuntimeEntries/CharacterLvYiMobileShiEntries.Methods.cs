#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLvYiMobileShiEntries
{
    internal void AnswerShiLy(PromptChoice selected) => _resolve().AnswerShiLy(selected);
    internal void ApplyShiLyHuilvGive(ProgramSkillFrame f, int targetSeat) => _resolve().ApplyShiLyHuilvGive(f, targetSeat);
    internal void ApplyShiLyHuilvGrant(ProgramSkillFrame f, bool red) => _resolve().ApplyShiLyHuilvGrant(f, red);
    internal void ApplyShiLySongyanUse(ProgramSkillFrame f) => _resolve().ApplyShiLySongyanUse(f);
    internal SkillProgramStepOutcome BeginShiLyHuilv(ProgramSkillFrame input) => _resolve().BeginShiLyHuilv(input);
    internal SkillProgramStepOutcome BeginShiLyHuilvSettle(ProgramSkillFrame input) => _resolve().BeginShiLyHuilvSettle(input);
    internal SkillProgramStepOutcome BeginShiLySongyan(ProgramSkillFrame input) => _resolve().BeginShiLySongyan(input);
    internal bool CanOfferShiLyHuilv(ProgramTriggerLegalityContext c) => _resolve().CanOfferShiLyHuilv(c);
    internal bool CanOfferShiLyHuilvSettle(ProgramTriggerLegalityContext c) => _resolve().CanOfferShiLyHuilvSettle(c);
    internal bool CanOfferShiLySongyan(ProgramTriggerLegalityContext c) => _resolve().CanOfferShiLySongyan(c);
    internal PromptChoice ChooseShiLyHuilvAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiLyHuilvAi(decision, f);
    internal PromptChoice ChooseShiLyHuilvSettleAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiLyHuilvSettleAi(decision, f);
    internal PromptChoice ChooseShiLySongyanAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiLySongyanAi(decision, f);
    internal void CompleteShiLySettleDamage(ProgramSkillFrame f) => _resolve().CompleteShiLySettleDamage(f);
    internal void ContinueShiLy(long id) => _resolve().ContinueShiLy(id);
    internal bool DrainShiLy(ProgramSkillFrame f) => _resolve().DrainShiLy(f);
    internal void FinishShiLyOperation(ProgramSkillFrame f) => _resolve().FinishShiLyOperation(f);
    internal bool IsShiLySongyanDrawTwo(long id) => _resolve().IsShiLySongyanDrawTwo(id);
    internal PromptChoice MakeShiLyChoice(ProgramSkillFrame f, string branch, string label) => _resolve().MakeShiLyChoice(f, branch, label);
    internal void ObserveShiLyRoundDiscard(CardMovedEvent moved) => _resolve().ObserveShiLyRoundDiscard(moved);
    internal void ObserveShiLyShishiDamage(DamageAppliedEvent damage) => _resolve().ObserveShiLyShishiDamage(damage);
    internal void PublishShiLy(ProgramSkillFrame f) => _resolve().PublishShiLy(f);
    internal bool ResumeShiLy(long id) => _resolve().ResumeShiLy(id);
    internal bool ReturnShiLyMovement(ProgramSkillFrame f) => _resolve().ReturnShiLyMovement(f);
    internal IReadOnlyList<PromptChoice> ShiLyChoices(ProgramSkillFrame f) => _resolve().ShiLyChoices(f);
    internal IReadOnlyList<int> ShiLyGiveTargets(int holderSeat) => _resolve().ShiLyGiveTargets(holderSeat);
    internal ShiLyHuilvGrantedEvent? ShiLyOpenGrant(int ownerSeat, int actorSeat) => _resolve().ShiLyOpenGrant(ownerSeat, actorSeat);
    internal bool ShiLyShishiCannotRespond(int sourceSeat, int responderSeat) => _resolve().ShiLyShishiCannotRespond(sourceSeat, responderSeat);
    internal bool ShiLyShishiUseUnanswered(int ownerSeat) => _resolve().ShiLyShishiUseUnanswered(ownerSeat);
    internal bool ShiLySongyanConversionForbidden(ProgramViewAsLegalityQuery query) => _resolve().ShiLySongyanConversionForbidden(query);
    internal bool HasShiLyCapability { get => _resolve().HasShiLyCapability; }
}
