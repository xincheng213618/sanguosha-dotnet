#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiXinXianYingHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXinXianYingMobileShiEntries> getCharacterXinXianYingMobileShiRuntime) : IShiXinXianYingHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op, int actor) => getCharacterXinXianYingMobileShiRuntime().BeginShiXxy(f, op, actor);
}
