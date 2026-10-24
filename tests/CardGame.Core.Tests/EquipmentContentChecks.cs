using CardGame.Core;

internal static class EquipmentContentChecks
{
    public static void OptionalPackagesAndGeneratedDependencies()
    {
        var standard = EquipmentContentRegistry.Create(BuiltInEquipmentContent.Standard);
        Require(standard.IsEquipment(CardKind.QinggangSword) && !standard.IsEquipment(CardKind.WushuangHalberd) &&
            !standard.IsEquipment(CardKind.ShuraInfernoHalberd) && !standard.IsEquipment(CardKind.GeneralWeapon),
            "Uninstalled activity and generated-equipment packages do not enter the standard equipment registry.");
        var activity = EquipmentContentRegistry.Create(BuiltInEquipmentContent.Standard, BuiltInEquipmentContent.Hulao);
        Require(activity.IsEquipment(CardKind.WushuangHalberd) && !activity.IsEquipment(CardKind.ShuraInfernoHalberd) &&
            activity.GetSkillBinding(CardKind.WushuangHalberd) is
                { SkillId: "hulao:wushuang-halberd", GrantPrefix: "equipment:hulao" } binding &&
            binding.CreateGrantId(17) == "equipment:hulao:17",
            "Explicit installation supplies only the selected definitions and the original equipment grant identities.");

        var builder = new EquipmentContentBuilder().Add(BuiltInEquipmentContent.Standard);
        var frozen = builder.Build();
        var local = new EquipmentDefinition(CardKind.Xuanjian, "本局玄剑", EquipmentSlot.Weapon, "本局注册的装备。", WeaponAttackRange: 6);
        builder.Add(new("test:local-equipment", [local]));
        Require(!frozen.IsEquipment(CardKind.Xuanjian) && builder.Build().Get(CardKind.Xuanjian) == local &&
            EquipmentCatalog.Get(CardKind.Xuanjian).WeaponAttackRange == 3,
            "A local registration neither rewrites a previously frozen match nor changes legacy global definitions.");

        var emptyRegistry = ContentRegistry.Build();
        var mode = new ContentModeDefinition("test:equipment-mode", "测试身份场", 8, 8,
            new Dictionary<string, int>());
        var ordinary = BuiltInEquipmentContent.CreateForMatch(mode, emptyRegistry);
        Require(!ordinary.IsEquipment(CardKind.WushuangHalberd) && !ordinary.IsEquipment(CardKind.GeneralWeapon),
            "Match assembly does not load unrelated special content in an otherwise ordinary table.");
        var generatedRegistry = ContentRegistry.Build(new GeneratedEquipmentDependencies());
        var generated = BuiltInEquipmentContent.CreateForMatch(mode, generatedRegistry);
        Require(generated.IsEquipment(CardKind.GeneralWeapon) && generated.IsEquipment(CardKind.Xuanjian) &&
            !generatedRegistry.Cards.Values.Any(card => card.LegacyKind is CardKind.GeneralWeapon or CardKind.Xuanjian) &&
            !generated.IsEquipment(CardKind.WushuangHalberd),
            "Native generation and named-weapon program outputs remain available even when the weapons have no deck entry.");

        var all = EquipmentContentRegistry.Create(BuiltInEquipmentContent.All);
        Require(EquipmentCatalog.Implemented.SequenceEqual(all.Implemented) &&
            all.Implemented.SequenceEqual(all.Implemented.OrderBy(definition => definition.Slot).ThenBy(definition => definition.Kind)),
            "The compatibility catalogue keeps the complete existing definitions and their slot/kind ordering.");
    }

