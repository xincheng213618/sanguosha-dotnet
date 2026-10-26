namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryZhouCangZhongyong)]
    private PromptChoice SelectAiBoundaryZhongyong(PendingDecision decision, ProgramSkillFrame frame)
    {
        var view = CreateSnapshot(decision.PlayerSeat);
        var self = view.Players.Single(p => p.Seat == decision.PlayerSeat);
        var brain = _aiBrains.GetValueOrDefault(decision.PlayerSeat) ?? new SimpleAiBrain(decision.PlayerSeat,
            unchecked(_options.Seed * 397) ^ (decision.PlayerSeat + 1), _options.AiPolicyVersion);
        var gift = new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false);
        double Value(PromptChoice choice)
        {
            var branch = choice.Parameters.GetValueOrDefault("branch");
            if (branch == "decline") return 0;
            if (branch == "acquire-dodge")
                return choice.Cards.Count * (self.Hand.Any(c => c.Kind == CardKind.Dodge) ? 6 : self.Hp <= 2 ? 20 : 10);
            if (choice.Targets is not [var recipient]) return double.NegativeInfinity;
            var support = brain.ScoreProgramTarget(view, recipient, gift) / 4;
            // A bonus does not justify supplying an enemy with defense. The
            // extra attack remains a potential benefit of our own visible hand.
            if (support <= 0) return support - 1;
            return support * choice.Cards.Count + (branch == "give-dodge" && self.Hand.Any(c => IsSlashCard(c.Kind)) ? 8 : 0);
        }
        return decision.Choices.OrderByDescending(Value).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }
}
