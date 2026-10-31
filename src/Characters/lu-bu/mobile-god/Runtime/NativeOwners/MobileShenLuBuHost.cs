#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShenLuBuHost(Func<RuntimeEntries.CharacterLuBuMobileGodEntries> getCharacterLuBuMobileGodRuntime) : IMobileShenLuBuHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuBuMobileGodRuntime().BeginMobileLb(f, op);
}
