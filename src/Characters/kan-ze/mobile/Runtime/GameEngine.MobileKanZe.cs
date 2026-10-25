namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 75800)]
    private void ConsumeMobileKuanshiFirstPrevention(KuanshiDamagePreventedEvent fact)
    {
        foreach (var owner in _players)
        {
            var grant = owner.KuanshiGrants.SingleOrDefault(g =>
                g.FrameId == fact.GrantFrameId && g.Source.SkillId == "mobile:kuanshi");
            if (grant is null) continue;
            owner.KuanshiGrants = owner.KuanshiGrants.Where(g => g.FrameId != grant.FrameId).ToArray();
            AdvanceEventRulesAndQueueFact(new KuanshiExpiredEvent(grant.FrameId, _turnNumber, "mobile-first-heavy-damage"));
        }
    }
}
