#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramAlcoholQuotaRuntime
{
    internal bool HasAdditionalAlcoholUse(CharacterState owner)
    {
        if (_coreNationalSkillsOperations.GetSkillBindingShard(owner).GetCardPolicies(SkillProgramCardPolicyKind.AdditionalAlcoholUses).Count == 0)
            return false;
        var extra = _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.AdditionalAlcoholUses, CardKind.Alcohol).Sum(item => (long)item.Policy.Value);
        // The native paid-use ledger starts when this capability is registered, including uses
        // before a live purchase. Dying recovery and quota-exempt uses never consume this budget.
        return extra > 0 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JianjiOrdinaryAlcoholPaidEvent>().Count(p => p.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber && p.ActorSeat == owner.Seat && p.CountsQuota) < 1L + extra;
    }
}
