#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunShaoMobileShiEntries
{
    internal void AnswerShiSs(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiSs(selected);
    internal void ApplyShiSsZhujiBonus(global::CardGame.Core.ProgramSkillFrame f, string choice) => _resolve().ApplyShiSsZhujiBonus(f, choice);
    internal void ApplyShiSsZhujiDiscard(global::CardGame.Core.ProgramSkillFrame f, int chosenCardId) => _resolve().ApplyShiSsZhujiDiscard(f, chosenCardId);
    internal void ApplyShiSsZhujiEquip(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ApplyShiSsZhujiEquip(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiSsGanjue(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiSsGanjue(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiSsZhuji(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiSsZhuji(input);
    internal global::CardGame.Core.PromptChoice ChooseShiSsGanjueAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiSsGanjueAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiSsZhujiAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiSsZhujiAi(decision, f);
    internal void ContinueShiSs(long id) => _resolve().ContinueShiSs(id);
    internal bool DrainShiSs(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiSs(f);
    internal void EquipShiSsZhujiGain(global::CardGame.Core.ProgramSkillFrame f, int cardId) => _resolve().EquipShiSsZhujiGain(f, cardId);
    internal void FinishShiSsOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiSsOperation(f);
    internal bool IsShiSsGanjueStrikeUse(long useFrameId) => _resolve().IsShiSsGanjueStrikeUse(useFrameId);
    internal void IssueShiSsGanjueStrike(global::CardGame.Core.ProgramSkillFrame f, int equipmentCardId, int targetSeat) => _resolve().IssueShiSsGanjueStrike(f, equipmentCardId, targetSeat);
    internal global::CardGame.Core.PromptChoice MakeShiSsChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label) => _resolve().MakeShiSsChoice(f, branch, label);
    internal void PublishShiSs(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiSs(f);
    internal bool ResumeShiSs(long id) => _resolve().ResumeShiSs(id);
    internal bool ReturnShiSsMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiSsMovement(f);
    internal void SettleShiSsGanjueStrike(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleShiSsGanjueStrike(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiSsChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiSsChoices(f);
    internal bool ShiSsGanjueCannotRespond(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind? incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => _resolve().ShiSsGanjueCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats);
    internal bool ShiSsGanjueSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery query) => _resolve().ShiSsGanjueSlashDistance(query);
    internal bool ShiSsGanjueUseDistance(global::CardGame.Core.ProgramCardDistanceQuery query) => _resolve().ShiSsGanjueUseDistance(query);
    internal bool ShiSsZhujiBonusQualified(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiSsZhujiBonusQualified(f);
    internal bool HasShiSsCapability { get => _resolve().HasShiSsCapability; }
}
