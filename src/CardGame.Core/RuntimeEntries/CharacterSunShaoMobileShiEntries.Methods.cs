#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunShaoMobileShiEntries
{
    internal void AnswerShiSs(PromptChoice selected) => _resolve().AnswerShiSs(selected);
    internal void ApplyShiSsZhujiBonus(ProgramSkillFrame f, string choice) => _resolve().ApplyShiSsZhujiBonus(f, choice);
    internal void ApplyShiSsZhujiDiscard(ProgramSkillFrame f, int chosenCardId) => _resolve().ApplyShiSsZhujiDiscard(f, chosenCardId);
    internal void ApplyShiSsZhujiEquip(ProgramSkillFrame f) => _resolve().ApplyShiSsZhujiEquip(f);
    internal SkillProgramStepOutcome BeginShiSsGanjue(ProgramSkillFrame input) => _resolve().BeginShiSsGanjue(input);
    internal SkillProgramStepOutcome BeginShiSsZhuji(ProgramSkillFrame input) => _resolve().BeginShiSsZhuji(input);
    internal PromptChoice ChooseShiSsGanjueAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiSsGanjueAi(decision, f);
    internal PromptChoice ChooseShiSsZhujiAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiSsZhujiAi(decision, f);
    internal void ContinueShiSs(long id) => _resolve().ContinueShiSs(id);
    internal bool DrainShiSs(ProgramSkillFrame f) => _resolve().DrainShiSs(f);
    internal void EquipShiSsZhujiGain(ProgramSkillFrame f, int cardId) => _resolve().EquipShiSsZhujiGain(f, cardId);
    internal void FinishShiSsOperation(ProgramSkillFrame f) => _resolve().FinishShiSsOperation(f);
    internal bool IsShiSsGanjueStrikeUse(long useFrameId) => _resolve().IsShiSsGanjueStrikeUse(useFrameId);
    internal void IssueShiSsGanjueStrike(ProgramSkillFrame f, int equipmentCardId, int targetSeat) => _resolve().IssueShiSsGanjueStrike(f, equipmentCardId, targetSeat);
    internal PromptChoice MakeShiSsChoice(ProgramSkillFrame f, string branch, string label) => _resolve().MakeShiSsChoice(f, branch, label);
    internal void PublishShiSs(ProgramSkillFrame f) => _resolve().PublishShiSs(f);
    internal bool ResumeShiSs(long id) => _resolve().ResumeShiSs(id);
    internal bool ReturnShiSsMovement(ProgramSkillFrame f) => _resolve().ReturnShiSsMovement(f);
    internal void SettleShiSsGanjueStrike(ProgramSkillFrame f) => _resolve().SettleShiSsGanjueStrike(f);
    internal IReadOnlyList<PromptChoice> ShiSsChoices(ProgramSkillFrame f) => _resolve().ShiSsChoices(f);
    internal bool ShiSsGanjueCannotRespond(int sourceSeat, int responderSeat, CardKind? incomingKind, IReadOnlyList<int> targetSeats) => _resolve().ShiSsGanjueCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats);
    internal bool ShiSsGanjueSlashDistance(ProgramSlashDistanceQuery query) => _resolve().ShiSsGanjueSlashDistance(query);
    internal bool ShiSsGanjueUseDistance(ProgramCardDistanceQuery query) => _resolve().ShiSsGanjueUseDistance(query);
    internal bool ShiSsZhujiBonusQualified(ProgramSkillFrame f) => _resolve().ShiSsZhujiBonusQualified(f);
    internal bool HasShiSsCapability { get => _resolve().HasShiSsCapability; }
}
