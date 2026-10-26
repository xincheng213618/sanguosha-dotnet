namespace CardGame.Core;

public sealed partial class GameEngine
{
    private int AdditionalOrdinaryAlcoholUses(CharacterState actor) =>
        _contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.AdditionalOrdinaryAlcoholUses)
            ? CardPolicies(actor, SkillProgramCardPolicyKind.AdditionalOrdinaryAlcoholUses).Sum(item => item.Policy.Value) : 0;

    private bool HasRemainingAdditionalAlcoholUse(CharacterState actor, int extra) =>
        CompleteProgramEventHistory().OfType<JianjiOrdinaryAlcoholPaidEvent>()
            .Count(fact => fact.ActualTurn == _turnNumber && fact.ActorSeat == actor.Seat && fact.CountsQuota) < 1 + extra;

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 5430)]
    private void ApplyAdditionalAlcoholQuota(JianjiOrdinaryAlcoholPaidEvent fact)
    {
        if (!fact.CountsQuota || !_contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.AdditionalOrdinaryAlcoholUses)) return;
        var actor = _players[fact.ActorSeat];
        var extra = AdditionalOrdinaryAlcoholUses(actor);
        if (extra > 0 && HasRemainingAdditionalAlcoholUse(actor, extra)) actor.UsedPlayPhaseAlcoholThisTurn = false;
    }
}
