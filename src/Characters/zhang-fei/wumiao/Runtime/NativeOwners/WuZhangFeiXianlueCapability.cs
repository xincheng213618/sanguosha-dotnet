#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WuZhangFeiXianlueCapability(Func<RuntimeEntries.CharacterZhangFeiWumiaoEntries> getCharacterZhangFeiWumiaoRuntime) : IWuZhangFeiXianlueHost, IWuZhangFeiXianlueSelection
{
    public SkillProgramStepOutcome Observe(ProgramSkillFrame frame) => getCharacterZhangFeiWumiaoRuntime().BeginWuZhangFeiXianlue(frame);
    public bool CanObserve(int ownerSeat, int targetSeat) => getCharacterZhangFeiWumiaoRuntime().CanObserveWuZhangFeiHand(ownerSeat, targetSeat);
}
