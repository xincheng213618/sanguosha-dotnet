#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileMaChaoHost(Func<RuntimeEntries.CharacterMaChaoMobileGodEntries> getCharacterMaChaoMobileGodRuntime) : IMobileShenMaChaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaChaoMobileGodRuntime().BeginMobileMaChao(f, op);
}
