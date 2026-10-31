#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiHuangZuHost(Func<RuntimeEntries.CharacterHuangZuMobileShiEntries> getCharacterHuangZuMobileShiRuntime) : IShiHuangZuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuangZuMobileShiRuntime().BeginShiHz(f, op);
}
