#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PrivateOfferProgramEffectCapability(Func<RuntimeEntries.CoreProgramPrivateDamageOfferOperationsEntries> getCoreProgramPrivateDamageOfferOperationsRuntime) : IPrivateOfferProgramEffectHost
{
    public SkillProgramStepOutcome DiscardSelectedParticipantCards(ProgramSkillFrame frame) => getCoreProgramPrivateDamageOfferOperationsRuntime().DiscardProgramSelectedParticipantCards(frame);
    public SkillProgramStepOutcome OfferBoundCardsForDamagePrevention(ProgramSkillFrame frame, string bind) => getCoreProgramPrivateDamageOfferOperationsRuntime().OfferProgramBoundCardsForDamagePrevention(frame, bind);
}
