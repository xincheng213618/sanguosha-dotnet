#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MaChengCapability(Func<RuntimeEntries.CharacterMaChengOlEntries> getCharacterMaChengOlRuntime) : IMaChengHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaChengOlRuntime().BeginChenglie(f, op);
}
