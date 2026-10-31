#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GuanHaiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries> getCharacterGuanHaiOlRuntime) : IGuanHaiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuanHaiOlRuntime().BeginGuanHai(f, op);
}
