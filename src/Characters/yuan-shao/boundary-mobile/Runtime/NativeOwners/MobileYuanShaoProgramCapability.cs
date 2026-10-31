#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileYuanShaoProgramCapability(Func<RuntimeEntries.CharacterYuanShaoBoundaryMobileEntries> getCharacterYuanShaoBoundaryMobileRuntime) : IMobileYuanShaoProgramHost, IMobileYuanShaoSelectionQueries
{
    public bool SuitAvailable(int owner, Suit suit) => getCharacterYuanShaoBoundaryMobileRuntime().MobileYuanShaoSuitAvailable(owner, suit);
    public SkillProgramStepOutcome Draw(ProgramSkillFrame frame, bool responder) => getCharacterYuanShaoBoundaryMobileRuntime().DrawMobileYuanShao(frame, responder);
}
