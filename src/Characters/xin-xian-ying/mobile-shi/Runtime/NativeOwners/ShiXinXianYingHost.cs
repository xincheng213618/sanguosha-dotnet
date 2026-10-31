#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiXinXianYingHost(Func<RuntimeEntries.CharacterXinXianYingMobileShiEntries> getCharacterXinXianYingMobileShiRuntime) : IShiXinXianYingHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op, int actor) => getCharacterXinXianYingMobileShiRuntime().BeginShiXxy(f, op, actor);
}
