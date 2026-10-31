#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouGanNingCapability(Func<RuntimeEntries.CharacterGanNingMouMobileEntries> getCharacterGanNingMouMobileRuntime) : IMouGanNingHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGanNingMouMobileRuntime().BeginMouGn(f, op);
}
