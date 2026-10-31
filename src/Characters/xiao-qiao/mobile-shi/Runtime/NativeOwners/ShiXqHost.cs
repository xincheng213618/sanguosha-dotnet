#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiXqHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMobileShiEntries> getCharacterXiaoQiaoMobileShiRuntime) : IShiXqHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterXiaoQiaoMobileShiRuntime().BeginShiXq(f, operation);
}
