namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_ProvenanceCardPolicies
{
    internal static bool IsProvenanceOwnedZone(CardLocation location,int owner)=>location.OwnerSeat==owner && location.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.WoodenOxGrain;
}
