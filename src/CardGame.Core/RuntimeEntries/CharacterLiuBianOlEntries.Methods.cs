#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuBianOlEntries
{
    internal bool CanRunShiYuanTargetDraw(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunShiYuanTargetDraw(candidate, trigger, context);
    internal SkillProgramStepOutcome DuShiProgramGrantSkill(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().DuShiProgramGrantSkill(frame, effect);
    internal bool IsYuWeiLimitDoubled(int ownerSeat) => _resolve().IsYuWeiLimitDoubled(ownerSeat);
    internal bool ShiYuanDrawAvailable(int ownerSeat, int sourceHp) => _resolve().ShiYuanDrawAvailable(ownerSeat, sourceHp);
    internal int ShiYuanDrawCountFor(int ownerHp, int sourceHp) => _resolve().ShiYuanDrawCountFor(ownerHp, sourceHp);
    internal int ShiYuanOptionUsesThisTurn(int ownerSeat, int drawCount) => _resolve().ShiYuanOptionUsesThisTurn(ownerSeat, drawCount);
    internal SkillProgramStepOutcome ShiYuanProgramTargetDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ShiYuanProgramTargetDraw(frame, effect);
    internal int ShiYuanTurnLimit(int ownerSeat) => _resolve().ShiYuanTurnLimit(ownerSeat);
    internal SkillProgramStepOutcome YuWeiProgramMarkActiveTurn(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().YuWeiProgramMarkActiveTurn(frame, effect);
    internal bool TracksShiYuanLedger { get => _resolve().TracksShiYuanLedger; }
}
