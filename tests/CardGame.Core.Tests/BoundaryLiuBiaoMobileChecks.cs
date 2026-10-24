using CardGame.Core;
using static BoundaryLiuBiaoScenario;

namespace CardGame.Core.Tests;

internal static class BoundaryLiuBiaoMobileChecks
{
    public static void ExtraFactionDrawKeepsSelfAndTargetlessActionsWhileOtherTargetsAreForbiddenCold()
    {
        var (registry, game) = CreateScene();
        var initial = game.CreateSnapshot(0).Players[0].HandCount;
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"); Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Watch);
        var mother = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Zishou);
        Need(game.CreateSnapshot(0).Players[0].HandCount == initial + 4 && game.Events.Select(item => item.Payload).OfType<CardTargetRestrictionGrantedEvent>().All(item => item.Restriction.Source.SkillId != Zishou),
            "Self-preservation's actual four-faction Draw4 is paid before its gain child, without substituting the normal draw or installing the later restriction prematurely.");
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue"); Play(game);
        Need(game.CreateSnapshot(0).Players[0] is { HandCount: 10, HandLimit: 7 } &&
            game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Reason.Value == $"skill-program.{Zishou}.{SkillProgramEffectOp.Draw}") == 4 &&
            game.Events.Select(item => item.Payload).OfType<CardTargetRestrictionGrantedEvent>().Count(item => item.Restriction.Source.SkillId == Zishou && item.Restriction.ParentFrameId == mother.Id) == 1 &&
            game.GetHumanLegalActions().All(action => action.TargetSeats.All(target => target == 0)) &&
            !game.GetHumanLegalActions().Any(action => action.Kind == LegalActionKind.Slash),
            "After cold child return, exactly one whole-turn self-only grant is issued; Draw4 plus the native normal Draw2 and the live +4 hand limit remain distinct.");
        var equipment = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Crossbow).Id;
        UseAction(game, game.GetHumanLegalActions().Single(action => action.CardId == equipment)); Play(game);
        var drawTwo = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.DrawTwo).Id;
        var beforeDrawTwo = game.CreateSnapshot(0).Players[0].HandCount;
        UseAction(game, game.GetHumanLegalActions().Single(action => action.CardId == drawTwo)); Play(game);
        var chain = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.IronChain).Id;
        UseAction(game, game.GetHumanLegalActions().Single(action => action.CardId == chain && action.Kind == LegalActionKind.Recast)); Play(game);
        Need(game.CreateSnapshot(0).Players[0].Equipment.Single().Id == equipment && game.CreateSnapshot(0).Players[0].HandCount == beforeDrawTwo + 1 &&
            game.GetHumanLegalActions().All(action => action.TargetSeats.All(target => target == 0)) &&
            !game.Events.Any(item => item.Payload is DamageAppliedEvent { SourceSeat: 0 }),
            "Real self equipment, implicit-self DrawTwo and targetless recast remain executable; no outgoing damage debt or blanket use prohibition replaces the native target restriction.");
    }

    public static void PreparationUnlimitedSlashSurvivesHandDropAndSeparatePlayPhasesCold()
    {
        var (registry, game) = CreateScene(unlimited: true);
        var grant = game.Events.Select(item => item.Payload).OfType<TurnRuleModifierGrantedEvent>().Single(item => item.Modifier.Source.SkillId == Zongshi).Modifier;
        Need(grant.Query == SkillRuleQuery.SlashLimit && grant.Operation == SkillRuleOperation.Unlimited && grant.Amount == 0,
            "Preparation issues a genuine whole-turn unlimited Slash modifier while the four-card hand exceeds three HP.");
        var view = game.CreateSnapshot(0).Players[0];
        var discard = new[] { view.Hand.Single(card => card.Kind == CardKind.Dodge).Id, view.Hand.First(card => card.Kind == CardKind.Slash).Id };
        Accept(game, new UseProgramSkillCommand(0, Driver, "discard-two", discard, [], game.Revision, Decision(game)!.PromptId)); Play(game);
        Need(game.CreateSnapshot(0).Players[0].HandCount == 2 && game.CreateSnapshot(0).Players[0].Hp == 3,
            "A real native two-card discard makes the owner's hand smaller than current HP after the preparation grant has already been issued.");
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        SlashTwice(game);
        End(game); Reach(game, OptionalZishou); Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "skip"); Play(game);
        Need(game.CreateSnapshot(0).Players[0].HandCount == 2, "The normal native Draw2 supplies two real Slashes while hand count remains below current HP.");
        SlashTwice(game);
        Need(game.Events.Select(item => item.Payload).OfType<TurnRuleModifierGrantedEvent>().Count(item => item.Modifier.Source.SkillId == Zongshi) == 1 &&
            game.Events.Select(item => item.Payload).OfType<PhaseChangedEvent>().Count(item => item.ActorSeat == 0 && item.Phase == TurnPhase.Play) == 2 &&
            game.Events.Select(item => item.Payload).OfType<DamageAppliedEvent>().Count(item => item.SourceSeat == 0) == 4,
            "The real inserted Play returns to a separate normal Play without reissuing the original preparation grant.");
    }

    private static void SlashTwice(GameEngine game)
    {
        for (var index = 0; index < 2; index++)
        {
            var action = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash);
            UseAction(game, action); Play(game);
        }
        Need(game.Events.Select(item => item.Payload).OfType<DamageAppliedEvent>().Count(item => item.SourceSeat == 0) >= 2,
            "Two real Slashes in one phase deal damage while the hand is below HP, proving that the issued unlimited modifier is not a live hand-count condition.");
    }
    private static void Need(bool value, string reason)
    { if (!value) throw new InvalidOperationException(reason); }
}
