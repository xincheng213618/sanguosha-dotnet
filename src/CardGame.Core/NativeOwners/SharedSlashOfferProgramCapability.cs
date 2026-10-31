#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SharedSlashOfferProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSharedSlashOffersEntries> getCoreGameEngineSharedSlashOffersRuntime) : ISharedSlashOfferProgramHost
{
    public SkillProgramStepOutcome GiveBoundCardThenOffer(ProgramSkillFrame frame, int actorSeat, string sourceBind) => getCoreGameEngineSharedSlashOffersRuntime().BeginSharedSlashGift(frame.Id, actorSeat, sourceBind);
}
