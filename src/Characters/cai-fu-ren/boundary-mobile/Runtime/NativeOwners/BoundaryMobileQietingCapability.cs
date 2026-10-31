#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryMobileQietingCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaiFuRenOlEntries> getCharacterCaiFuRenOlRuntime) : IBoundaryMobileQietingHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame) => getCharacterCaiFuRenOlRuntime().BeginOrdinaryQieting(frame);
}
