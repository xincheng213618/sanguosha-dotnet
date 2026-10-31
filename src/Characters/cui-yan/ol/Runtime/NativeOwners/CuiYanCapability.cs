#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class CuiYanCapability(Func<RuntimeEntries.CharacterCuiYanOlEntries> getCharacterCuiYanOlRuntime) : ICuiYanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCuiYanOlRuntime().BeginCuiYan(f, op);
}
