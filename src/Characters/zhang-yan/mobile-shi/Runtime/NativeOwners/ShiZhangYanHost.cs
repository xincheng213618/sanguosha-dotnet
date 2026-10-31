#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiZhangYanHost(Func<RuntimeEntries.CharacterZhangYanMobileShiEntries> getCharacterZhangYanMobileShiRuntime) : IShiZhangYanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangYanMobileShiRuntime().BeginShiZy(f, op);
}
