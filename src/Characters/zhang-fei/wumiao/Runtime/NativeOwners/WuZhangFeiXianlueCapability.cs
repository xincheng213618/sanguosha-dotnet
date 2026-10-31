#nullable enable
using System.Globalization;
using System.Numerics;

namespace CardGame.Core.NativeOwners;
internal sealed class WuZhangFeiXianlueCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangFeiWumiaoEntries> getCharacterZhangFeiWumiaoRuntime) : IWuZhangFeiXianlueHost, IWuZhangFeiXianlueSelection
{
    public SkillProgramStepOutcome Observe(ProgramSkillFrame frame) => getCharacterZhangFeiWumiaoRuntime().BeginWuZhangFeiXianlue(frame);
    public bool CanObserve(int ownerSeat, int targetSeat) => getCharacterZhangFeiWumiaoRuntime().CanObserveWuZhangFeiHand(ownerSeat, targetSeat);
}
