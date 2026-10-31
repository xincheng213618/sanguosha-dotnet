#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLvYiMobileShiEntries
{
    internal void AnswerShiLy(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiLy(selected);
    internal void ApplyShiLyHuilvGive(global::CardGame.Core.ProgramSkillFrame f, int targetSeat) => _resolve().ApplyShiLyHuilvGive(f, targetSeat);
    internal void ApplyShiLyHuilvGrant(global::CardGame.Core.ProgramSkillFrame f, bool red) => _resolve().ApplyShiLyHuilvGrant(f, red);
    internal void ApplyShiLySongyanUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ApplyShiLySongyanUse(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiLyHuilv(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiLyHuilv(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiLyHuilvSettle(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiLyHuilvSettle(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiLySongyan(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiLySongyan(input);
    internal bool CanOfferShiLyHuilv(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferShiLyHuilv(c);
    internal bool CanOfferShiLyHuilvSettle(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferShiLyHuilvSettle(c);
    internal bool CanOfferShiLySongyan(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferShiLySongyan(c);
    internal global::CardGame.Core.PromptChoice ChooseShiLyHuilvAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiLyHuilvAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiLyHuilvSettleAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiLyHuilvSettleAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiLySongyanAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiLySongyanAi(decision, f);
    internal void CompleteShiLySettleDamage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteShiLySettleDamage(f);
    internal void ContinueShiLy(long id) => _resolve().ContinueShiLy(id);
    internal bool DrainShiLy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiLy(f);
    internal void FinishShiLyOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiLyOperation(f);
    internal bool IsShiLySongyanDrawTwo(long id) => _resolve().IsShiLySongyanDrawTwo(id);
    internal global::CardGame.Core.PromptChoice MakeShiLyChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label) => _resolve().MakeShiLyChoice(f, branch, label);
    internal void ObserveShiLyRoundDiscard(global::CardGame.Core.CardMovedEvent moved) => _resolve().ObserveShiLyRoundDiscard(moved);
    internal void ObserveShiLyShishiDamage(global::CardGame.Core.DamageAppliedEvent damage) => _resolve().ObserveShiLyShishiDamage(damage);
    internal void PublishShiLy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiLy(f);
    internal bool ResumeShiLy(long id) => _resolve().ResumeShiLy(id);
    internal bool ReturnShiLyMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiLyMovement(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiLyChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiLyChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<int> ShiLyGiveTargets(int holderSeat) => _resolve().ShiLyGiveTargets(holderSeat);
    internal global::CardGame.Core.ShiLyHuilvGrantedEvent? ShiLyOpenGrant(int ownerSeat, int actorSeat) => _resolve().ShiLyOpenGrant(ownerSeat, actorSeat);
    internal bool ShiLyShishiCannotRespond(int sourceSeat, int responderSeat) => _resolve().ShiLyShishiCannotRespond(sourceSeat, responderSeat);
    internal bool ShiLyShishiUseUnanswered(int ownerSeat) => _resolve().ShiLyShishiUseUnanswered(ownerSeat);
    internal int[]? ShiLySongyanAncestorProcessing(long id) => _resolve().ShiLySongyanAncestorProcessing(id);
    internal bool ShiLySongyanConversionForbidden(global::CardGame.Core.ProgramViewAsLegalityQuery query) => _resolve().ShiLySongyanConversionForbidden(query);
    internal bool HasShiLyCapability { get => _resolve().HasShiLyCapability; }
}
