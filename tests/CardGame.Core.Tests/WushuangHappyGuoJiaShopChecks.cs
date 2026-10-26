using System.Text.Json;
using CardGame.Core;

internal static partial class WushuangModeChecks
{
    private const string HappyGjShopDriver = "fixture:wushuang-happy-guojia-driver";
    private const string HappyGjShopWatch = "fixture:wushuang-happy-guojia-watch";

    public static void HappyZuoxingForeignRecipientPaysNativeFixedSevenMaximumCold()
    {
        // Historical fixed-seven mechanism: the current production pools no
        // longer offer 2020. A test-only configured opening runs its original
        // paid acquisition and parent return, including during cold replay.
        var (game, registry) = Start(false, 33, skills: ["classic:qingguo", "classic:lianying"],
            initialHp: 20, materialCase: "happy-zuoxing", legacyOpeningTactic: 2020);
        Play(game);
        Require(E<WushuangTacticAcquiredEvent>(game).Count(e => e.Seat == 0 && e.TacticId == 2020 && e.IsOpening) == 1 &&
                game.CreateSnapshot(0).Players[0].MaxHp == 7,
            "The configured historical opening uses the real native 2020 acquisition, rather than a lookalike maximum-HP modifier.");
        void Use(string skill, string activation, params int[] targets) =>
            Accept(game, new UseProgramSkillCommand(0, skill, activation, [], targets, game.Revision, P(game)!.PromptId));
        void Answer(Func<PromptChoice, bool> select)
        {
            var p = P(game)!;
            Accept(game, new AnswerPromptCommand(0, p.PromptId, p.Choices.Single(select).Id, game.Revision));
        }
        Use(Driver, "acquire-material-flow"); Play(game);
        Use(HappyGjShopDriver, "enable"); Play(game);
        Use(HappyGjShopDriver, "hurt-self"); Play(game);
        Use(HappyGjShopDriver, "hurt-opponent", 1); Play(game);
        End(game);
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == "happy:tianyi" &&
            P(game)!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
        Answer(c => c.Targets.SequenceEqual([1])); Play(game);
        var gift = E<HappyGuoJiaBenefactorIssuedEvent>(game).Single(e => e.Source.OwnerSeat == 0 && e.Recipient == 1);
        Require(game.CreateSnapshot(1).Players[1].Skills!.Any(s => s.ContentId == "happy:zuoxing") && !string.IsNullOrWhiteSpace(gift.GrantedInstance),
            "Real Tianyi grants the foreign recipient the exact Zuoxing instance bound to the original payer.");
        End(game);
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == HappyGjShopWatch);
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.HappyGuoJia is { Stage: HappyGuoJiaStage.NativeUse, Plan.Kind: CardKind.DrawTwo });
        var receipt = frame.HappyGuoJia!;
        var native = game.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == receipt.UseId);
        var payment = E<ShenGuoJiaMaximumPaidEvent>(game).Single(e => e.FrameId == frame.Id);
        var hand = game.CreateSnapshot(1).Players[1].HandCount;
        Require(frame.OwnerSeat == 1 && frame.SkillInstanceId == gift.GrantedInstance && receipt.Benefactor == 0 &&
                payment is { Index: 0, Seat: 0, Before: 7, After: 7, Purpose: "happy-zuoxing" } &&
                receipt.MaximumBefore == payment.Before && receipt.MaximumAfter == payment.After &&
                native.VirtualOrdinaryTrickOrigin?.ParentProgramFrameId == frame.Id &&
                native.Action is { ActorSeat: 1, ProviderSeat: 1, PhysicalCards.Count: 0 } && native.CardId == 0 && native.PhysicalCardIds.Count == 0,
            "The foreign zero-entity DrawTwo owns one normalized seven-to-seven payment on its original benefactor and parent.");
        Cold(game, registry);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        Drive(game, () => E<ShenGuoJiaNativeReturnedEvent>(game).Any(e => e.FrameId == frame.Id));
        Require(game.CreateSnapshot(0).Players[0].MaxHp == 7 && game.CreateSnapshot(1).Players[1].HandCount == hand + 2 &&
                E<ShenGuoJiaMaximumPaidEvent>(game).Count(e => e.FrameId == frame.Id) == 1 &&
                E<ShenGuoJiaNativeIssuedEvent>(game).Count(e => e.FrameId == frame.Id && e.UseId == native.Id) == 1 &&
                E<ShenGuoJiaNativeReturnedEvent>(game).Count(e => e.FrameId == frame.Id && e.UseId == native.Id) == 1 &&
                E<CardUseFinishedEvent>(game).Count(e => e.ResolutionId == native.Id && e.CardId == 0 && e.CardKind == CardKind.DrawTwo) == 1 &&
                E<ShenGuoJiaCompletedEvent>(game).Count(e => e.FrameId == frame.Id) == 1 &&
                !game.ResolutionStack.Any(f => f.Id == frame.Id),
            "Cold completes the original DrawTwo and parent once with no repeated maximum payment or invented material.");
        Cold(game, registry);
    }

    private static void RegisterHappyGjShopFixture(IContentRegistryBuilder builder)
    {
        var catalog = SkillProgramCatalog.Load($$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
          {"id":"{{HappyGjShopDriver}}","revision":1,"activations":[
            {"id":"enable","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"grantSkills","target":"owner","skillIds":["happy:tianyi","{{HappyGjShopWatch}}"]}]},
            {"id":"hurt-self","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"damage","target":"owner","amount":1}]},
            {"id":"hurt-opponent","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]}]},
          {"id":"{{HappyGjShopWatch}}","revision":1,"triggers":[{"id":"foreign-native-draw","window":"cardUseCommitted","ownerRelation":"observer","cardKinds":["drawTwo"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}
        ]}
        """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> {
                [HappyGjShopDriver] = new { name = "真实佐幸驱动", description = "真实损伤和觉醒受赠流程。", optionLabels = new Dictionary<string, string>() },
                [HappyGjShopWatch] = new { name = "佐幸原生摸牌子窗", description = "原受赠者使用锦囊后的真实提交窗口。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
        foreach (var (id, program) in catalog.Programs)
            builder.AddSkill(new(id, id, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
    }
}
