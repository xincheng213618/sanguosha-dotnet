#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DiscardedProvenanceProgramCapability(Func<RuntimeEntries.CoreGameEngineDiscardedProvenanceEntries> getCoreGameEngineDiscardedProvenanceRuntime, Func<RuntimeEntries.CoreGameEngineProvenanceVirtualAlcoholEntries> getCoreGameEngineProvenanceVirtualAlcoholRuntime) : IDiscardedProvenanceProgramHost
{
    public SkillProgramStepOutcome ClaimDiscardedEntityWithProvenance(ProgramSkillFrame frame, string provenanceId) => getCoreGameEngineDiscardedProvenanceRuntime().BeginDiscardedProvenanceClaim(frame, provenanceId);
    public SkillProgramStepOutcome OfferFaceUpForOutsideClaims(ProgramSkillFrame frame, string provenanceId) => getCoreGameEngineDiscardedProvenanceRuntime().OfferProvenanceFaceUp(frame, provenanceId);
    public SkillProgramStepOutcome UseVirtualAlcohol(ProgramSkillFrame frame) => getCoreGameEngineProvenanceVirtualAlcoholRuntime().BeginProvenanceVirtualAlcohol(frame);
}
