#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeZhouFeiXianshuCapability(Func<RuntimeEntries.CharacterZhouFeiLeEntries> getCharacterZhouFeiLeRuntime) : ILeZhouFeiXianshuHost, ILeZhouFeiXianshuSelectionQuery
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterZhouFeiLeRuntime().BeginLeZhouFeiXianshu(frame);
    public bool CanGive(int ownerSeat, Card card) => getCharacterZhouFeiLeRuntime().HasLingkongHandEntityMark(ownerSeat, card.Id);
}