    public static void PurchasedEquipmentLifetimeUsesExactEntitySource()
    {
        var registry = EquipmentContentRegistry.Create(BuiltInEquipmentContent.Standard, BuiltInEquipmentContent.WushuangPurchases);
        var deckCard = new Card(100, CardKind.QinggangSword, Suit.Spade, 6);
        var purchasedCard = new Card(101, CardKind.QinggangSword, Suit.Spade, 6);
        var origin = new EquipmentCardOrigin(purchasedCard.Id, BuiltInEquipmentContent.WushuangShopSource, 0);
        var from = CardLocation.Equipment(0);
        var to = CardLocation.DiscardPile;
        Require(registry.NormalizeDestination(new(deckCard, from, to)) == to &&
            registry.NormalizeDestination(new(purchasedCard, from, to, origin, HasPriorMovement: true)) == CardLocation.OutsideGame,
            "The same weapon kind discards normally from the deck and is destroyed only for its purchased physical entity.");
        Require(registry.NormalizeDestination(new(purchasedCard, CardLocation.OutsideGame, from, origin)) == from &&
            registry.NormalizeDestination(new(purchasedCard, CardLocation.OutsideGame, CardLocation.Equipment(1), origin)) == CardLocation.OutsideGame &&
            registry.NormalizeDestination(new(purchasedCard, from, CardLocation.Equipment(1), origin, HasPriorMovement: true)) == CardLocation.OutsideGame &&
            registry.NormalizeDestination(new(purchasedCard, CardLocation.OutsideGame, from, origin,
                HasPriorMovement: true, AlreadyDestroyed: true)) == CardLocation.OutsideGame,
            "Only the initial owner receives the fresh purchase; transfer and later resurrection cannot bypass its lifetime.");
        var standard = EquipmentContentRegistry.Create(BuiltInEquipmentContent.Standard);
        Require(standard.NormalizeDestination(new(purchasedCard, from, to, origin)) == to,
            "A source policy does not activate in a registry where its optional feature has not been installed.");
        try
        {
            registry.NormalizeDestination(new(deckCard, from, to, origin));
            throw new InvalidOperationException("Mismatched equipment provenance was accepted.");
        }
        catch (InvalidOperationException failure) when (failure.Message.Contains("creation identity", StringComparison.Ordinal)) { }
    }

