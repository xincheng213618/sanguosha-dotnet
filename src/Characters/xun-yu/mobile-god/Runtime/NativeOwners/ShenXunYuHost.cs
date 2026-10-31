#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenXunYuHost(Func<RuntimeEntries.CharacterXunYuMobileGodEntries> getCharacterXunYuMobileGodRuntime) : IShenXunYuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterXunYuMobileGodRuntime().ResolveShenXunYu(frame, op);
}
