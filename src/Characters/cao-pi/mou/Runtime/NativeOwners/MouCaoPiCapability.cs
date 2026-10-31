#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouCaoPiCapability(Func<RuntimeEntries.CharacterCaoPiMouEntries> getCharacterCaoPiMouRuntime) : IMouCaoPiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaoPiMouRuntime().BeginMouCaoPi(f, op);
}
