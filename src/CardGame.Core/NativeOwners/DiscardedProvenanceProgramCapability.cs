#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DiscardedProvenanceProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineDiscardedProvenanceRuntime>> getCoreGameEngineDiscardedProvenanceRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime>> getCoreGameEngineProvenanceVirtualAlcoholRuntime) : IDiscardedProvenanceProgramHost
{
    public SkillProgramStepOutcome ClaimDiscardedEntityWithProvenance(ProgramSkillFrame frame, string provenanceId) => getCoreGameEngineDiscardedProvenanceRuntime().Value.BeginDiscardedProvenanceClaim(frame, provenanceId);
    public SkillProgramStepOutcome OfferFaceUpForOutsideClaims(ProgramSkillFrame frame, string provenanceId) => getCoreGameEngineDiscardedProvenanceRuntime().Value.OfferProvenanceFaceUp(frame, provenanceId);
    public SkillProgramStepOutcome UseVirtualAlcohol(ProgramSkillFrame frame) => getCoreGameEngineProvenanceVirtualAlcoholRuntime().Value.BeginProvenanceVirtualAlcohol(frame);
}
