#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeXiaoQiaoWeiwanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoLeEntries> getCharacterXiaoQiaoLeRuntime) : ILeXiaoQiaoWeiwanHost, ILeXiaoQiaoWeiwanSelectionQuery
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterXiaoQiaoLeRuntime().BeginWeiwan(f);
    public bool CanDiscard(int owner, Card card) => getCharacterXiaoQiaoLeRuntime().CanPayWeiwanQin(owner, card);
}