    public static void ExplicitMatchDefinitionsOverrideOnlyLegacyFallback()
    {
        var customRegistry = ContentRegistry.Build(new RuntimeEquipmentFixture(explicitEquipment: true));
        var customRules = BuiltInEquipmentContent.CreateForMatch(customRegistry.Modes[RuntimeEquipmentFixture.ModeId],
            customRegistry, additionalPackages: customRegistry.EquipmentPackages);
        Require(customRules.Get(CardKind.Xuanjian).WeaponAttackRange == 6,
            "A production-registered definition is authoritative over the legacy generated-equipment fallback.");

        var custom = StartAndEquip(customRegistry);
        Require(custom.GetAttackRange(0) == 6 && custom.CreateSnapshot(0).Players[0].Equipment.Single().Kind == CardKind.Xuanjian &&
            EquipmentCatalog.Get(CardKind.Xuanjian).WeaponAttackRange == 3,
            "The real identity engine equips the locally defined range-six weapon without changing the compatibility catalogue.");
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(custom.CreateCheckpoint())), customRegistry);
        Require(restored.GetAttackRange(0) == 6 && restored.Revision == custom.Revision &&
            restored.CreateSnapshot(0).Players[0].Equipment.Single().Id == custom.CreateSnapshot(0).Players[0].Equipment.Single().Id,
            "Cold command replay reconstructs the same local equipment rules and physical weapon.");

        var ordinaryRegistry = ContentRegistry.Build(new RuntimeEquipmentFixture(explicitEquipment: false));
        var ordinary = StartAndEquip(ordinaryRegistry);
        Require(ordinary.GetAttackRange(0) == 3 && EquipmentCatalog.Get(CardKind.Xuanjian).WeaponAttackRange == 3 &&
            custom.GetAttackRange(0) == 6,
            "A second ordinary match retains the original range and cannot contaminate the explicitly configured match.");

        var conflicting = new EquipmentContentPackage("test:conflicting-equipment",
            [customRules.Get(CardKind.Xuanjian) with { WeaponAttackRange = 7 }]);
        try
        {
            ContentRegistry.Build(new RuntimeEquipmentFixture(explicitEquipment: true), new ExtraEquipmentFixture(conflicting));
            throw new InvalidOperationException("Conflicting explicit equipment registrations were accepted.");
        }
        catch (InvalidOperationException failure) when (failure.Message.Contains("Conflicting equipment definition", StringComparison.Ordinal)) { }
    }

    private static GameEngine StartAndEquip(ContentRegistry registry)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord,
            ModeId = RuntimeEquipmentFixture.ModeId, AdvanceAfterHumanCommands = true,
            UseInteractiveSetup = false, UseInteractiveDiscard = false
        }, registry);
        Require(game.Submit(new StartGameCommand(game.Revision)).Accepted, "The identity fixture must start through the real command session.");
        for (var step = 0; step < 20 && !game.GetHumanLegalActions().Any(action => action.Kind == LegalActionKind.Equip); step++)
            Require(game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted, "The identity fixture must reach its first real play phase.");
        var equip = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Equip);
        Require(game.Submit(new PlayCardCommand(0, equip.CardId!.Value, equip.TargetSeats,
            game.Revision, game.PendingDecision!.PromptId, equip.PlayedCardKind)).Accepted,
            "The selected physical weapon must equip through native payment and movement.");
        return game;
    }

    private sealed class RuntimeEquipmentFixture(bool explicitEquipment) : IGameContentPackage
    {
        internal const string ModeId = "test:equipment-runtime-identity";
        public PackageManifest Manifest { get; } = new("test:equipment-runtime", new Version(1, 0, 0), []);

        public void Register(IContentRegistryBuilder builder)
        {
            var weapon = EquipmentCatalog.Get(CardKind.Xuanjian);
            builder.AddCard(new("test:runtime-xuanjian", weapon.DisplayName, "装备牌", weapon.Description, CardKind.Xuanjian));
            builder.AddDeck(new("test:runtime-equipment-deck", "装备运行测试牌堆", 4, 2,
                [new("test:runtime-xuanjian", 40)]));
            string[] generals = ["test:equipment-lord", "test:equipment-loyalist", "test:equipment-rebel", "test:equipment-renegade"];
            foreach (var id in generals) builder.AddGeneral(new(id, id, id, null, BaseHp: 4));
            builder.AddMode(new(ModeId, "装备运行测试身份场", 4, 4,
                new Dictionary<string, int> { ["Lord"] = 1, ["Loyalist"] = 1, ["Rebel"] = 1, ["Renegade"] = 1 },
                "test:runtime-equipment-deck", GeneralPoolIds: generals));
            if (explicitEquipment) builder.AddEquipmentContent(new("test:runtime-equipment-rules",
                [weapon with { WeaponAttackRange = 6 }]));
        }
    }

    private sealed class ExtraEquipmentFixture(EquipmentContentPackage equipment) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("test:extra-equipment-runtime", new Version(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder) => builder.AddEquipmentContent(equipment);
    }

    private sealed class GeneratedEquipmentDependencies : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("test:generated-equipment", new Version(1, 0, 0), []);

        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$$"""
                {"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[{
                  "id":"test:equipment-generator","revision":1,"triggers":[{
                    "id":"generate","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
                    "effects":[{"op":"equipSampledGenerals","target":"owner","amount":1},
                               {"op":"placeNamedWeapon","target":"owner","outputKind":"xuanjian"}]
                  }]}]}
                """, $$$$"""
                {"schemaVersion":{{{{SkillProgramCatalog.PresentationSchemaVersion}}}},"skills":{
                  "test:equipment-generator":{"name":"生成装备","description":"测试隐式与显式装备依赖。"}}}
                """);
            builder.AddSkill(new("test:equipment-generator", "生成装备", "测试隐式与显式装备依赖。")
                { Program = catalog.Programs["test:equipment-generator"], ProgramPresentation = catalog.Presentations["test:equipment-generator"] });
        }
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
