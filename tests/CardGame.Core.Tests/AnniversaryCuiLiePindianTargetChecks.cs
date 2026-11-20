using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class AnniversaryCuiLiePindianTargetChecks
{
    private const string Hero = "anniversary:cui-lie", Chibi = "anniversary:chibi";
    private const string Mode = "identity:anniversary-chibi-target", Deck = "fixture:anniversary-chibi-target-deck";
    private static readonly string[] Generals =
        ["fixture:anniversary-chibi-user", Hero, "fixture:anniversary-chibi-far", "fixture:anniversary-chibi-near"];

    internal static void FarNativeUseOpensOriginalPindianCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, registry);
        PendingDecision? Prompt() => Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(prompt => prompt is not null);
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Chibi fixture command was rejected.");
        }
        PendingDecision Reach(Func<PendingDecision, bool> ready)
        {
            for (var step = 0; step < 80; step++)
            {
                if (Prompt() is { } prompt)
                {
                    if (ready(prompt)) return prompt;
                    throw new InvalidOperationException($"Unexpected Chibi fixture prompt {prompt.Kind}/{prompt.SkillPrompt?.SkillId} for seat {prompt.PlayerSeat}.");
                }
                if (game.State.Winner != Winner.None) break;
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException($"Chibi fixture missed its expected choice in {game.State.Phase}.");
        }

        Submit(new StartGameCommand());
        var play = Reach(prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (play.Kind == DecisionKind.SelectGeneral)
        {
            Submit(new SelectGeneralCommand(play.PlayerSeat, Generals[play.PlayerSeat], game.Revision, play.PromptId));
            play = Reach(prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        Need(play.PlayerSeat == 0 && game.CreateSnapshot(0).Players[2].IsAlive &&
            game.GetCombatDistance(0, 2) > 1 && game.GetCombatDistance(0, 3) == 1,
            "The first native user has a living far target and an adjacent target.");
        var action = game.GetLegalActions(0).First(candidate => candidate.Kind == LegalActionKind.IronChain &&
            candidate.ConversionSource is null && candidate.TargetSeats.SequenceEqual([2, 3]));
        var ironChain = action.CardId!.Value;
        Submit(new PlayCardCommand(0, ironChain, action.TargetSeats, game.Revision, play.PromptId, action.PlayedCardKind));
        var offer = Reach(prompt => prompt.PlayerSeat == 1 && prompt.SkillPrompt?.SkillId == Chibi);
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == ironChain);
        var window = game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(frame => frame.ParentFrameId == use.Id);
        Need(use.Action is { Type: CardActionType.Use, ActorSeat: 0 } original &&
            original.EffectiveDesignatedTargetSeats.SequenceEqual([2, 3]) && window.Action.ActionId == original.ActionId &&
            game.CardMovements.Count(move => move.CardId == ironChain && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing) == 1 && game.CreateSnapshot(0).Players[0].HandCount > 0,
            "The physical Iron Chain stays on its original finalized-target window while the user retains a Pindian input.");
        var activate = offer.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Submit(new AnswerPromptCommand(1, offer.PromptId, activate.Id, game.Revision));

        var issued = game.Events.Select(entry => entry.Payload).OfType<AnniversaryCuiLiePindianIssuedEvent>().Single();
        var chibi = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == issued.FrameId);
        var pair = game.ResolutionStack.OfType<PindianFrame>().Single();
        var sourcePrompt = Reach(prompt => prompt is { Kind: DecisionKind.SkillModule, PlayerSeat: 1, IsPrivate: true });
        Need(issued.Actor == 0 && issued.ContestId == pair.Id &&
            chibi.AnniversaryCuiLie is { Stage: AnniversaryCuiLieStage.Pindian } receipt &&
            receipt.ParentId == window.Id && receipt.UseId == use.Id && receipt.ActionId == use.Action!.ActionId &&
            receipt.PindianId == pair.Id && pair.ParentFrameId == chibi.Id &&
            pair is { SourceSeat: 1, OpponentSeat: 0, PindianStep: PindianStep.ChooseSourceCard },
            "Accepting Chibi opens the native pair under the original Iron Chain observer frame.");
        var sourceCard = game.CreateSnapshot(1).Players[1].Hand.First().Id;
        var sourceChoice = sourcePrompt.Choices.Single(choice => choice.Cards.SequenceEqual([sourceCard]) &&
            choice.Parameters.GetValueOrDefault("action") == "pindian-source-card");
        Submit(new AnswerPromptCommand(1, sourcePrompt.PromptId, sourceChoice.Id, game.Revision));
        var committed = game.ResolutionStack.OfType<PindianFrame>().Single();
        var opponentPrompt = Reach(prompt => prompt is { Kind: DecisionKind.SkillModule, PlayerSeat: 0, IsPrivate: true });
        Need(committed.Id == pair.Id && committed.ParentFrameId == chibi.Id && committed.SourceCardId == sourceCard &&
            committed.PindianStep == PindianStep.ChooseOpponentCard && !opponentPrompt.ValidCardIds.Contains(ironChain),
            "The real source card stays on the original pair and the opponent chooses from the remaining hand.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) ==
            SnapshotJson.Serialize(game.CreateSnapshot(seat))) && restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack.OfType<PindianFrame>().Single()) == JsonSerializer.Serialize(committed),
            "Cold replay retains the original pair, chosen source input, and private opponent prompt.");
    }

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-chibi-target", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            foreach (var general in Generals.Where(id => id != Hero))
                builder.AddGeneral(new(general, "固定同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "斥避固定牌堆", 2, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 24)
                    .Select(_ => new ContentDeckPhysicalCard("standard:iron_chain", Suit.Club, 8)).ToArray()
            });
            builder.AddMode(new(Mode, "斥避原生拼点", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
