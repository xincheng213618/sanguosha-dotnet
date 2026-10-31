#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiangXingOlEntries
{
    internal SkillProgramStepOutcome GiveProgramSelectedTargetHand(ProgramSkillFrame frame, int targetSeat) => _resolve().GiveProgramSelectedTargetHand(frame, targetSeat);
    internal SkillProgramStepOutcome LuelveProgramCounterSlash(ProgramSkillFrame frame) => _resolve().LuelveProgramCounterSlash(frame);
}
