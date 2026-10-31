#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiDengAiCapability(Func<RuntimeEntries.CharacterDengAiMobileShiEntries> getCharacterDengAiMobileShiRuntime) : IShiDengAiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDengAiMobileShiRuntime().BeginShiDa(f, op);
}
