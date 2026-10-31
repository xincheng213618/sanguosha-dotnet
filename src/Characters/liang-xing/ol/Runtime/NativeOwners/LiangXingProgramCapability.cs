#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiangXingProgramCapability(Func<RuntimeEntries.CharacterLiangXingOlEntries> getCharacterLiangXingOlRuntime) : ILiangXingProgramHost, ILuelveCounterSlashProgramHost
{
    public SkillProgramStepOutcome GiveSelectedTargetHand(ProgramSkillFrame frame, int targetSeat) => getCharacterLiangXingOlRuntime().GiveProgramSelectedTargetHand(frame, targetSeat);
    public SkillProgramStepOutcome SelectedTargetVirtualSlashAgainstOwner(ProgramSkillFrame frame) => getCharacterLiangXingOlRuntime().LuelveProgramCounterSlash(frame);
}
