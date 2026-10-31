#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PrivateOfferProgramEffectCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramPrivateDamageOfferOperationsEntries> getCoreProgramPrivateDamageOfferOperationsRuntime) : IPrivateOfferProgramEffectHost
{
    public SkillProgramStepOutcome DiscardSelectedParticipantCards(ProgramSkillFrame frame) => getCoreProgramPrivateDamageOfferOperationsRuntime().DiscardProgramSelectedParticipantCards(frame);
    public SkillProgramStepOutcome OfferBoundCardsForDamagePrevention(ProgramSkillFrame frame, string bind) => getCoreProgramPrivateDamageOfferOperationsRuntime().OfferProgramBoundCardsForDamagePrevention(frame, bind);
}
