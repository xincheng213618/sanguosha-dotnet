#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class CaoXiCapability(Func<RuntimeEntries.CharacterCaoXiOlEntries> getCharacterCaoXiOlRuntime) : ICaoXiProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaoXiOlRuntime().BeginCaoXi(f, op);
}
