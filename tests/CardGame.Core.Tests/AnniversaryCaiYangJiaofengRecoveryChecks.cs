using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;

using static BoundaryLiDianChecks;

internal static class AnniversaryCaiYangJiaofengRecoveryChecks
{
    private const string Hero = "anniversary:cai-yang", Jiaofeng = "anniversary:jiaofeng";
    private const string Peer = "fixture:jiaofeng-peer", Watch = "fixture:jiaofeng-recovery", Deck = "fixture:jiaofeng-deck";
    private const string DrawReason = "skill-program.anniversary:jiaofeng.draw";

    internal static void FirstNativeSlashRecoveryChildReturnsCold()
    {
        const int owner = 0, target = 3;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "cai-yang-jiaofeng-recovery", Hero, 2, 1, 0, 0, 0, [Watch],
            [new(CardKind.Slash, Suit.Spade, 7)], [],
            [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
            [new(Peer) { StartingHand = [] }], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = owner, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, registry);
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];

        Accept(game, new StartGameCommand());
        Settle(game);
        var originalOwner = Player(owner);
        var originalTargetHp = Player(target).Hp;
        var slash = originalOwner.Hand.Single(card => card.Kind == CardKind.Slash);
        Require(originalOwner.MaxHp == 5 && originalOwner.Hp == 2 && Player(target).HandCount == 0,
            "Cai Yang begins with three lost HP and a real Slash against an empty-handed enemy.");
        var action = game.GetLegalActions(owner).Single(candidate => candidate.Kind == LegalActionKind.Slash &&
            candidate.CardId == slash.Id && candidate.ConversionSource is null && candidate.TargetSeats.SequenceEqual([target]));
        Accept(game, new PlayCardCommand(owner, slash.Id, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));
        Reach(game, prompt => prompt is { PlayerSeat: owner, SkillPrompt.SkillId: Watch });

        var started = Facts<CaiYangJiaofengStartedEvent>().Single();
        var issued = Facts<CaiYangJiaofengRecoveryIssuedEvent>().Single();
        var draw = Facts<CaiYangJiaofengDrawReturnedEvent>().Single();
        var stack = game.ResolutionStack.ToArray();
        var parent = stack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == started.FrameId);
        var hp = stack.OfType<HpChangedTriggerWindowFrame>().Single();
        var slashUse = stack.OfType<CardUseFrame>().Single(frame => frame.CardId == slash.Id);
        var paidCard = game.CardMovements.Single(move => move.Reason.Value == DrawReason);
        var hpIndex = Array.IndexOf(stack, hp);
        Require(started.Origin is { Source.OwnerSeat: owner, Source.SkillId: Jiaofeng, Target: target, LostHp: 3, OriginalAmount: 1 } &&
            stack.OfType<BeforeDamageProgramWindowFrame>().Single(frame => frame.Id == started.Origin.WindowId).CaiYangJiaofeng == started.Origin &&
            parent.AnniversaryCaiYang is { Stage: AnniversaryCaiYangStage.RecoveryChildren, RecoveryIssued: true } &&
            draw.FrameId == parent.Id && draw.Count == 1 && paidCard.From == CardLocation.DrawPile && paidCard.To == CardLocation.Hand(owner) &&
            paidCard.Sequence > draw.Before && paidCard.Sequence <= draw.After && Player(owner).Hand.Single().Id == paidCard.CardId &&
            issued == new CaiYangJiaofengRecoveryIssuedEvent(parent.Id, 1) &&
            hpIndex > 0 && stack[hpIndex - 1].Id == parent.Id && hp.Change.ParentFrameId == parent.Id &&
            hp.ResumeFrameId == parent.Id && hp.Continuation == PostEventContinuation.Program &&
            hp.Change is { SourceSeat: owner, TargetSeat: owner, Kind: HpChangeKind.Recovery, Amount: 1, HpBefore: 2, HpAfter: 3 } &&
            Player(owner).Hp == 3 && Player(target).Hp == originalTargetHp && Facts<DamageAppliedEvent>().Length == 0,
            "The first Slash suspends before damage with one real card drawn and a paid one-HP recovery under the original Jiaofeng frame.");

        var promptId = P(game)!.PromptId;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var restoredParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var restoredHp = game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.Id == hp.Id);
        var runtime = game.GetRuntimeModule<CharacterCaiYangAnniversaryRuntime>();
        Require(P(game)?.PromptId == promptId && runtime.CaiYangFirstChild(restoredParent, restoredHp) &&
            !runtime.CaiYangFirstChild(restoredParent, restoredHp with { ResumeFrameId = hp.Id }) &&
            !runtime.CaiYangFirstChild(restoredParent, restoredHp with { Continuation = PostEventContinuation.AwaitedProgramMovement }) &&
            !runtime.CaiYangFirstChild(restoredParent, restoredHp with { Change = restoredHp.Change with { TargetSeat = target } }) &&
            !runtime.CaiYangFirstChild(restoredParent with { AnniversaryCaiYang = restoredParent.AnniversaryCaiYang! with { RecoveryIssued = false } }, restoredHp),
            "Cold restore accepts the original recovery child and rejects wrong links, a foreign recipient, or an unissued recovery.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Settle(game);

        Require(Facts<DamageAppliedEvent>().Single() is { SourceSeat: owner, TargetSeat: target, Amount: 2, SourceLess: false } damage &&
            damage.SourceCardUseResolutionId == slashUse.Id && Player(target).Hp == originalTargetHp - 2 &&
            Facts<CardUseFinishedEvent>().Count(entry => entry.ResolutionId == slashUse.Id && entry.CardId == slash.Id) == 1 &&
            Facts<CaiYangJiaofengDrawReturnedEvent>().Single() == draw && Facts<CaiYangJiaofengRecoveryIssuedEvent>().Single() == issued &&
            Facts<RecoveryAppliedEvent>().Single() == new RecoveryAppliedEvent(owner, owner, 1, 3) &&
            Player(owner).Hp == 3 && Player(owner).Hand.Single().Id == paidCard.CardId,
            "The original physical Slash resumes with damage increased to two, without repeating the draw or recovery.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-jiaofeng-recovery", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,"triggers":[
                  {"id":"actual-recovery","window":"afterHealthChanged","subject":"owner",
                   "condition":{"kind":"healthChangeKindIs","hpChangeKind":"recovery"},"optional":false,"isSkillInvocation":false,
                   "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Watch] = new { name = "观察交锋恢复", description = "暂停在原生恢复子窗。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } }
                }));
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "固定同桌", "", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "交锋固定实体牌堆", 0, 0, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2), 16).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "交锋首伤恢复", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
