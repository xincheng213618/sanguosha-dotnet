#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileYuanShaoProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYuanShaoBoundaryMobileRuntime>> getCharacterYuanShaoBoundaryMobileRuntime) : IMobileYuanShaoProgramHost, IMobileYuanShaoSelectionQueries
{
    public bool SuitAvailable(int owner, Suit suit) => getCharacterYuanShaoBoundaryMobileRuntime().Value.MobileYuanShaoSuitAvailable(owner, suit);
    public SkillProgramStepOutcome Draw(ProgramSkillFrame frame, bool responder) => getCharacterYuanShaoBoundaryMobileRuntime().Value.DrawMobileYuanShao(frame, responder);
}
