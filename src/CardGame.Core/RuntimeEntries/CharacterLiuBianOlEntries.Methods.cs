#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuBianOlEntries
{
    internal bool CanRunShiYuanTargetDraw(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunShiYuanTargetDraw(candidate, trigger, context);
    internal global::CardGame.Core.SkillProgramStepOutcome DuShiProgramGrantSkill(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().DuShiProgramGrantSkill(frame, effect);
    internal bool IsYuWeiLimitDoubled(int ownerSeat) => _resolve().IsYuWeiLimitDoubled(ownerSeat);
    internal bool ShiYuanDrawAvailable(int ownerSeat, int sourceHp) => _resolve().ShiYuanDrawAvailable(ownerSeat, sourceHp);
    internal int ShiYuanDrawCountFor(int ownerHp, int sourceHp) => _resolve().ShiYuanDrawCountFor(ownerHp, sourceHp);
    internal int ShiYuanOptionUsesThisTurn(int ownerSeat, int drawCount) => _resolve().ShiYuanOptionUsesThisTurn(ownerSeat, drawCount);
    internal global::CardGame.Core.SkillProgramStepOutcome ShiYuanProgramTargetDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ShiYuanProgramTargetDraw(frame, effect);
    internal int ShiYuanTurnLimit(int ownerSeat) => _resolve().ShiYuanTurnLimit(ownerSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome YuWeiProgramMarkActiveTurn(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().YuWeiProgramMarkActiveTurn(frame, effect);
    internal bool TracksShiYuanLedger { get => _resolve().TracksShiYuanLedger; }
}
