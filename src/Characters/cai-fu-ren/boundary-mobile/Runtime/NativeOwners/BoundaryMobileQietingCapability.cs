#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryMobileQietingCapability(Func<RuntimeEntries.CharacterCaiFuRenOlEntries> getCharacterCaiFuRenOlRuntime) : IBoundaryMobileQietingHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame) => getCharacterCaiFuRenOlRuntime().BeginOrdinaryQieting(frame);
}
