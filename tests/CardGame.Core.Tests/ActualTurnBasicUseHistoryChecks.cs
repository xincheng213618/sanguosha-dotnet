using CardGame.Core;
using CardGame.Core.RuntimeSkills;

internal static class ActualTurnBasicUseHistoryChecks
{
    public static void BasicPrefixRetainsNativeOrderNamesAndTurnEndBoundary()
    {
        CurrentTurnCardUseKindsRecordedEvent Fact(int actor, long origin, long? action, CardKind kind,
            CardActionType type = CardActionType.Use, int? provider = null) =>
            new(2, 1, actor, origin, action, 0,
                1 << (int)(kind == CardKind.Duel ? SkillProgramCardCategory.Trick : SkillProgramCardCategory.Basic),
                RoundActionType: type, RoundProviderSeat: provider ?? actor) { EffectiveCardKind = kind };
        var slash = Fact(1, 20, 200, CardKind.Slash);
        var dodge = Fact(2, 20, 201, CardKind.Dodge, CardActionType.Response);
        var history = new List<IGameEvent>
        {
            new TurnStartedEvent(1, 0),
            Fact(0, 10, 100, CardKind.Peach) with { TurnNumber = 1, TurnOwnerSeat = 0 },
            new TurnEndedEvent(1, 0), new TurnStartedEvent(2, 1),
            Fact(1, 19, 199, CardKind.Duel), slash,
            new ProgramCurrentSlashFireChangedEvent(99, 20, 200,
                new("fixture:basic-prefix", "fire", 1, "seat-1:basic-prefix"), CardKind.Slash, CardKind.FireSlash, null),
            dodge,
            Fact(3, 20, 202, CardKind.Dodge, CardActionType.Response, provider: 0),
            Fact(0, 23, null, CardKind.Alcohol),
            Fact(3, 24, 240, CardKind.Peach)
        };
        var beforeEnd = RuntimeRules_Core_CurrentTurnUseKinds.ActualTurnBasicUses(history, 2, 1);
        Require(beforeEnd.Select(f => f.EffectiveCardKind).SequenceEqual(new CardKind?[]
                { CardKind.FireSlash, CardKind.Dodge, CardKind.Alcohol, CardKind.Peach }) &&
            beforeEnd.Select(f => f.ActorSeat).SequenceEqual([1, 2, 0, 3]) &&
            beforeEnd[1].OriginFrameId == slash.OriginFrameId && beforeEnd[1].CardActionId == dodge.CardActionId &&
            beforeEnd[2].CardActionId is null && slash.EffectiveCardKind == CardKind.Slash,
            "The shared history keeps declaration order, corrects only the attacking Slash's name, includes native zero-action Wine, and excludes supplied Dodge without mutating recorded facts.");

        history.Add(new TurnEndedEvent(2, 1));
        history.Add(Fact(0, 25, 250, CardKind.Peach));
        history.Add(new TurnStartedEvent(3, 2));
        history.Add(Fact(2, 30, 300, CardKind.Peach) with { TurnNumber = 3, TurnOwnerSeat = 2 });
        var afterEnd = RuntimeRules_Core_CurrentTurnUseKinds.ActualTurnBasicUses(history, 2, 1);
        Require(afterEnd.SequenceEqual(beforeEnd),
            "An AfterTurnEnded reader retains the original turn instead of observing an empty current-turn tail or later uses.");
        var firstThree = afterEnd.Take(3).Select(f => new AHuiNanBasicUseSnapshot(f.ActorSeat, f.EffectiveCardKind!.Value)).ToArray();
        Require(firstThree.Length == 3 && firstThree.Take(2).Count(f => f.ActorSeat == 1) == 1 &&
            firstThree.Take(2).Count(f => f.ActorSeat == 3) == 0 && firstThree[2].Kind == CardKind.Alcohol,
            "Jueman's existing first-two-actors and third-basic-name rules consume the shared frozen prefix directly.");
    }

    private static void Require(bool value, string message)
    {
        if (!value) throw new InvalidOperationException(message);
    }
}
