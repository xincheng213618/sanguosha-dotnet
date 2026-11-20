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
        bool Watching(PendingDecision prompt) => prompt is { PlayerSeat: owner, SkillPrompt.SkillId: Watch };
        void Continue() => Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");

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
        Reach(game, prompt => Watching(prompt) && game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Any());

        var started = Facts<CaiYangJiaofengStartedEvent>().Single();
        var drawing = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == started.FrameId);
        var attempt = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single();
        var runtime = game.GetRuntimeModule<CharacterCaiYangAnniversaryRuntime>();
        Require(drawing.AnniversaryCaiYang is { Stage: AnniversaryCaiYangStage.Drawing } &&
            attempt.Context is { RecipientSeat: owner, RequestedCount: 1 } && attempt.Context.ParentFrameId == drawing.Id &&
            attempt.Context.Reason.Value == DrawReason && runtime.CaiYangFirstChild(drawing, attempt) &&
            !runtime.CaiYangFirstChild(drawing, attempt with { Context = attempt.Context with { RequestedCount = 2 } }) &&
            Facts<CaiYangJiaofengDrawReturnedEvent>().Length == 0,
            "The first real Slash pauses before the original Jiaofeng frame's one-card native draw.");
        Continue();

        Reach(game, prompt => Watching(prompt) && game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>()
            .Any(frame => frame.Batch.ParentFrameId == drawing.Id));
        var gainPromptId = P(game)!.PromptId;
        var gainParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == drawing.Id);
        var gain = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.ParentFrameId == drawing.Id);
        var receipt = gainParent.NativeOperationDrawReturns.Single();
        var paidCard = game.CardMovements.Single(move => move.Reason.Value == DrawReason);
        var draw = Facts<CaiYangJiaofengDrawReturnedEvent>().Single();
        Require(gainParent.AnniversaryCaiYang is { Stage: AnniversaryCaiYangStage.DrawChildren, RecoveryIssued: false } &&
            gainParent.NativeDraw is null && receipt.InstructionIndex == drawing.InstructionIndex &&
            receipt.Operation == SkillProgramEffectOp.ResolveAnniversaryCaiYangJiaofeng && receipt.Reason.Value == DrawReason &&
            receipt.Outcome is { RecipientSeat: owner, RequestedCount: 1, Replacement: null } outcome && outcome.AttemptFrameId == attempt.Id &&
            outcome.ActualDrawIds.SequenceEqual([paidCard.CardId]) && outcome.SequenceBefore == draw.Before && outcome.SequenceAfter == draw.After &&
            Facts<CaiYangJiaofengRecoveryIssuedEvent>().Length == 0 && Facts<DamageAppliedEvent>().Length == 0 && Player(owner).Hp == 2,
            "The physical card and shared receipt are present before the gain observer returns, with recovery and original damage still pending.");
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var restoredGainParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == drawing.Id);
        var restoredGain = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Id == gain.Id);
        runtime = game.GetRuntimeModule<CharacterCaiYangAnniversaryRuntime>();
        Require(P(game)?.PromptId == gainPromptId && JsonSerializer.Serialize(restoredGainParent.NativeOperationDrawReturns.Single()) == JsonSerializer.Serialize(receipt) &&
            runtime.CaiYangFirstChild(restoredGainParent, restoredGain) &&
            !runtime.CaiYangFirstChild(restoredGainParent with { NativeOperationDrawReturns = [] }, restoredGain),
            "Cold restore returns to the original gain child, which requires its delivered draw receipt.");
        Continue();

        Reach(game, prompt => Watching(prompt) && game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Any());
        var issued = Facts<CaiYangJiaofengRecoveryIssuedEvent>().Single();
        var stack = game.ResolutionStack.ToArray();
        var parent = stack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == started.FrameId);
        var hp = stack.OfType<HpChangedTriggerWindowFrame>().Single();
        var slashUse = stack.OfType<CardUseFrame>().Single(frame => frame.CardId == slash.Id);
        var recovery = parent.RecoveryApplications?.SingleOrDefault();
        var hpIndex = Array.IndexOf(stack, hp);
        Require(started.Origin is { Source.OwnerSeat: owner, Source.SkillId: Jiaofeng, Target: target, LostHp: 3, OriginalAmount: 1 } &&
            stack.OfType<BeforeDamageProgramWindowFrame>().Single(frame => frame.Id == started.Origin.WindowId).CaiYangJiaofeng == started.Origin &&
            parent.AnniversaryCaiYang is { Stage: AnniversaryCaiYangStage.RecoveryChildren, RecoveryIssued: true } &&
            draw.FrameId == parent.Id && draw.Count == 1 && paidCard.From == CardLocation.DrawPile && paidCard.To == CardLocation.Hand(owner) &&
            draw.AttemptId == attempt.Id && JsonSerializer.Serialize(parent.NativeOperationDrawReturns.Single()) == JsonSerializer.Serialize(receipt) &&
            paidCard.Sequence > draw.Before && paidCard.Sequence <= draw.After && Player(owner).Hand.Single().Id == paidCard.CardId &&
            issued == new CaiYangJiaofengRecoveryIssuedEvent(parent.Id, 1) &&
            recovery is { SourceSeat: owner, TargetSeat: owner, RequestedAmount: 1, HpBefore: 2, HpAfter: 3 } &&
            recovery.HpChangeId == hp.Change.Id && recovery.InstructionIndex == parent.InstructionIndex && recovery.GameplayHash == parent.GameplayHash &&
            Facts<ProgramRecoveryAppliedEvent>().Single() == new ProgramRecoveryAppliedEvent(parent.Id, recovery) &&
            hpIndex > 0 && stack[hpIndex - 1].Id == parent.Id && hp.Change.ParentFrameId == parent.Id &&
            hp.ResumeFrameId == parent.Id && hp.Continuation == PostEventContinuation.Program &&
            hp.Change is { SourceSeat: owner, TargetSeat: owner, Kind: HpChangeKind.Recovery, Amount: 1, HpBefore: 2, HpAfter: 3 } &&
            Player(owner).Hp == 3 && Player(target).Hp == originalTargetHp && Facts<DamageAppliedEvent>().Length == 0,
            "The first Slash suspends before damage with one real card drawn and a paid one-HP recovery under the original Jiaofeng frame.");

        var promptId = P(game)!.PromptId;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var restoredParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var restoredHp = game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.Id == hp.Id);
        runtime = game.GetRuntimeModule<CharacterCaiYangAnniversaryRuntime>();
        Require(P(game)?.PromptId == promptId && runtime.CaiYangFirstChild(restoredParent, restoredHp) &&
            restoredParent.RecoveryApplications?.SingleOrDefault() == recovery &&
            JsonSerializer.Serialize(restoredParent.NativeOperationDrawReturns.Single()) == JsonSerializer.Serialize(receipt) &&
            !runtime.CaiYangFirstChild(restoredParent, restoredHp with { ResumeFrameId = hp.Id }) &&
            !runtime.CaiYangFirstChild(restoredParent, restoredHp with { Continuation = PostEventContinuation.AwaitedProgramMovement }) &&
            !runtime.CaiYangFirstChild(restoredParent, restoredHp with { Change = restoredHp.Change with { TargetSeat = target } }) &&
            !runtime.CaiYangFirstChild(restoredParent with { AnniversaryCaiYang = restoredParent.AnniversaryCaiYang! with { RecoveryIssued = false } }, restoredHp),
            "Cold restore accepts the original recovery child and rejects wrong links, a foreign recipient, or an unissued recovery.");
        Continue();
        Settle(game);

        Require(Facts<DamageAppliedEvent>().Single() is { SourceSeat: owner, TargetSeat: target, Amount: 2, SourceLess: false } damage &&
            damage.SourceCardUseResolutionId == slashUse.Id && Player(target).Hp == originalTargetHp - 2 &&
            Facts<CardUseFinishedEvent>().Count(entry => entry.ResolutionId == slashUse.Id && entry.CardId == slash.Id) == 1 &&
            Facts<CaiYangJiaofengDrawReturnedEvent>().Single() == draw && Facts<CaiYangJiaofengRecoveryIssuedEvent>().Single() == issued &&
            Facts<ProgramRecoveryAppliedEvent>().Single().Receipt == recovery && game.CardMovements.Single(move => move.Reason.Value == DrawReason) == paidCard &&
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
                  {"id":"actual-draw","window":"nativeDrawStarting","subject":"owner","condition":{"kind":"phaseIsPlay"},
                   "optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen",
                    "options":[{"id":"continue"}]}]},
                  {"id":"actual-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                   "movementOccurrence":"perBatch","movementReasons":["{{DrawReason}}"],"optional":false,"isSkillInvocation":false,
                   "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]},
                  {"id":"actual-recovery","window":"afterHealthChanged","subject":"owner",
                   "condition":{"kind":"healthChangeKindIs","hpChangeKind":"recovery"},"optional":false,"isSkillInvocation":false,
                   "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Watch] = new { name = "观察交锋结算", description = "在摸牌请求、获得牌和回复体力时暂停。",
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
