using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDuanhunFieldEquipmentChecks
{
    private const string Skill = StandardShanheDuanhunPackage.SkillId;
    private const string Driver = "fixture:duanhun-field-damage", Deck = "fixture:duanhun-field-deck";

    internal static void OnlyOriginalEquippedWeaponBypassesDamageArmorCold()
    {
        var setup = new ShanheBattleSetup(238, 1, "fixture:duanhun-field", "classic:guan-yu", 8, 4, 0, 0, 0,
            [Driver, Skill], [new(CardKind.Duanhun, Suit.Spade, 5), new(CardKind.Crossbow, Suit.Club, 3)], [],
            [new ShanheCombatantDefinition("classic:guan-yu", MaxHpBonus: 4, Hp: 8) { StartingHand = [], InitialArmor = 5 }],
            [], "fixture:duanhun-field");
        var content = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] })
            .WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 2
        }, content);
        Submit(game, new StartGameCommand());
        var play = ReachPlay(game);
        var weapon = Player(0).Hand.Single(card => card.Kind == CardKind.Duanhun);
        var replacement = Player(0).Hand.Single(card => card.Kind == CardKind.Crossbow);
        Need(weapon is { Id: > 0, Suit: Suit.Spade, Rank: 5 } && HasPolicy() && Player(0).Equipment.Count == 0 &&
             Player(1) is { Hp: 8, Armor: 5 },
            "The source holds the official physical Duanhun and a separate same-name grant while the victim has real armor.");
        Need(Hit().ArmorPayment is { ArmorBefore: 5, ArmorAbsorbed: 2, ArmorAfter: 3, HpLossAmount: 0 } &&
             Player(1) is { Hp: 8, Armor: 3 },
            "The hand card and extra skill grant still leave the original two damage subject to armor payment.");

        var checkpoint = GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint()));
        game = GameReplay.Restore(checkpoint, content);
        Need(ReachPlay(game).PromptId == play.PromptId && Player(0).Hand.Any(card => card.Id == weapon.Id) &&
             Player(0).Equipment.Count == 0 && HasPolicy() && Player(1) is { Hp: 8, Armor: 3 },
            "Cold replay restores the pre-equip action with the original hand entity and prior armor payment.");
        Equip(weapon.Id);
        var installed = game.CardMovements.Single(move => move.CardId == weapon.Id &&
            move.From == CardLocation.Processing && move.To == CardLocation.Equipment(0) && move.Reason == CardMoveReasons.EquipmentEnter);
        Need(Player(0).Equipment.Single().Id == weapon.Id && HasPolicy(), "The native Equip action installs the original weapon.");
        Need(Hit().ArmorPayment is null && Player(1) is { Hp: 6, Armor: 3 },
            "With the actual weapon installed, damage from the same source bypasses scalar armor and removes two HP.");

        Equip(replacement.Id);
        var removed = game.CardMovements.Single(move => move.CardId == weapon.Id && move.Sequence > installed.Sequence);
        Need(removed.From == CardLocation.Equipment(0) && removed.To == CardLocation.DiscardPile &&
             removed.Reason == CardMoveReasons.EquipmentReplace && Player(0).Equipment.Single().Id == replacement.Id && HasPolicy(),
            "The replacement moves the original Duanhun out of its slot while the source retains the separate same-name grant.");
        Need(Hit().ArmorPayment is { ArmorBefore: 3, ArmorAbsorbed: 2, ArmorAfter: 1, HpLossAmount: 0 } &&
             Player(1) is { Hp: 6, Armor: 1 },
            "After losing the physical source, the third native two damage resumes the victim's armor payment.");

        PlayerSnapshot Player(int seat) => game.CreateSnapshot(0).Players[seat];
        bool HasPolicy() => Player(0).Skills!.Any(skill => skill.ContentId == Skill);

        DamageAppliedEvent Hit()
        {
            var before = game.Events.Count;
            Submit(game, new UseProgramSkillCommand(0, Driver, "hit", [], [1], game.Revision, play.PromptId));
            play = ReachPlay(game);
            var damage = game.Events.Skip(before).Select(entry => entry.Payload).OfType<DamageAppliedEvent>().Single();
            Need(damage is { SourceSeat: 0, TargetSeat: 1, Amount: 2, SourceLess: false },
                "Each check uses the same original sourced two-point damage action.");
            return damage;
        }

        void Equip(int cardId)
        {
            var action = game.GetHumanLegalActions().Single(candidate =>
                candidate.Kind == LegalActionKind.Equip && candidate.CardId == cardId && candidate.ConversionSource is null);
            Submit(game, new PlayCardCommand(0, cardId, action.TargetSeats, game.Revision, play.PromptId, action.PlayedCardKind, action.TargetCardId));
            play = ReachPlay(game);
        }
    }

    private static PendingDecision ReachPlay(GameEngine game)
    {
        for (var step = 0; step < 60; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { } prompt)
            {
                Need(prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 },
                    $"Unexpected Duanhun fixture prompt {prompt.Kind} for seat {prompt.PlayerSeat}.");
                if (game.ResolutionStack.Count == 0)
                    return prompt;
            }
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The Duanhun fixture did not return to its native play phase.");
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "The Duanhun fixture command was rejected.");
    }

    private static void Need(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:duanhun-field", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[{"id":"hit","minCards":0,"maxCards":0,"minTargets":1,
                    "maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,
                    "effects":[{"op":"damage","target":"selectedTarget","amount":2}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "真实两点伤害", description = "对目标造成两点伤害。" } }
            });
            var programs = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Driver, "真实两点伤害", "对目标造成两点伤害。")
                { Program = programs.Programs[Driver], ProgramPresentation = programs.Presentations[Driver] });
            builder.AddDeck(new(Deck, "无自然起手的牌堆", 0, 0, [new("standard:dodge", 32)]));
        }
    }
}
