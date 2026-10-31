#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeXiaoQiaoWeiwanCapability(Func<RuntimeEntries.CharacterXiaoQiaoLeEntries> getCharacterXiaoQiaoLeRuntime) : ILeXiaoQiaoWeiwanHost, ILeXiaoQiaoWeiwanSelectionQuery
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterXiaoQiaoLeRuntime().BeginWeiwan(f);
    public bool CanDiscard(int owner, Card card) => getCharacterXiaoQiaoLeRuntime().CanPayWeiwanQin(owner, card);
}
