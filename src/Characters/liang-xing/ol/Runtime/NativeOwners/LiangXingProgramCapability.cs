#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiangXingProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiangXingOlRuntime>> getCharacterLiangXingOlRuntime) : ILiangXingProgramHost, ILuelveCounterSlashProgramHost
{
    public SkillProgramStepOutcome GiveSelectedTargetHand(ProgramSkillFrame frame, int targetSeat) => getCharacterLiangXingOlRuntime().Value.GiveProgramSelectedTargetHand(frame, targetSeat);
    public SkillProgramStepOutcome SelectedTargetVirtualSlashAgainstOwner(ProgramSkillFrame frame) => getCharacterLiangXingOlRuntime().Value.LuelveProgramCounterSlash(frame);
}
