#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiHeQiHost(Func<RuntimeEntries.CharacterHeQiMobileShiEntries> getCharacterHeQiMobileShiRuntime) : IShiHeQiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHeQiMobileShiRuntime().BeginShiHq(f, op);
}
