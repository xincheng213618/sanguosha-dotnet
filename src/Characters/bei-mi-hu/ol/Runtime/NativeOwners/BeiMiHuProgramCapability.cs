#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BeiMiHuProgramCapability(Func<RuntimeEntries.CharacterBeiMiHuOlEntries> getCharacterBeiMiHuOlRuntime) : IBeiMiHuProgramHost
{
    public SkillProgramStepOutcome BeginBeiMiHuOperation(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterBeiMiHuOlRuntime().BeginBeiMiHuOperation(f, op);
}
