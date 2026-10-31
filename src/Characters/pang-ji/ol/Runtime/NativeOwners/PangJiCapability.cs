#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PangJiCapability(Func<RuntimeEntries.CharacterPangJiOlEntries> getCharacterPangJiOlRuntime) : IPangJiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterPangJiOlRuntime().BeginPangJi(f, op);
}
