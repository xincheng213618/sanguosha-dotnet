#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class YanPuCapability(Func<RuntimeEntries.CharacterYanPuOlEntries> getCharacterYanPuOlRuntime) : IYanPuHost
{
    public SkillProgramStepOutcome ExecuteYanPu(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYanPuOlRuntime().BeginYanPu(f, op);
}
