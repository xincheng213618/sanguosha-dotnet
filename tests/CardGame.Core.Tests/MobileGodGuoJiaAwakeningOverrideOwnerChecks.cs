using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileGodGuoJiaAwakeningOverrideOwnerChecks
{
    private const string Huishi = "mobile:huishi-gift", Baijia = "ol:baijia", Guju = "ol:guju", Canshi = "ol:canshi";
    private const string Dujiang = "mou-mobile:dujiang", Duojing = "mou-mobile:duojing";
    private const string Fanxiang = "ol:fanxiang", Liangzhu = "ol:liangzhu", Xiaoji = "classic:xiaoji";
    private const string Moucuan = "ol:moucuan", Binghuo = "ol:binghuo";
    private const string Jianghuo = "anniversary:jianghuo", Lishi = "anniversary:lishi";
    private const string Diaoling = "ol:diaoling";
    private const string Choujue = "ol:choujue", Beishui = "ol:beishui";
    private const string OwnerGeneral = "fixture:huishi-baijia-owner", Watch = "fixture:huishi-baijia-maximum";
    private const string Mode = "identity:classic-huishi-baijia", Deck = "fixture:huishi-baijia-deck";
    private const string WatchMaximum = """
        {"id":"maximum-paid","window":"afterHealthChanged","subject":"owner",
         "condition":{"kind":"healthChangeKindIs","hpChangeKind":"maximumHp"},"optional":false,"isSkillInvocation":false,
         "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}
        """;
    private const string WatchSkillGrant = """
        {"id":"actual-grant-child","window":"skillsChanged","subject":"owner",
         "usageScope":"game","usageLimit":1,"optional":false,"isSkillInvocation":false,
         "effects":[{"op":"chooseOption","target":"owner","resultBind":"grant","options":[{"id":"continue"}]}]}
        """;

    private static GameEngine CreateMatch(ContentRegistry registry) =>
        GameEngine.CreateStandard(new GameOptions
        {
            Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, ConfirmOpeningHand = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry);

    internal static void PaidHuishiWaivesBaijiaThresholdForNextPreparationCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture("ol:bei-mi-hu", [Baijia, Guju], WatchMaximum));
        var match = new HuishiMatch(registry);
        match.Start();
        var firstTurn = match.Game.State.TurnNumber;
        var initialMaximum = match.Owner.MaxHp;
        match.Need(initialMaximum >= match.Game.PlayerCount && match.Owner.Skills?.Any(skill => skill.ContentId == Baijia) == true &&
            match.Facts<BeiMiHuAwakeningMaximumPaidEvent>().Length == 0 && match.Facts<BeiMiHuGujuDrawPaidEvent>().Length == 0,
            "The original first Preparation has zero Guju cards and cannot awaken Baijia on its own.");

        match.GrantToSelf(Baijia);
        var granted = match.Facts<ShenGuoJiaAwakeningOverrideEvent>().Single();
        var expense = match.Facts<ShenGuoJiaMaximumPaidEvent>().Single();
        var reducedMaximum = initialMaximum - 2;
        match.Need(granted is { Recipient: 0, SkillId: Baijia, Source: { OwnerSeat: 0, SkillId: Huishi, BindingId: "grant-awakening" } } &&
            granted.FrameId == expense.FrameId && expense is { Seat: 0, Purpose: "huishi-gift" } &&
            expense.Before == initialMaximum && expense.After == reducedMaximum && match.Owner.MaxHp == reducedMaximum &&
            match.Facts<MaximumHpChangedEvent>().Count(fact => fact is { PlayerSeat: 0, Delta: -2, SkillId: Huishi } && fact.MaximumHp == reducedMaximum) == 1 &&
            match.Facts<BeiMiHuAwakeningMaximumPaidEvent>().Length == 0,
            "The original self-targeted Huishi pays precisely two maximum HP and grants the chosen Baijia instance for later Preparation.");

        match.EndPlay();
        match.Reach(() => match.Prompt?.SkillPrompt?.SkillId == Watch && match.Facts<BeiMiHuAwakeningMaximumPaidEvent>().Length == 1);
        var paid = match.Facts<BeiMiHuAwakeningMaximumPaidEvent>().Single();
        var awaken = match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paid.FrameId);
        var health = match.Game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.ResumeFrameId == paid.FrameId);
        var healthBinding = match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Watch);
        match.Need(match.Facts<TurnStartedEvent>().Where(fact => fact.ActorSeat == 0).Select(fact => fact.TurnNumber).SequenceEqual([firstTurn, paid.Turn]) &&
            awaken.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, OwnerSeat: 0 } preparation &&
            preparation.ParentFrameId == paid.ParentFrameId && awaken.BeiMiHu is { Stage: "maximum-children" } &&
            paid.CumulativeActualCards == 0 && match.Facts<BeiMiHuGujuDrawPaidEvent>().Length == 0 &&
            paid.Source.SkillInstanceId == granted.InstanceId && paid.GameplayHash == granted.GameplayHash && granted.FrameId < paid.FrameId &&
            paid.MaximumBefore == reducedMaximum && paid.MaximumAfter == reducedMaximum + 1 &&
            health.Change is { Kind: HpChangeKind.MaximumHp, TargetSeat: 0, Amount: 1 } && health.Change.ParentFrameId == paid.FrameId &&
            healthBinding.WindowContext is { Window: SkillProgramTriggerWindow.AfterHealthChanged } change && change.HpChange == health.Change &&
            match.Facts<BeiMiHuAwakeningCompletedEvent>().Length == 0,
            "The exact grant waives only the natural seven-card threshold. The next actual Preparation pays original Baijia and awaits its native maximum-HP child.");

        match.Restore();
        match.Need(match.Facts<ShenGuoJiaAwakeningOverrideEvent>().SequenceEqual([granted]) &&
            match.Facts<BeiMiHuAwakeningMaximumPaidEvent>().SequenceEqual([paid]) &&
            JsonSerializer.Serialize(match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paid.FrameId)) == JsonSerializer.Serialize(awaken) &&
            JsonSerializer.Serialize(match.Game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.Id == health.Id)) == JsonSerializer.Serialize(health),
            "Cold replay retains both original facts and the exact paid awakening waiting for its original health child.");

        match.Continue();
        match.ReachPlay();
        var completed = match.Facts<BeiMiHuAwakeningCompletedEvent>().Single();
        match.Need(completed is { OwnerSeat: 0, LostSkillId: Guju, GrantedSkillId: Canshi } && completed.FrameId == paid.FrameId &&
            match.Facts<BeiMiHuAwakeningMaximumPaidEvent>().SequenceEqual([paid]) && match.Facts<ShenGuoJiaMaximumPaidEvent>().SequenceEqual([expense]) &&
            match.Owner.MaxHp == reducedMaximum + 1 && match.Owner.Skills?.Any(skill => skill.ContentId == Canshi) == true &&
            match.Owner.Skills?.All(skill => skill.ContentId != Guju) == true,
            "Returning that child completes the original awakening once with its normal skill replacement and no second expense.");
    }

    internal static void PaidHuishiWaivesDujiangArmorForNextPreparationCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture("mou:lu-meng-mobile", [Dujiang], WatchSkillGrant));
        var match = new HuishiMatch(registry);
        match.Start();
        var firstTurn = match.Game.State.TurnNumber;
        var initialMaximum = match.Owner.MaxHp;
        match.Need(match.Owner.Armor == 0 && match.Facts<MouLuMengDujiangAwakenedEvent>().Length == 0 &&
            match.Owner.Skills?.Any(skill => skill.ContentId == Dujiang) == true, "The first real Preparation offers no natural Dujiang.");

        match.GrantToSelf(Dujiang);
        var grant = match.Facts<ShenGuoJiaAwakeningOverrideEvent>().Single();
        match.Need(grant is { Recipient: 0, SkillId: Dujiang, Source: { OwnerSeat: 0, SkillId: Huishi, BindingId: "grant-awakening" } } &&
            match.Owner.MaxHp == initialMaximum - 2 && match.Owner.Armor == 0 &&
            match.Facts<ShenGuoJiaMaximumPaidEvent>().Single().FrameId == grant.FrameId &&
            match.Facts<MouLuMengDujiangAwakenedEvent>().Length == 0, "Paid Huishi grants the exact Dujiang instance without manufacturing Armor.");

        match.EndPlay();
        match.Reach(() => match.Prompt?.SkillPrompt?.SkillId == Watch);
        var awakened = match.Facts<MouLuMengDujiangAwakenedEvent>().Single();
        var root = match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == awakened.FrameId);
        var watcher = match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Watch);
        match.Need(awakened is { OwnerSeat: 0, Armor: 0, Phase: { Kind: ActualDiscardRecoveryPhaseKind.Preparation, ActorSeat: 0, ActualTurnOwnerSeat: 0 } } &&
            match.Facts<TurnStartedEvent>().Where(fact => fact.ActorSeat == 0).Select(fact => fact.TurnNumber).SequenceEqual([firstTurn, awakened.Phase.TurnNumber]) &&
            match.Facts<ActualDiscardRecoveryPhaseStartedEvent>().Any(fact => fact.Phase == awakened.Phase) &&
            grant.FrameId < root.Id && awakened.Source.SkillInstanceId == grant.InstanceId && awakened.GameplayHash == grant.GameplayHash &&
            root.MouLuMengDujiang == new MouLuMengDujiangReceipt(awakened.Source, awakened.GameplayHash, awakened.Phase) &&
            watcher.WindowContext is { Window: SkillProgramTriggerWindow.SkillsChanged, OwnerSeat: 0 },
            "Dujiang uses its real Preparation receipt at zero Armor while its original skill grant is awaiting a child.");

        match.Restore();
        match.Need(match.Facts<ShenGuoJiaAwakeningOverrideEvent>().SequenceEqual([grant]) &&
            match.Facts<MouLuMengDujiangAwakenedEvent>().SequenceEqual([awakened]) &&
            JsonSerializer.Serialize(match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == root.Id)) == JsonSerializer.Serialize(root),
            "Cold replay keeps the same grant boundary and exact zero-Armor Dujiang parent.");
        match.Continue();
        match.ReachPlay();
        match.Need(match.Owner.Armor == 0 && match.Owner.MaxHp == initialMaximum - 2 &&
            match.Owner.Skills?.Count(skill => skill.ContentId == Duojing) == 1 &&
            match.Facts<SkillsAcquiredEvent>().Count(fact => fact.PlayerSeat == 0 && fact.SourceSkillId == Dujiang && fact.SkillIds.SequenceEqual([Duojing])) == 1 &&
            match.Facts<MouLuMengDujiangAwakenedEvent>().SequenceEqual([awakened]),
            "The original Dujiang settles once and grants exactly one Duojing to the recorded recipient.");
    }

    internal static void PaidHuishiWaivesFanxiangWitnessForNextPreparationCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(),
            new Fixture("ol:sp-sun-shangxiang", [Fanxiang, Liangzhu], WatchMaximum, "classic:sun-shangxiang"));
        var match = new HuishiMatch(registry);
        match.Start();
        var firstTurn = match.Game.State.TurnNumber;
        var initialMaximum = match.Owner.MaxHp;
        match.Need(match.Facts<SpSunShangxiangHelpedEvent>().Length == 0 && match.Facts<SpSunShangxiangFanxiangAcceptedEvent>().Length == 0,
            "The first real Preparation has no wounded beneficiary and does not award Fanxiang.");

        match.GrantToSelf(Fanxiang);
        var grant = match.Facts<ShenGuoJiaAwakeningOverrideEvent>().Single();
        var reducedMaximum = initialMaximum - 2;
        var hpBeforeAwakening = match.Owner.Hp;
        match.Need(grant is { Recipient: 0, SkillId: Fanxiang, Source: { OwnerSeat: 0, SkillId: Huishi, BindingId: "grant-awakening" } } &&
            match.Owner.MaxHp == reducedMaximum && match.Facts<ShenGuoJiaMaximumPaidEvent>().Single().FrameId == grant.FrameId &&
            match.Facts<SpSunShangxiangFanxiangAcceptedEvent>().Length == 0, "Actual paid Huishi grants the same Fanxiang instance for a later turn.");

        match.EndPlay();
        match.Reach(() => match.Prompt?.SkillPrompt?.SkillId == Watch);
        var accepted = match.Facts<SpSunShangxiangFanxiangAcceptedEvent>().Single();
        var root = match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == accepted.FrameId);
        var health = match.Game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.ResumeFrameId == root.Id);
        match.Need(root.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, OwnerSeat: 0 } preparation &&
            preparation.ParentFrameId == accepted.Receipt.LifecycleId && root.SpSunShangxiangFanxiang == accepted.Receipt &&
            accepted.Receipt is { QualifiedSeat: -1, Source: { OwnerSeat: 0, BindingId: "assisted-wounded-awakening" } } &&
            accepted.Receipt.Source.SkillInstanceId == grant.InstanceId && accepted.Receipt.GameplayHash == grant.GameplayHash &&
            accepted.Receipt.MaximumBefore == reducedMaximum && grant.FrameId < root.Id &&
            match.Facts<TurnStartedEvent>().Where(fact => fact.ActorSeat == 0).Select(fact => fact.TurnNumber).SequenceEqual([firstTurn, match.Game.State.TurnNumber]) &&
            match.Facts<SpSunShangxiangHelpedEvent>().Length == 0 && match.Facts<SpSunShangxiangFanxiangCompletedEvent>().Length == 0 &&
            health.Change is { Kind: HpChangeKind.MaximumHp, Amount: 1, TargetSeat: 0 } && health.Change.ParentFrameId == root.Id,
            "Actual Fanxiang uses the issued receipt without an assisted seat and waits for its original maximum-HP child.");

        match.Restore();
        match.Need(match.Facts<ShenGuoJiaAwakeningOverrideEvent>().SequenceEqual([grant]) &&
            match.Facts<SpSunShangxiangFanxiangAcceptedEvent>().SequenceEqual([accepted]) &&
            JsonSerializer.Serialize(match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == root.Id)) == JsonSerializer.Serialize(root),
            "Cold replay preserves the original accepted Fanxiang frame and its exact grant.");
        match.Continue();
        match.ReachPlay();
        var completed = match.Facts<SpSunShangxiangFanxiangCompletedEvent>().Single();
        match.Need(completed == new SpSunShangxiangFanxiangCompletedEvent(root.Id, 0, reducedMaximum + 1) &&
            match.Owner.MaxHp == reducedMaximum + 1 && match.Owner.Hp == hpBeforeAwakening + 1 &&
            match.Owner.Skills?.Count(skill => skill.ContentId == Xiaoji) == 1 &&
            match.Owner.Skills?.All(skill => skill.ContentId != Liangzhu) == true &&
            match.Facts<SpSunShangxiangFanxiangAcceptedEvent>().SequenceEqual([accepted]),
            "The original Fanxiang recovers one HP, replaces Liangzhu with one Xiaoji and completes once.");
    }

    internal static void PaidHuishiWaivesMoucuanBingForNextPreparationCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture("ol:ma-yuan-yi", [Moucuan], WatchMaximum));
        var match = new HuishiMatch(registry);
        match.Start();
        var firstTurn = match.Game.State.TurnNumber;
        var initialMaximum = match.Owner.MaxHp;
        match.Need(match.Facts<MoucuanMaximumPaidEvent>().Length == 0, "The first actual Preparation does not pay for Moucuan.");
        match.GrantToSelf(Moucuan);
        var grant = match.Facts<ShenGuoJiaAwakeningOverrideEvent>().Single();
        var reducedMaximum = initialMaximum - 2;
        match.Need(grant is { Recipient: 0, SkillId: Moucuan } && match.Owner.MaxHp == reducedMaximum &&
            match.Facts<ShenGuoJiaMaximumPaidEvent>().Single().FrameId == grant.FrameId &&
            match.Facts<MoucuanMaximumPaidEvent>().Length == 0, "Huishi pays for the original Moucuan instance without awakening it immediately.");

        match.EndPlay();
        match.Reach(() => match.Prompt?.SkillPrompt?.SkillId == Watch);
        var paid = match.Facts<MoucuanMaximumPaidEvent>().Single();
        var root = match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paid.FrameId);
        var health = match.Game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.ResumeFrameId == root.Id);
        match.Need(root.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, OwnerSeat: 0 } preparation &&
            root.MaYuanYi is { BingBefore: 0, FactionCount: 1, Stage: MaYuanYiStage.MaximumChildren } receipt &&
            receipt.ParentId == preparation.ParentFrameId && receipt.Source.SkillInstanceId == grant.InstanceId && receipt.GameplayHash == grant.GameplayHash &&
            match.Facts<TurnStartedEvent>().Where(fact => fact.ActorSeat == 0).Select(fact => fact.TurnNumber).SequenceEqual([firstTurn, receipt.ActualTurn]) &&
            grant.FrameId < paid.FrameId && paid.OwnerSeat == 0 && paid.MaximumBefore == reducedMaximum && paid.MaximumAfter == reducedMaximum - 1 &&
            health.Change is { Kind: HpChangeKind.MaximumHp, Amount: 1, TargetSeat: 0 } && health.Change.ParentFrameId == root.Id &&
            match.Facts<MoucuanSkillGrantedEvent>().Length == 0,
            "With zero Bing and one faction, the original actual Preparation pays Moucuan once and awaits its health child.");

        match.Restore();
        match.Need(match.Facts<ShenGuoJiaAwakeningOverrideEvent>().SequenceEqual([grant]) &&
            match.Facts<MoucuanMaximumPaidEvent>().SequenceEqual([paid]) &&
            JsonSerializer.Serialize(match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == root.Id)) == JsonSerializer.Serialize(root),
            "Cold replay retains the actual Moucuan frame and its single expense.");
        match.Continue();
        match.ReachPlay();
        match.Need(match.Owner.MaxHp == reducedMaximum - 1 && match.Owner.Skills?.Count(skill => skill.ContentId == Binghuo) == 1 &&
            match.Facts<MoucuanMaximumPaidEvent>().SequenceEqual([paid]) && match.Facts<MoucuanSkillGrantedEvent>().SequenceEqual([new(paid.FrameId, 0)]),
            "The original expense grants exactly one Binghuo to the recipient after the health child.");
    }

    internal static void PaidHuishiWaivesJianghuoDamageBeforeNextActualTurnCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture("anniversary:shen-zhong-hui", [Jianghuo], WatchMaximum));
        var match = new HuishiMatch(registry);
        match.Start();
        var firstTurn = match.Game.State.TurnNumber;
        var initialMaximum = match.Owner.MaxHp;
        match.Need(match.Facts<DamageAppliedEvent>().Length == 0 && match.Facts<ShenZhongHuiStartedEvent>().Length == 0,
            "None of the original living participants has taken damage, so Jianghuo does not awaken before the first turn.");
        match.GrantToSelf(Jianghuo);
        var grant = match.Facts<ShenGuoJiaAwakeningOverrideEvent>().Single();
        var reducedMaximum = initialMaximum - 2;
        match.Need(grant is { Recipient: 0, SkillId: Jianghuo } && match.Owner.MaxHp == reducedMaximum &&
            match.Facts<ShenGuoJiaMaximumPaidEvent>().Single().FrameId == grant.FrameId,
            "The real Huishi pays first and awards the original Jianghuo instance.");

        match.EndPlay();
        match.Reach(() => match.Prompt?.SkillPrompt?.SkillId == Watch);
        var started = match.Facts<ShenZhongHuiStartedEvent>().Single(fact => fact.Operation == SkillProgramEffectOp.AwakenJianghuo);
        var root = match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == started.FrameId);
        var window = match.Game.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(frame => frame.Id == started.ParentId);
        var prelude = match.Game.ResolutionStack.OfType<BeiMiHuTurnPreludeFrame>().Single(frame => frame.WindowId == window.Id);
        var health = match.Game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.ResumeFrameId == root.Id);
        match.Need(grant.FrameId < root.Id && started.Source.SkillInstanceId == grant.InstanceId && started.GameplayHash == grant.GameplayHash &&
            started.Source is { OwnerSeat: 0, BindingId: "all-living-damaged" } &&
            root.WindowContext is { Window: SkillProgramTriggerWindow.TurnBeforeStart } context && context.ParentFrameId == window.Id &&
            context.BeforeTurnPreludeProof == prelude.Proof && prelude.Proof.UpcomingOwnerSeat == 0 &&
            match.Facts<TurnStartedEvent>().Where(fact => fact.ActorSeat == 0).Select(fact => fact.TurnNumber).SequenceEqual([firstTurn]) &&
            match.Facts<DamageAppliedEvent>().Length == 0 && health.Change is { Kind: HpChangeKind.MaximumHp, Amount: 1, TargetSeat: 0 } &&
            health.Change.ParentFrameId == root.Id && match.Owner.MaxHp == reducedMaximum + 1,
            "The paid override starts Jianghuo on the original pre-turn frame before the actual next turn, even with no damage.");

        match.Restore();
        match.Need(match.Facts<ShenGuoJiaAwakeningOverrideEvent>().SequenceEqual([grant]) &&
            match.Facts<ShenZhongHuiStartedEvent>().Count(fact => fact == started) == 1 &&
            JsonSerializer.Serialize(match.Game.ResolutionStack.OfType<BeiMiHuTurnPreludeFrame>().Single(frame => frame.Id == prelude.Id)) == JsonSerializer.Serialize(prelude),
            "Cold replay keeps the same original pre-turn frame and the single accepted Jianghuo.");
        match.Continue();
        match.Reach(() => match.Facts<TurnStartedEvent>().Any(fact => fact.ActorSeat == 0 && fact.TurnNumber == prelude.Proof.UpcomingTurnNumber));
        match.Need(match.Owner.MaxHp == reducedMaximum + 1 && match.Owner.Skills?.Count(skill => skill.ContentId == Lishi) == 1 &&
            match.Facts<MaximumHpChangedEvent>().Count(fact => fact is { PlayerSeat: 0, SkillId: Jianghuo, Delta: 1 }) == 1 &&
            match.Facts<SkillsAcquiredEvent>().Count(fact => fact.PlayerSeat == 0 && fact.SourceSkillId == Jianghuo && fact.SkillIds.SequenceEqual([Lishi])) == 1,
            "The recorded turn starts after exactly one maximum-HP increase and one Lishi grant.");
    }

    internal static void PaidHuishiWaivesDiaolingMilitaryCardsForNextPreparationCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture("ol:sp-zhang-liao", [Diaoling], WatchMaximum));
        var match = new HuishiMatch(registry);
        match.Start();
        var firstTurn = match.Game.State.TurnNumber;
        var initialMaximum = match.Owner.MaxHp;
        match.Need(match.Facts<MubingCardsAcquiredEvent>().Length == 0 && match.Facts<DiaolingAwakenedEvent>().Length == 0,
            "The first actual Preparation has no military acquisitions and offers no natural Diaoling.");
        match.GrantToSelf(Diaoling);
        var grant = match.Facts<ShenGuoJiaAwakeningOverrideEvent>().Single();
        var reducedMaximum = initialMaximum - 2;
        match.Need(grant is { Recipient: 0, SkillId: Diaoling } && match.Owner.MaxHp == reducedMaximum &&
            match.Facts<ShenGuoJiaMaximumPaidEvent>().Single().FrameId == grant.FrameId &&
            match.Facts<DiaolingAwakenedEvent>().Length == 0, "Huishi pays for the original Diaoling instance for a later turn.");

        match.EndPlay();
        match.Reach(() => match.Prompt?.SkillPrompt?.SkillId == Diaoling);
        var awakened = match.Facts<DiaolingAwakenedEvent>().Single();
        var root = match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == awakened.ProgramFrameId);
        match.Need(awakened is { OwnerSeat: 0, MilitaryCardCount: 0 } && match.Facts<MubingCardsAcquiredEvent>().Length == 0 &&
            root.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, OwnerSeat: 0 } &&
            root.TriggerId == "six-military-cards" && root.SkillInstanceId == grant.InstanceId && root.GameplayHash == grant.GameplayHash &&
            grant.FrameId < root.Id &&
            match.Facts<TurnStartedEvent>().Where(fact => fact.ActorSeat == 0).Select(fact => fact.TurnNumber).SequenceEqual([firstTurn, match.Game.State.TurnNumber]) &&
            match.Prompt is { PlayerSeat: 0 } prompt && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "draw-two"),
            "The original Diaoling awakens at zero military cards during the next actual Preparation and offers its original reward.");

        match.Restore();
        match.Need(match.Facts<ShenGuoJiaAwakeningOverrideEvent>().SequenceEqual([grant]) &&
            match.Facts<DiaolingAwakenedEvent>().SequenceEqual([awakened]) &&
            JsonSerializer.Serialize(match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == root.Id)) == JsonSerializer.Serialize(root),
            "Cold replay keeps the same original Diaoling pending reward.");
        match.ChooseOption("draw-two");
        match.ReachPlay();
        var drawn = match.Game.CardMovements.Where(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) &&
            move.Reason.Value == "skill-program.ol:diaoling.Draw").Select(move => move.CardId).ToArray();
        match.Need(drawn.Length == 2 && drawn.Distinct().Count() == 2 &&
            match.Facts<DiaolingAwakenedEvent>().SequenceEqual([awakened]) && match.Owner.MaxHp == reducedMaximum,
            "Choosing the original reward moves exactly two physical cards to the recorded owner and completes Diaoling once.");
    }

    internal static void PaidHuishiLetsActualChoujueAcceptBeforeGeneralCompletionCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture("ol:wen-yang", [Choujue], WatchMaximum));
        var match = new HuishiMatch(registry);
        match.Start();
        var turn = match.Game.State.TurnNumber;
        var initialMaximum = match.Owner.MaxHp;
        match.GrantToSelf(Choujue);
        var grant = match.Facts<ShenGuoJiaAwakeningOverrideEvent>().Single();
        var reducedMaximum = initialMaximum - 2;
        match.Need(grant is { Recipient: 0, SkillId: Choujue } && match.Owner.MaxHp == reducedMaximum &&
            match.Facts<ShenGuoJiaMaximumPaidEvent>().Single().FrameId == grant.FrameId &&
            match.Facts<WenYangAwakenedEvent>().Length == 0, "The actual Huishi payment grants later Choujue without starting it immediately.");

        match.EndPlay();
        match.Reach(() => match.Prompt?.SkillPrompt?.SkillId == Watch);
        var accepted = match.Facts<WenYangAwakenedEvent>().Single();
        var started = match.Facts<WenYangStartedEvent>().Single(fact => fact.FrameId == accepted.FrameId);
        var root = match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == accepted.FrameId);
        var parent = match.Game.ResolutionStack.OfType<DeferredTurnEndFrame>().Single(frame => frame.Id == started.ParentId);
        var health = match.Game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(frame => frame.ResumeFrameId == root.Id);
        match.Need(accepted is { OwnerSeat: 0, Level: 1 } && accepted.MaximumBefore == reducedMaximum && accepted.MaximumAfter == reducedMaximum - 1 &&
            Math.Abs(match.Owner.HandCount - accepted.HpBefore) < 3 && grant.FrameId < root.Id &&
            root.WenYang is { Paid: true, Stage: WenYangStage.MaximumChildren } && started.Source.SkillInstanceId == grant.InstanceId &&
            started.GameplayHash == grant.GameplayHash && started.Source.BindingId == "own-actual-turn-ended" &&
            parent.TurnNumber == turn && parent.OwnerSeat == 0 &&
            match.Facts<TurnEndedEvent>().Any(fact => fact.ActorSeat == 0 && fact.TurnNumber == turn) &&
            health.Change is { Kind: HpChangeKind.MaximumHp, TargetSeat: 0, Amount: 1 } && health.Change.ParentFrameId == root.Id &&
            match.Facts<SkillAwakenedEvent>().All(fact => fact.SkillId != Choujue) && match.Facts<WenYangLeafGrantedEvent>().Length == 0,
            "Choujue accepts at the original actual turn end below its natural threshold, before the general awakening or Beishui grant.");

        match.Restore();
        var acceptedHistory = new ProgramEventHistoryIndex(match.Game.Events, []).Read<IProgramAwakeningAcceptedEvent>().Single();
        match.Need(match.Facts<WenYangAwakenedEvent>().SequenceEqual([accepted]) &&
            match.Facts<ShenGuoJiaAwakeningOverrideEvent>().SequenceEqual([grant]) &&
            acceptedHistory.MatchesAwakening(0, Choujue) && !acceptedHistory.MatchesAwakening(0, Beishui) &&
            !acceptedHistory.MatchesAwakening(1, Choujue) &&
            match.Facts<SkillAwakenedEvent>().All(fact => fact.SkillId != Choujue) &&
            JsonSerializer.Serialize(match.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == root.Id)) == JsonSerializer.Serialize(root),
            "Cold replay preserves the exact accepted event while its original health child is still pending.");
        match.Continue();
        match.Reach(() => match.Game.ResolutionStack.All(frame => frame.Id != root.Id));
        match.Need(match.Facts<WenYangAwakenedEvent>().SequenceEqual([accepted]) &&
            match.Facts<WenYangLeafGrantedEvent>().SequenceEqual([new(root.Id, 0, Beishui)]) &&
            match.Facts<SkillAwakenedEvent>().Count(fact => fact.PlayerSeat == 0 && fact.SkillId == Choujue && fact.AcquiredSkillIds.SequenceEqual([Beishui])) == 1 &&
            match.Owner.Skills?.Count(skill => skill.ContentId == Beishui) == 1,
            "Returning the original child awards Beishui and emits general completion exactly once.");
    }

    private sealed class HuishiMatch(ContentRegistry registry)
    {
        internal GameEngine Game { get; private set; } = CreateMatch(registry);
        internal PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        internal PlayerSnapshot Owner => Game.CreateSnapshot(0).Players[0];
        internal T[] Facts<T>() where T : IGameEvent => Game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

        internal void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {Game.State.TurnNumber}, phase {Game.State.Phase}, " +
                    $"prompt {Prompt?.Kind}/{Prompt?.SkillPrompt?.SkillId}.");
        }

        internal void Start()
        {
            Submit(new StartGameCommand());
            Reach(() => Prompt is { Kind: DecisionKind.SelectGeneral, PlayerSeat: 0 });
            Submit(new SelectGeneralCommand(0, OwnerGeneral, Game.Revision, Prompt!.PromptId));
            ReachPlay();
        }

        internal void ReachPlay() => Reach(() => Prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && Game.ResolutionStack.Count == 0);
        internal void EndPlay() => Submit(new EndPlayPhaseCommand(0, Game.Revision, Prompt!.PromptId));
        internal void Continue() => ChooseOption("continue");
        internal void ChooseOption(string option) => Answer("option-id", option);

        internal void GrantToSelf(string awakening)
        {
            var action = Game.GetLegalActions(0).Single(candidate =>
                candidate.ProgramSkillId == Huishi && candidate.ProgramActivationId == "grant-awakening");
            Need(action.SelectableTargetSeats.Contains(0), "The real limited skill offers its owner as a target.");
            Submit(new UseProgramSkillCommand(0, Huishi, "grant-awakening", [], [0], Game.Revision, Prompt!.PromptId));
            Reach(() => Prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("value") == awakening) == true);
            Answer("value", awakening);
            ReachPlay();
        }

        internal void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 155 && !ready(); step++)
            {
                if (Prompt?.SkillPrompt?.SkillId == Watch)
                    Continue();
                else
                {
                    Need(Prompt is null && Game.State.Winner == Winner.None, "The awakening fixture reached an unexpected boundary.");
                    Submit(new AdvanceOneStepCommand(Game.Revision));
                }
            }
            Need(ready(), "The actual Preparation or its native child was not reached.");
        }

        internal void Restore()
        {
            var before = Game;
            Game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(before.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(Game.CreateSnapshot(seat)) == SnapshotJson.Serialize(before.CreateSnapshot(seat))) &&
                Game.CardMovements.SequenceEqual(before.CardMovements), "Cold replay preserves every seat view and physical movement.");
        }

        private void Submit(GameCommand command)
        {
            var result = Game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The actual awakening command was rejected.");
        }

        private void Answer(string parameter, string value)
        {
            var prompt = Prompt!;
            var choice = prompt.Choices.Single(candidate => candidate.Parameters.GetValueOrDefault(parameter) == value);
            Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision));
        }
    }

    private sealed class Fixture(string recipientGeneral, IReadOnlyList<string> recipientSkills, string watchTrigger, params string[] relatedGenerals) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-huishi-baijia-owner", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, ["mobile:shen-guo-jia", recipientGeneral, .. relatedGenerals]);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Watch}}","revision":1,"triggers":[{{watchTrigger}}]}
                ]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{
                  "name":"体力上限已支付","description":"实际体力上限变化后暂停。","optionLabels":{"continue":"继续"} } } }
                """);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(OwnerGeneral, "辉逝觉醒原生夹具", "supporter", Huishi, "wei", 6, [.. recipientSkills, Watch]));
            var peers = Enumerable.Range(1, 3).Select(index => $"fixture:huishi-baijia-peer-{index}").ToArray();
            foreach (var peer in peers)
                builder.AddGeneral(new(peer, "固定同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "辉逝拜假固定牌堆", 4, 0, [new("standard:dodge", 40)]));
            builder.AddMode(new(Mode, "辉逝拜假真实准备阶段", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: [OwnerGeneral, .. peers]));
        }
    }
}
