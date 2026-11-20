using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class MobileFumianTurnGapChecks
{
    private const string Hero = "mobile:wu-xian", Skill = "mobile:fumian", Daiyan = "classic:daiyan";
    private const string Peer = "fixture:fumian-quiet-peer", Deck = "fixture:fumian-turn-gap-deck";

    internal static void SkippedOwnTurnLeavesOneDrawBonus()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(17, 1, "fumian-turn-gap", Hero, 3, 0, 0, 0, 0, [], [], [],
            [new(Peer) { StartingHand = [] }], [], new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry);

        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        void AnswerOwner(PendingDecision prompt, string key, string value) => Accept(game,
            new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(c => c.Parameters.GetValueOrDefault(key) == value).Id, game.Revision));
        PendingDecision Reach(Func<PendingDecision, bool> ready)
        {
            for (var step = 0; step < 150 && game.State.Winner == Winner.None; step++)
            {
                if (game.CreateSnapshot(0).PendingDecision is { } prompt)
                {
                    if (ready(prompt)) return prompt;
                    Require(prompt.SkillPrompt?.SkillId == Daiyan, $"Unexpected Fumian fixture choice {prompt.Kind}/{prompt.SkillPrompt?.SkillId}.");
                    AnswerOwner(prompt, "program-action", "skip");
                }
                else Accept(game, new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("Fumian missed its expected actual-turn choice.");
        }
        PendingDecision Offer() => Reach(p => p.SkillPrompt?.SkillId == Skill &&
            p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        void Choose(string option)
        {
            AnswerOwner(Offer(), "program-action", "activate");
            AnswerOwner(Reach(p => p.SkillPrompt?.SkillId == Skill), "option-id", option);
        }
        void EndPlay()
        {
            var play = Reach(p => p.Kind == DecisionKind.PlayCard);
            Accept(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        }

        Accept(game, new StartGameCommand());
        Choose("targets");
        var first = Facts<AlternatingChoiceBenefitResolvedEvent>().Single();
        var targets = Facts<RedAdditionalTargetsGrantedEvent>().Single();
        Require(first is { OwnerTurnOrdinal: 1, Amount: 1, OptionId: "targets" } &&
            targets is { OwnerSeat: 0, Maximum: 1, Source.SkillId: Skill } && targets.Source.SkillInstanceId == first.SkillInstanceId,
            "The first actual choice issues a real red-target entitlement from the mobile skill instance.");
        EndPlay();
        AnswerOwner(Offer(), "program-action", "skip");
        EndPlay();
        Choose("draw");
        Reach(p => p.Kind == DecisionKind.PlayCard);

        var starts = Facts<TurnStartedEvent>().Where(turn => turn.ActorSeat == 0).ToArray();
        var issued = Facts<AlternatingChoiceDrawBenefitIssuedEvent>().Single();
        var assigned = Facts<AlternatingChoiceDrawPhaseAssignedEvent>().Single();
        var draw = game.Events.SkipWhile(entry => entry.Payload is not TurnStartedEvent { ActorSeat: 0 } started ||
                started.TurnNumber != game.State.TurnNumber).Select(entry => entry.Payload).OfType<CardsDrawnEvent>().Single();
        Require(starts is [_, _, var third] && third.TurnNumber == issued.TurnNumber &&
            Facts<AlternatingChoiceBenefitResolvedEvent>() is [var old, { OwnerTurnOrdinal: 3, OptionId: "draw", Amount: 1 } resolved] &&
            old == first && resolved.PendingOptionId == "draw" && resolved.SkillInstanceId == first.SkillInstanceId &&
            issued is { OwnerTurnOrdinal: 3, Amount: 1, Source: { SkillId: Skill, OwnerSeat: 0 } } &&
            issued.Source.SkillInstanceId == first.SkillInstanceId && issued.ProgramFrameId == assigned.ProgramFrameId &&
            issued.InstructionIndex == assigned.InstructionIndex &&
            assigned.Phase is { Kind: ActualDiscardRecoveryPhaseKind.Draw, ActorSeat: 0, ActualTurnOwnerSeat: 0 } &&
            assigned.Phase.TurnNumber == issued.TurnNumber && Facts<ActualDiscardRecoveryPhaseStartedEvent>().Any(e => e.Phase == assigned.Phase) &&
            draw is { PlayerSeat: 0, Count: 2 },
            "A skipped real own turn leaves the third-turn Draw at one bonus card on its original native phase.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-fumian-turn-gap", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, "classic:wu-xian");
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "福绵固定牌堆", 0, 1, [new("standard:peach", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "福绵实际回合", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
