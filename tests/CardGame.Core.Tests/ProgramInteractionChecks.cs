using System.Linq.Expressions;
using System.Reflection;
using CardGame.Core;

internal static class ProgramInteractionChecks
{

    public static void AiSelectionPreservesPublishedOrderAndConditionalPolicies()
    {
        var catalog = ProgramInteractionCatalog.Discover(Methods(typeof(ProgramAiChoiceSelector)));
        var router = new ProgramInteractionRouter(catalog, (method, type) => method.CreateDelegate(type),
            _ => throw new Exception("A pure AI policy reached answer activation."));
        var frame = Frame();
        var first = Choice("z", "unused", [1, 2], [0, 1], ("rank-sum", "10"));
        var tied = Choice("a", "unused", [3, 4], [2, 3], ("rank-sum", "10"));
        var lowerRank = Choice("b", "unused", [5, 6], [0], ("rank-sum", "9"));
        var fewerCards = Choice("c", "unused", [7], [], ("rank-sum", "99"));
        var decision = Decision(first, tied, lowerRank, fewerCards);
        var original = decision.Choices.ToArray();
        Require(ReferenceEquals(router.SelectAi(decision, frame, Effect(SkillProgramEffectOp.SelectSourceCard)), first) &&
                ReferenceEquals(router.SelectAi(decision, frame, Effect(SkillProgramEffectOp.DrawAfterOtherDeathThenChooseMissingSkill)), first) &&
                ReferenceEquals(router.SelectAi(decision, frame, Effect(SkillProgramEffectOp.CollectPublicPile)), first),
            "First-choice and equal-target-count policies must retain publication order, not alphabetize ties.");
        Require(ReferenceEquals(router.SelectAi(decision, frame, Effect(SkillProgramEffectOp.RevealTargetHandCard)), tied),
            "The explicit ordinal policy must preserve its distinct choice-ID ordering.");
        Require(ReferenceEquals(router.SelectAi(decision, frame,
                    Effect(SkillProgramEffectOp.SelectCardSubset, SkillProgramSubsetAiOrder.MostCardsThenRankSum)), tied),
            "Subset selection must prioritize card count, then rank sum, then ordinal ID.");
        Reject(() => router.SelectAi(decision, frame, Effect(SkillProgramEffectOp.SelectCardSubset)));
        Require(ReferenceEquals(router.SelectAi(decision, frame,
                    Effect(SkillProgramEffectOp.MoveBoundCards, destination: SkillProgramCardDestination.DrawPileBottom)), tied),
            "Bottom placement must retain its ordinal policy.");
        Reject(() => router.SelectAi(decision, frame,
            Effect(SkillProgramEffectOp.MoveBoundCards, destination: SkillProgramCardDestination.DiscardPile)));

        var pass = Choice("pass", "unused", parameters: [("branch", "pass")]);
        var nonPass = Choice("z-non-pass", "unused", parameters: [("branch", "use")]);
        Require(ReferenceEquals(router.SelectAi(Decision(pass, nonPass, tied), frame,
                    Effect(SkillProgramEffectOp.UseRoundPricedPileDyingAlcohol)), nonPass),
            "The non-pass policy must choose the first published eligible option.");
        var ordinary = Choice("a", "unused", parameters: [("card-kind", nameof(CardKind.Slash))]);
        var drawLaterId = Choice("z", "unused", parameters: [("card-kind", nameof(CardKind.DrawTwo))]);
        var drawEarlierId = Choice("b", "unused", parameters: [("card-kind", nameof(CardKind.DrawTwo))]);
        Require(ReferenceEquals(router.SelectAi(Decision(ordinary, drawLaterId, drawEarlierId), frame,
                    Effect(SkillProgramEffectOp.UseAllHandCardsAsOrdinaryTrick)), drawEarlierId),
            "Draw Two preference must precede ordinal tie-breaking.");

        var empty = Choice("00", "unused");
        var multiple = Choice("01", "unused", [10, 11]);
        var coveredZ = Choice("z", "unused", [12]);
        var coveredA = Choice("a", "unused", [13]);
        var coverage = Decision(empty, multiple, coveredZ, coveredA);
        var coverageEffect = Effect(SkillProgramEffectOp.SelectAndMoveOwnedCard, coverageBind: "coverage");
        var inspected = new List<int>();
        Require(ReferenceEquals(ProgramAiChoiceSelector.SelectByCoverage(coverage, coverageEffect, card =>
                { inspected.Add(card); return true; }), coveredA) && inspected.SequenceEqual([12, 13]),
            "Coverage ranking must only inspect singleton payments and use ordinal ties among matching cards.");
        Require(ReferenceEquals(ProgramAiChoiceSelector.SelectByCoverage(coverage, coverageEffect, _ => false), empty),
            "When no singleton reduces coverage, the old ordinal fallback must remain unchanged.");
        Require(ReferenceEquals(ProgramAiChoiceSelector.SelectByCoverage(Decision(coveredZ, empty, coveredA),
                Effect(SkillProgramEffectOp.SelectAndMoveOwnedCard),
                _ => throw new Exception("Disabled coverage policy inspected a card.")), coveredZ),
            "Without a coverage binding, selection must return the first published choice without sorting or querying cards.");
        Require(decision.Choices.Zip(original).All(pair => ReferenceEquals(pair.First, pair.Second)),
            "AI ranking must neither replace published choices nor reorder the prompt's original collection.");
        CheckSkillScopedAiChoices();
    }

