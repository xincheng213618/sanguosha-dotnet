#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DiscardedProvenanceProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDiscardedProvenanceEntries> getCoreGameEngineDiscardedProvenanceRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProvenanceVirtualAlcoholEntries> getCoreGameEngineProvenanceVirtualAlcoholRuntime) : IDiscardedProvenanceProgramHost
{
    public SkillProgramStepOutcome ClaimDiscardedEntityWithProvenance(ProgramSkillFrame frame, string provenanceId) => getCoreGameEngineDiscardedProvenanceRuntime().BeginDiscardedProvenanceClaim(frame, provenanceId);
    public SkillProgramStepOutcome OfferFaceUpForOutsideClaims(ProgramSkillFrame frame, string provenanceId) => getCoreGameEngineDiscardedProvenanceRuntime().OfferProvenanceFaceUp(frame, provenanceId);
    public SkillProgramStepOutcome UseVirtualAlcohol(ProgramSkillFrame frame) => getCoreGameEngineProvenanceVirtualAlcoholRuntime().BeginProvenanceVirtualAlcohol(frame);
}
