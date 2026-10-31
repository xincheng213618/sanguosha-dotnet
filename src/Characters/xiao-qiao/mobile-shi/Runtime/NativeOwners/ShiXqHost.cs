#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiXqHost(Func<RuntimeEntries.CharacterXiaoQiaoMobileShiEntries> getCharacterXiaoQiaoMobileShiRuntime) : IShiXqHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterXiaoQiaoMobileShiRuntime().BeginShiXq(f, operation);
}