    private static void CheckSkillScopedAiChoices()
    {
        var catalog = ProgramInteractionCatalog.Discover(Methods(typeof(ScopedAiChoices)));
        var bindings = 0;
        var router = new ProgramInteractionRouter(catalog, (method, type) =>
        { bindings++; return method.CreateDelegate(type); }, _ => throw new Exception("AI selection reached activation."));
        var decision = Decision(Choice("generic", "unused"), Choice("first-skill", "unused"), Choice("second-skill", "unused"));
        var effect = Effect(SkillProgramEffectOp.ChooseOption);
        Require(ReferenceEquals(router.SelectAi(decision, Frame(), effect), decision.Choices[1]) &&
                ReferenceEquals(router.SelectAi(decision, Frame() with { SkillId = "fixture:other" }, effect), decision.Choices[2]) &&
                ReferenceEquals(router.SelectAi(decision, Frame() with { SkillId = "fixture:unmatched" }, effect), decision.Choices[0]),
            "Distinct skills can register one operation; a matching skill policy takes precedence over the generic fallback.");
        Require(ReferenceEquals(router.SelectAi(decision, Frame(), effect), decision.Choices[1]) && bindings == 3,
            "Each selected policy binds once per router without caching another skill's policy for that operation.");
        Reject(() => ProgramInteractionCatalog.Discover(Methods(typeof(ScopedAiChoices)).Concat(Methods(typeof(DuplicateScopedAiChoice)))));
        Reject(() => ProgramInteractionCatalog.Discover(Methods(typeof(ScopedAiChoices)).Append(Methods(typeof(ScopedAiChoices)).Single(m => m.Name == "Generic"))));
        Reject(() => ProgramInteractionCatalog.Discover(Methods(typeof(EmptySkillAiChoice))));
    }

    private static class ScopedAiChoices
    {
        [ProgramAiChoice(SkillProgramEffectOp.ChooseOption)]
        private static PromptChoice Generic(PendingDecision decision) => decision.Choices[0];
        [ProgramAiChoice(SkillProgramEffectOp.ChooseOption, SkillId = "fixture:interaction")]
        private static PromptChoice First(PendingDecision decision, ProgramSkillFrame frame) => decision.Choices[1];
        [ProgramAiChoice(SkillProgramEffectOp.ChooseOption, SkillId = "fixture:other")]
        private static PromptChoice Second(PendingDecision decision, ProgramSkillFrame frame, SkillProgramEffect effect) => decision.Choices[2];
    }
    private static class DuplicateScopedAiChoice
    {
        [ProgramAiChoice(SkillProgramEffectOp.ChooseOption, SkillId = "fixture:interaction")]
        private static PromptChoice Duplicate(PendingDecision decision) => decision.Choices[0];
    }
    private static class EmptySkillAiChoice
    {
        [ProgramAiChoice(SkillProgramEffectOp.ChooseOption, SkillId = " ")]
        private static PromptChoice Invalid(PendingDecision decision) => decision.Choices[0];
    }

    private static MethodInfo[] Methods(Type type) => type.GetMethods(
        BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

    private static ProgramSkillFrame Frame() => new(41, 0, "fixture:interaction", "run", "hash", 0, [], []);

    private static PendingDecision Decision(params PromptChoice[] choices) =>
        new(DecisionKind.ProgramTrigger, 0, "Fixture", [], []) { PromptId = new(7), Revision = 9, Choices = choices };

    private static PromptChoice Choice(string id, string action, IReadOnlyList<int>? cards = null,
        IReadOnlyList<int>? targets = null, params (string Key, string Value)[] parameters)
    {
        var values = parameters.ToDictionary(item => item.Key, item => item.Value, StringComparer.Ordinal);
        values.Add("program-action", action);
        return new(new(id), id, cards ?? [], targets ?? [], values);
    }

    private static SkillProgramEffect Effect(SkillProgramEffectOp op, SkillProgramSubsetAiOrder? order = null,
        SkillProgramCardDestination? destination = null, string? coverageBind = null) =>
        new(op, SkillProgramEffectTarget.Owner, 0, new(SkillProgramConditionKind.Always, 0, []),
            aiOrder: order, destination: destination, coverageResultBind: coverageBind);

    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new Exception("An invalid interaction contract or route was accepted.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
