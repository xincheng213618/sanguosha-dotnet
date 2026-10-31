#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GuanHaiCapability(Func<RuntimeEntries.CharacterGuanHaiOlEntries> getCharacterGuanHaiOlRuntime) : IGuanHaiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuanHaiOlRuntime().BeginGuanHai(f, op);
}
