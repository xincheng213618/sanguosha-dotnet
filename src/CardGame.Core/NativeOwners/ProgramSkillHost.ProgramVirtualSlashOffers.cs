#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed partial class ProgramSkillHost
{
    public SkillProgramStepOutcome OfferVirtualSlashOrDraw(SkillProgramEffect effect, ProgramSkillFrame frame, int actorSeat) => getCoreGameEngineProgramVirtualSlashOffersRuntime().OfferProgramVirtualSlashOrDraw(frame, actorSeat);
    public void GrantTurnCardEffectImmunity(ProgramSkillFrame frame, int targetSeat, IReadOnlyList<CardKind> cardKinds) => getCoreGameEngineProgramVirtualSlashOffersRuntime().GrantProgramTurnCardEffectImmunity(frame, targetSeat, cardKinds);
}
