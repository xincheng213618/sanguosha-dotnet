using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Persistence;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private IShanheCampaignSaveStore? _shanheSaveStore;
    private ShanheCampaignSession? _shanheSession;
    private GameEngine? _shanheBattleGame;
    private GameEngine? _shanheTableGame;
    private GameEngine? _shanheReturnGame;
    private ShanheBattleSetup? _shanheTableSetup;
    private int _shanheRunDifficulty = 1;
    private bool _isShanheOpen;
    private bool _shanheWasAutoAdvance;
    private bool _hasShanheProgress;
    private string _shanheStatusText = "准备出征";
    private ShanheDifficultyOption _selectedShanheDifficulty = null!;
    private ShanheGeneralOption? _selectedShanheGeneral;
    private ShanheScenarioOption? _selectedShanheScenario;

    public IReadOnlyList<ShanheScenarioOption> ShanheScenarios { get; } =
        StandardShanheCampaignCatalog.Editions.Select(edition => new ShanheScenarioOption(edition.Id,
            edition.Name + (edition.Id == StandardShanheCampaignCatalog.Source2026EditionId ? "（推荐）" : ""))).ToArray();
    public ShanheScenarioOption SelectedShanheScenario
    {
        get => _selectedShanheScenario ?? ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Source2026EditionId);
        set
        {
            if (IsShanheRunInProgress && value?.Id != StandardShanheCampaignCatalog.EditionIdForCampaign(_shanheSession!.Definition.Id, _shanheRunDifficulty)) return;
            if (value is null || !ShanheScenarios.Contains(value) || !SetProperty(ref _selectedShanheScenario, value)) return;
            var previousDifficulty = _selectedShanheDifficulty?.Value ?? 1;
            _selectedShanheDifficulty = ShanheDifficulties.FirstOrDefault(option => option.Value == previousDifficulty) ?? ShanheDifficulties[0];
            RaisePropertyChanged(nameof(ShanheDifficulties));
            RaisePropertyChanged(nameof(SelectedShanheDifficulty));
            RaisePropertyChanged(nameof(SelectedShanheScenarioDescription));
            RaisePropertyChanged(nameof(SelectedShanheDifficultyDescription));
            RefreshShanheGuideTasks();
            if (!IsShanheRunInProgress) RefreshShanhePresentation();
        }
    }
    private static readonly IReadOnlyList<ShanheDifficultyOption> LocalShanheDifficulties =
        [new(1, "初征 · 难度一"), new(2, "进阶 · 难度二"), new(3, "鏖战 · 难度三")];
    private static readonly IReadOnlyList<ShanheDifficultyOption> SourceShanheDifficulties = Array.AsReadOnly(
        StandardShanheSourceDifficultyCatalog.Season5.Entries.Select(row => new ShanheDifficultyOption(row.SourceId, row.Name)).ToArray());
    public IReadOnlyList<ShanheDifficultyOption> ShanheDifficulties => SelectedShanheScenario.Id == StandardShanheCampaignCatalog.Source2026EditionId
        ? SourceShanheDifficulties : LocalShanheDifficulties;
    public ObservableRangeCollection<ShanheGeneralOption> ShanheGenerals { get; } = [];
    public ObservableCollection<ShanheMapRow> ShanheMapRows { get; } = [];
    public ObservableCollection<ShanheJourneyNode> ShanheMapNodes { get; } = [];
    public ObservableCollection<ShanheJourneyLink> ShanheMapLinks { get; } = [];
    public ObservableCollection<ShanheCampaignChoice> ShanheChoices { get; } = [];
    public ObservableCollection<ShanheBuildCard> ShanheTacticCards { get; } = [];
    public ObservableCollection<ShanheBuildCard> ShanheExtraSkillCards { get; } = [];

    public ShanheDifficultyOption SelectedShanheDifficulty
    {
        get => _selectedShanheDifficulty;
        set
        {
            if (IsShanheRunInProgress && value?.Value != _shanheRunDifficulty) return;
            if (value is null || !ShanheDifficulties.Contains(value) || !SetProperty(ref _selectedShanheDifficulty, value)) return;
            RaisePropertyChanged(nameof(SelectedShanheDifficultyDescription));
            RefreshShanheGuideTasks();
            if (!IsShanheRunInProgress) RefreshShanhePresentation();
        }
    }
    public ShanheGeneralOption? SelectedShanheGeneral
    {
        get => _selectedShanheGeneral;
        set
        {
            if (IsShanheRunInProgress && value?.Id != _shanheSession!.Snapshot.PlayerGeneralId) return;
            if (!IsShanheRunInProgress && !IsSelectableShanheGeneral(value)) return;
            // Changing a page clears the ListBox selection before it binds the next eight cards.
            // Keep the chosen general until the player explicitly chooses another card.
            if (value is null || !ShanheGenerals.Contains(value) || !SetProperty(ref _selectedShanheGeneral, value)) return;
            RevealSelectedShanheGeneral();
            foreach (var property in new[] { nameof(SelectedShanhePortrait), nameof(SelectedShanheGeneralDescription), nameof(SelectedShanheSkillText), nameof(SelectedShanheGeneralMeta) })
                RaisePropertyChanged(property);
            (StartShanheCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }
    }
    public Brush SelectedShanhePortrait => SelectedShanheGeneral?.Portrait ?? Brushes.Transparent;
    public string SelectedShanheGeneralDescription => SelectedShanheGeneral?.Description ?? "选择一名武将，查看技能与出征信息。";
    public string SelectedShanheSkillText => SelectedShanheGeneral?.SkillSummary ?? string.Empty;
    public string SelectedShanheGeneralMeta
    {
        get
        {
            if (SelectedShanheGeneral is not { } general) return string.Empty;
            var text = $"{general.Kingdom} · {general.EditionText}";
            var defaultFaction = IsShanheRunInProgress && _shanheSession is { } session ? session.Definition.DefaultGodFactionId :
                StandardShanheCampaignCatalog.Editions.Single(edition => edition.Id == SelectedShanheScenario.Id).DefaultGodFactionId;
            return defaultFaction is not null &&
                StandardShanheCampaignCatalog.PlayerRosterRegistry.Generals[general.Id].FactionId == "god"
                ? text + $" · 出征默认{FactionName(defaultFaction)}" : text;
        }
    }
    public string SelectedShanheScenarioDescription => SelectedShanheScenario.Id switch
    {
        StandardShanheCampaignCatalog.Source2026EditionId =>
            "诸侯伐董：每章完成六格主线即可挑战首领。沿途开放奇遇、集市和休息，两处可选战场与擂台可按需补打，全部体验共十次。日行千里Ⅰ / Ⅱ每章再增加1 / 2场战斗。可选25档难度。",
        StandardShanheCampaignCatalog.Returning2026EditionId =>
            "诸侯伐董：沿三章路线征战，收集战法、技能与装备，挑战神鬼无前吕布等关底首领。",
        _ => "诸侯伐董：选择路线，击败敌军，组合战法与技能，抵达洛阳。"
    };
    public string SelectedShanheDifficultyDescription
    {
        get
        {
            if (SelectedShanheScenario.Id != StandardShanheCampaignCatalog.Source2026EditionId)
                return $"本地第{SelectedShanheDifficulty?.Value ?? 1}档。";
            var source = StandardShanheSourceDifficultyCatalog.Season5.Get(SelectedShanheDifficulty?.Value ?? 1);
            return source.Name + "：" + (string.IsNullOrEmpty(source.Description) ? "基础难度。" : source.Description + "。") +
                "\n本地配置复现可直接选择全部25档，敌军实际数值见关卡预览。";
        }
    }
    public string ShanheScenarioDetails => IsShanheRunInProgress && _shanheSession is { } session ? session.Definition.IntroText :
        StandardShanheCampaignCatalog.CreateCodexDefinition(SelectedShanheDifficulty?.Value ?? 1, SelectedShanheScenario.Id).IntroText;
    public bool HasShanheSession => _shanheSession is not null;
    public bool IsShanheOpen
    {
        get => _isShanheOpen;
        private set
        {
            if (!SetProperty(ref _isShanheOpen, value)) return;
            if (value) { _shanheWasAutoAdvance = IsAutoAdvance; IsAutoAdvance = false; }
            else IsAutoAdvance = _shanheWasAutoAdvance;
            RaisePropertyChanged(nameof(CanSaveShanheProgress));
            RaisePropertyChanged(nameof(SessionSaveStatus));
            (SaveGameCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }
    }
    public bool HasShanheProgress { get => _hasShanheProgress; private set => SetProperty(ref _hasShanheProgress, value); }
    public bool CanContinueShanheExpedition => HasShanheProgress && (_shanheSession is null || IsShanheRunInProgress);
    public string ShanheStatusText
    {
        get => _shanheStatusText;
        private set { if (SetProperty(ref _shanheStatusText, value)) RaisePropertyChanged(nameof(SessionSaveStatus)); }
    }
    public string ShanheEditionNotice => "单机征程 · " + SelectedShanheScenario.Name +
        (SelectedShanheScenario.Id == StandardShanheCampaignCatalog.LocalEditionId
            ? " · 本地起步版" : " · 神武将出征默认按群");
    public string ShanheCampaignTitle => _shanheSession?.Snapshot.CampaignName ?? "诸侯伐董 · 征途将启";
    public bool IsShanheBattleActive => _shanheSession?.Snapshot.Phase == ShanheCampaignPhase.Battle;
    public bool IsShanheBattleSession => _shanheTableGame is not null && ReferenceEquals(_game, _shanheTableGame);
    public string ShanheRouteSummary => _shanheSession is { } session
        ? $"已过 {session.Snapshot.CompletedNodeIds.Count} 处 · 当前：{ShanheCurrentNodeName} · 难度 {_shanheRunDifficulty}\n路线：" +
            string.Join(" → ", session.Snapshot.CompletedNodeIds.Select(id => session.Definition.Nodes.Single(node => node.Id == id).Name)
                .Append(ShanheCurrentNodeName).Distinct(StringComparer.Ordinal))
        : "从军营起程，选择战斗、奇遇、行商与营地，沿自己的路线抵达洛阳。";
    public string ShanheVitalText
    {
        get
        {
            if (_shanheSession is not { } session) return "选择一名武将，踏上征途";
            var live = HasLiveShanheBattle
                ? CurrentShanheBattleState?.Players.Single(player => player.IsHuman) : null;
            return $"{ShanheGeneralName(session.Snapshot.PlayerGeneralId)}   体力 {live?.Hp ?? session.Snapshot.Hp}/{live?.MaxHp ?? session.Snapshot.MaxHp}";
        }
    }
    public string ShanheGoldText => $"铜币   {CurrentShanheBuildState?.Gold ?? 0}";
    public string ShanheTacticsText => CurrentShanheBuildState is { } state
        ? $"{ShanheRankStatusText}\n体力上限 +{state.Bonuses.MaxHp} · 起手 +{state.Bonuses.InitialHand} · 摸牌 +{state.Bonuses.Draw} · 出杀 +{state.Bonuses.Slash}\n战法 {state.TacticIds.Count} 项"
        : "战法：征途中获得的成长会带入后续战斗。";
    public string ShanheBuildTabTitle => $"战法构筑 · {ShanheTacticCards.Count}";
    public string ShanheTacticEmptyText => ShanheTacticCards.Count == 0 ? "尚无战法，可从奖励或集市获取。" : "当前战法";
    public string ShanheStartingLoadoutText => CurrentShanheBuildState is { } state
        ? "随身牌：" + (state.StartingCards.Count == 0 ? "无" :
            string.Join("、", state.StartingCards.Select(card => CardCatalog.Get(card.Kind).DisplayName))) +
            "\n随身装备：" + (state.StartingEquipment.Count == 0 ? "无" :
                string.Join("、", state.StartingEquipment.Select(card => CardCatalog.Get(card.Kind).DisplayName)))
        : "随身牌每战加入手牌，随身装备每战开局直接置入装备区。";
    private bool ShanheStartingResourcesExpireOnUse => _shanheSession?.Definition.StartingResourcesExpireOnUse ?? true;
    private string ShanheStartingResourceNotice => ShanheStartingResourcesExpireOnUse
        ? "随身牌每战加入手牌，用后按规则移出；随身装备开战直接装备，可正常夺取和使用。牌堆不含装备。"
        : "随身牌每战加入手牌，初始装备每战直接装备。";
    public string ShanheSkillsText => CurrentShanheBuildState is { } state
        ? $"额外技能 {state.ExtraSkillIds.Count}/{state.ExtraSkillSlots}：" +
            (state.ExtraSkillIds.Count == 0 ? "尚未习得" : string.Join("、", state.ExtraSkillIds.Select(ShanheSkillName))) +
            $"\n随身牌 {state.StartingCards.Count} · 随身装备 {state.StartingEquipment.Count}" + ShanheRecruitedCompanionsText
        : "额外技能：在战后奖励、奇遇与行商中寻找新的组合。";
    private string ShanheCurrentNodeName => _shanheSession?.Snapshot.CurrentStoryEventName ?? (_shanheSession?.Snapshot.CurrentNodeId is { } id
        ? _shanheSession.Definition.Nodes.FirstOrDefault(node => node.Id == id)?.Name ?? id : "起点");
    private string ShanheBattleNodeName => _shanheTableSetup is { } setup
        ? _shanheSession?.Definition.Nodes.FirstOrDefault(node => node.Id == setup.NodeId)?.Name ?? setup.NodeId : ShanheCurrentNodeName;
    private int ShanheAlliedSeatCount => _snapshot?.Players.Count(player => player.TeamId == ShanheBattleSetup.PlayerTeamId) ?? 0;
    private int ShanheEnemySeatCount => _snapshot?.Players.Count(player => player.TeamId == ShanheBattleSetup.EnemyTeamId) ?? 0;
    private string ShanheBattleRosterText => $"义军 {ShanheAlliedSeatCount} 席 · 敌军 {ShanheEnemySeatCount} 席 · {Math.Max(0, ShanheAlliedSeatCount - 1)} 名人机友军";
    private string ShanheBattleObjective => $"你的目标：击败{ShanheBattleNodeName}的全部敌军。" +
        (ShanheAlliedSeatCount > 1 ? $"本战有 {ShanheAlliedSeatCount - 1} 名人机友军助战。" : "本战由你独自迎敌。") +
        "胜利后保留剩余体力，领取战利品再继续行军；战败可按进入本关时的状态重新挑战。";
    public string ShanheCarryoverNotice => "胜利后保留剩余体力。战法持续本次征程，每战获得且不占技能槽；额外所学技能单独占槽，满槽时可替换。" + ShanheStartingResourceNotice;
    public string ShanheContextTitle => _shanheSession?.Snapshot.Phase switch
    {
        ShanheCampaignPhase.Battle => "战 鼓 已 起",
        ShanheCampaignPhase.Reward => "战 后 整 备",
        ShanheCampaignPhase.OpeningSkillDraft => "传 承 · 初 始 技 能",
        ShanheCampaignPhase.NodeChoice => ShanheCurrentNodeName,
        ShanheCampaignPhase.Won => "山 河 新 篇",
        ShanheCampaignPhase.Lost => "此 行 未 竟",
        _ => _shanheSession is null ? "诸 侯 会 盟" : "前 路 待 择"
    };
    public string ShanheContextDescription
    {
        get
        {
            if (_shanheSession is not { } session) return SelectedShanheScenarioDescription;
            var state = session.Snapshot;
            if (state.Phase is ShanheCampaignPhase.Won or ShanheCampaignPhase.Lost) return state.EndingText;
            return state.StoryText + (state.BossRewardSummary is { IsBonusOnly: true } ? "\n\n领取本次胜利的追加奖励，确认后继续行军。"
                : state.BossRewardSummary is not null ? "\n\n首领已败，处理本章各组奖励，确认后继续征程。"
                : state.Phase == ShanheCampaignPhase.Reward ? "\n\n从下方奖励中选择一项，再继续征程。"
                : state.Phase == ShanheCampaignPhase.Battle ? "\n\n" + (IsShanheBattleRetryReady ? ShanheBattleRetryDescription
                    : "可返回牌桌继续战斗；保存进度并退出后，下次从本关开头重新挑战。") : "");
        }
    }

    public ICommand OpenShanheCommand { get; private set; } = null!;
    public ICommand CloseShanheCommand { get; private set; } = null!;
    public ICommand StartShanheCommand { get; private set; } = null!;
    public ICommand ContinueShanheCommand { get; private set; } = null!;
    public ICommand ResumeShanheBattleCommand { get; private set; } = null!;
    public ICommand SelectShanheNodeCommand { get; private set; } = null!;
    public ICommand ChooseShanheOptionCommand { get; private set; } = null!;

    private void InitializeShanheCampaign(IShanheCampaignSaveStore? store)
    {
        _shanheSaveStore = store ?? new FileShanheCampaignSaveStore();
        OpenShanheCommand = new RelayCommand(() =>
        {
            if (IsTutorialActive) return;
            IsHelpOpen = false; IsLogOpen = false; IsHistoryOpen = false; IsGeneralGalleryOpen = false;
            IsShanheOpen = true;
            if (_shanheSession is null && HasShanheProgress) ContinueShanheCampaign();
            RefreshShanhePresentation();
        }, () => !IsTutorialActive);
        CloseShanheCommand = new RelayCommand(LeaveShanheExpedition);
        StartShanheCommand = new RelayCommand(StartShanheCampaign, () => CanPrepareShanheExpedition && IsSelectableShanheGeneral(SelectedShanheGeneral));
        ContinueShanheCommand = new RelayCommand(ContinueShanheCampaign, () => !IsTutorialActive && CanContinueShanheExpedition);
        ResumeShanheBattleCommand = new RelayCommand(ResumeShanheBattle, () => IsShanheBattleActive && !IsTutorialActive);
        SelectShanheNodeCommand = new RelayCommand<ShanheCampaignNode>(SelectShanheNode,
            IsCurrentShanheNode);
        ChooseShanheOptionCommand = new RelayCommand<ShanheCampaignChoice>(ChooseShanheOption,
            IsCurrentShanheChoice);
        _selectedShanheDifficulty = ShanheDifficulties[0];
        InitializeShanheGeneralRoster();
        InitializeShanheJourney();
        InitializeShanheExit();
        InitializeShanheRewards();
        HasShanheProgress = _shanheSaveStore.Exists;
        RefreshShanhePresentation();
    }

    private void StartShanheCampaign()
    {
        if (!CanPrepareShanheExpedition || SelectedShanheGeneral is not { } general || !IsSelectableShanheGeneral(general)) return;
        if (IsShanheSettlementPending && (!SaveShanheProgress() || IsShanheSettlementPending))
        { ShanheStatusText = "上次征程结算尚未保存，请先重试结算，避免覆盖图册与征程记录。"; return; }
        try
        {
            var difficulty = SelectedShanheDifficulty.Value;
            var definition = StandardShanheCampaignCatalog.CreateCodexDefinition(difficulty, SelectedShanheScenario.Id);
            var session = ShanheCampaignSession.Create(definition,
                CreateCodexCampaignOptions(_seedOverride ?? RandomNumberGenerator.GetInt32(1, int.MaxValue), general.Id, definition)
                    with { PlayerRank = SelectedShanheRank.Value == 1 ? null : SelectedShanheRank.Value,
                        PersistentMarket = SelectedShanheScenario.Id is StandardShanheCampaignCatalog.Returning2026EditionId or
                            StandardShanheCampaignCatalog.Source2026EditionId },
                StandardShanheCampaignCatalog.CreateBattleRegistry);
            _shanheSession = session;
            _shanheResumeFailed = false;
            _shanheRunDifficulty = difficulty;
            _shanheBattleGame = null;
            HasShanheProgress = true;
            RefreshShanhePresentation();
            SaveShanheProgress();
        }
        catch (Exception error) when (IsSaveException(error)) { ShanheStatusText = $"征程未能开始：{ReadableSaveError(error)}"; }
    }

    private void ContinueShanheCampaign()
    {
        if (IsTutorialActive || _shanheSaveStore is null) return;
        if (_shanheSession is not null)
        {
            if (EnableCurrentShanheRewardRules()) SaveShanheProgress();
            IsShanheOpen = true; RefreshShanhePresentation(); return;
        }
        try
        {
            var save = _shanheSaveStore.Read();
            var session = RestoreShanheProgress(save);
            _shanheSession = session; _shanheRunDifficulty = save.Difficulty; _shanheBattleGame = null;
            if (session.AcceptedCommands.LastOrDefault() is EndShanheExpeditionCommand) _dismissedShanheSettlement = session;
            _shanheResumeFailed = false;
            SelectedShanheScenario = ShanheScenarios.Single(option => option.Id ==
                StandardShanheCampaignCatalog.EditionIdForCampaign(session.Definition.Id, save.Difficulty));
            SelectedShanheDifficulty = ShanheDifficulties.Single(option => option.Value == save.Difficulty);
            SelectedShanheRank = ShanheRanks.Single(option => option.Value == session.Snapshot.PlayerRank);
            var rewardRulesUpdated = EnableCurrentShanheRewardRules();
            IsShanheOpen = true;
            ShanheStatusText = $"已恢复征程进度 · {save.SavedAtUtc.ToLocalTime():MM-dd HH:mm}。" +
                (session.PendingBattleSetup is not null ? "本关从头挑战，不恢复退出前的牌局。" : "");
            SettleShanheCodexReward();
            if (IsShanheSettlementPending) _dismissedShanheSettlement = null;
            RefreshShanhePresentation();
            if (rewardRulesUpdated) SaveShanheProgress();
        }
        catch (Exception error) when (IsSaveException(error))
        {
            _shanheSession = null;
            _shanheResumeFailed = true;
            HasShanheProgress = false;
            IsShanheOpen = true;
            ShanheStatusText = $"读取征程失败，已重置到出征准备：{ReadableSaveError(error)} 原文件保留，确认新出征前不会覆盖。";
            RefreshShanhePresentation();
        }
    }

    private void SelectShanheNode(ShanheCampaignNode node)
    {
        if (IsTutorialActive || !IsCurrentShanheNode(node)) return;
        ApplyShanheCommand(new MoveShanheNodeCommand(node.Id, node.Revision));
        if (_shanheSession?.PendingBattleSetup is not null) StartShanheBattle();
    }

    private void ChooseShanheOption(ShanheCampaignChoice choice)
    {
        if (IsTutorialActive || !IsCurrentShanheChoice(choice)) return;
        ApplyShanheCommand(choice.IsReward
            ? new ChooseShanheRewardCommand(choice.Id, choice.ReplaceSkillId, choice.Revision)
            : new ChooseShanheNodeOptionCommand(choice.Id, choice.ReplaceSkillId, choice.Revision));
        if (_shanheSession?.PendingBattleSetup is not null) StartShanheBattle();
    }

    private void ApplyShanheCommand(ShanheCampaignCommand command)
    {
        if (_shanheSession is not { } session || IsTutorialActive) return;
        var previousPhase = session.Snapshot.Phase;
        var result = session.Submit(command);
        if (!result.Accepted) { ShanheStatusText = $"选择未执行：{result.Error}"; return; }
        EnableCurrentShanheRewardRules();
        if (previousPhase != ShanheCampaignPhase.Battle && result.State.Phase == ShanheCampaignPhase.Battle)
            _shanheBattleGame = null;
        RefreshShanhePresentation();
        SaveShanheProgress();
    }

    private void StartShanheBattle()
    {
        if (_shanheSession is not { PendingBattleSetup: not null } session) return;
        try
        {
            var previous = session.CreateCheckpoint();
            var issued = session.Submit(new BeginShanheBattleAttemptCommand(session.Revision));
            if (!issued.Accepted) throw new InvalidOperationException(issued.Error);
            if (!SaveShanheProgress())
            {
                _shanheSession = ShanheCampaignSession.Restore(previous, session.Definition,
                    StandardShanheCampaignCatalog.CreateBattleRegistry);
                RefreshShanhePresentation();
                return;
            }
            var setup = session.PendingBattleSetup!;
            var battle = StandardShanheCampaignCatalog.CreateBattle(setup, _confirmOpeningHand);
            _shanheBattleGame = battle;
            IsNewGameSetupOpen = false; IsIdentityRevealOpen = false;
            AttachShanheBattle(battle, setup);
            var result = SubmitCommand(new StartGameCommand());
            if (!result.Accepted) throw new InvalidOperationException(result.Error?.Message ?? "战斗开局失败。");
            RefreshCommandResult(result.State);
            IsShanheOpen = false;
            RefreshShanhePresentation();
            SaveShanheProgress();
        }
        catch (Exception error) when (IsSaveException(error))
        { ShanheStatusText = $"战斗暂未准备好：{ReadableSaveError(error)} 征程节点已保留，请重试继续征程。"; }
    }

    private void ResumeShanheBattle()
    {
        if (IsTutorialActive || !IsShanheBattleActive) return;
        if (!HasLiveShanheBattle) { StartShanheBattle(); return; }
        var battle = _shanheBattleGame!;
        if (!ReferenceEquals(_game, battle)) AttachShanheBattle(battle, _shanheSession!.PendingBattleSetup!);
        IsNewGameSetupOpen = false;
        IsShanheOpen = false;
    }

    private void CaptureShanheBattleResult()
    {
        if (_shanheSession is not { } session || session.Snapshot.Phase != ShanheCampaignPhase.Battle ||
            _shanheBattleGame is not { } battle || !ReferenceEquals(_game, battle) || battle.State.Status != EngineStatus.Completed) return;
        var completed = battle.CreateSnapshot(0);
        var player = completed.Players.Single(seat => seat.IsHuman);
        if (completed.WinnerTeamId != ShanheBattleSetup.PlayerTeamId || !player.IsAlive || player.Hp <= 0)
        {
            // The failed attempt never advances the expedition journal, spends its
            // entry resources, or issues a settlement receipt. Retry uses the same setup.
            IsShanheOpen = true;
            IsShanheMarketOpen = false;
            RefreshShanhePresentation();
            SaveShanheProgress();
            return;
        }
        var result = session.CompleteBattle(battle, session.Revision);
        if (!result.Accepted) { ShanheStatusText = $"战斗结算未完成：{result.Error}"; return; }
        EnableCurrentShanheRewardRules();
        SettleShanheCodexReward();
        IsShanheOpen = true;
        RefreshShanhePresentation();
        SaveShanheProgress();
    }

    private bool SaveShanheBeforeOrdinaryGame()
    {
        if (!IsShanheBattleSession) return true;
        if (!SaveShanheProgress()) return false;
        DiscardShanheBattleAttempt();
        _shanheWasAutoAdvance = false;
        IsShanheOpen = false;
        return true;
    }

    public bool FlushShanheProgress() => SaveShanheProgress();

    private bool SaveShanheProgress()
    {
        if (_shanheSession is not { } session || _shanheSaveStore is null || IsTutorialActive) return true;
        try
        {
            _shanheSaveStore.Write(new ShanheCampaignSaveFile(ShanheCampaignSaveFile.CurrentFormatVersion,
                DateTimeOffset.UtcNow, _shanheRunDifficulty, session.CreateCheckpoint()));
            SettleShanheCodexReward();
            HasShanheProgress = true;
            ShanheStatusText = $"山河图进度已保存 · {DateTime.Now:HH:mm:ss}" +
                (session.PendingBattleSetup is not null ? " · 退出后从本关开头挑战" : "");
            if (IsShanheBattleSession)
            {
                _autoSavePending = false;
                _saveTimer?.Stop();
                HasSaveError = false;
                SaveStatus = ShanheStatusText;
            }
            return true;
        }
        catch (Exception error) when (IsSaveException(error))
        {
            ShanheStatusText = $"山河图独立征程未保存：{ReadableSaveError(error)} 已有存档保留，当前征程仍可继续，请稍后重试。";
            if (IsShanheBattleSession)
            {
                _autoSavePending = true;
                HasSaveError = true;
                SaveStatus = ShanheStatusText;
            }
            return false;
        }
    }

    private void RefreshShanhePresentation()
    {
        if (_selectedShanheDifficulty is null) return;
        var definition = _shanheSession?.Definition ?? StandardShanheCampaignCatalog.CreateCodexDefinition(SelectedShanheDifficulty.Value, SelectedShanheScenario.Id);
        if (_shanheSession is { } mapSession)
        {
            var mapNodes = mapSession.VisibleMapNodes;
            var mapIds = mapNodes.Select(node => node.Id).ToHashSet(StringComparer.Ordinal);
            definition = definition with
            {
                Nodes = mapNodes.Select(node => node with { NextNodeIds = node.NextNodeIds.Where(mapIds.Contains).ToArray() }).ToArray(),
                StartingNodeIds = mapSession.ResolveVisibleNodeIds(definition.StartingNodeIds).Where(mapIds.Contains).ToArray()
            };
        }
        RefreshShanheGeneralRoster();
        var state = _shanheSession?.Snapshot;
        var available = state?.AvailableNodes.Select(node => node.Id).ToHashSet(StringComparer.Ordinal) ?? [];
        var completed = state?.CompletedNodeIds.ToHashSet(StringComparer.Ordinal) ?? [];
        var depths = new Dictionary<string, int>(StringComparer.Ordinal);
        var pending = new Queue<(string Id, int Depth)>();
        foreach (var id in definition.StartingNodeIds) pending.Enqueue((id, 0));
        while (pending.TryDequeue(out var item))
        {
            if (depths.ContainsKey(item.Id)) continue;
            depths[item.Id] = item.Depth;
            var current = definition.Nodes.Single(node => node.Id == item.Id);
            foreach (var next in current.NextNodeIds) pending.Enqueue((next, item.Depth + 1));
        }
        ShanheMapRows.Clear();
        foreach (var group in definition.Nodes.Where(node => !node.IsStoryBattle).GroupBy(node => depths.GetValueOrDefault(node.Id)).OrderBy(group => group.Key))
        {
            var nodes = group.Select(node => new ShanheCampaignNode(node.Id, node.Name, ShanheKindLabel(node.Kind),
                node.Description + (node.NextNodeIds.Count == 0 ? "\n此处是本条路线的终点。" : "\n通往：" +
                    string.Join("、", node.NextNodeIds.Select(id => definition.Nodes.Single(next => next.Id == id).Name))),
                completed.Contains(node.Id) ? "已完成" : state?.CurrentNodeId == node.Id ? "当前所在"
                    : available.Contains(node.Id) ? "可前往 →" : "等待前路开启",
                available.Contains(node.Id), state?.Revision ?? -1)).ToArray();
            ShanheMapRows.Add(new($"第 {group.Key + 1} 程", nodes));
        }
        RefreshShanheJourneyMap(definition, depths, state?.CurrentNodeId, completed);
        ShanheChoices.Clear();
        ShanheTacticCards.Clear();
        ShanheExtraSkillCards.Clear();
        if (CurrentShanheBuildState is { } buildState)
        {
            foreach (var id in buildState.TacticIds)
            {
                var tactic = definition.Rewards.Single(reward => reward.Id == id);
                var ruleText = tactic.TacticSkillId is { } skillId && CurrentPresentationRegistry.Skills.TryGetValue(skillId, out var skill)
                    ? skill.Description : string.Empty;
                ShanheTacticCards.Add(new(id, tactic.Name, tactic.Description, ruleText,
                    tactic.OpeningCompanionPool is not null ? "初始战法 · 出征时获得同伴 · 本次征程固定"
                        : tactic.OpeningSkillRewardIds is not null ? "初始战法 · 出征时触发一次 · 获得的技能占额外技能槽"
                        : "战法 · 本次征程持续 · 每战作为技能获得 · 不占额外技能槽"));
            }
            foreach (var id in buildState.ExtraSkillIds)
            {
                var skill = CurrentPresentationRegistry.Skills.GetValueOrDefault(id);
                ShanheExtraSkillCards.Add(new(id, skill?.Name ?? id, skill?.Description ?? "当前获得的额外技能。",
                    string.Empty, "额外所学技能 · 占用 1 个技能槽 · 满槽时可被替换"));
            }
        }
        if (state?.Phase == ShanheCampaignPhase.Reward)
            foreach (var reward in state.RewardOffers) AddShanheChoice(reward.Id, reward.Name, reward.Description, reward, true, true);
        if (state?.Phase == ShanheCampaignPhase.NodeChoice)
            foreach (var choice in state.NodeChoices)
            {
                var reward = choice.RewardId is { } id ? definition.Rewards.Single(item => item.Id == id) : null;
                var cost = (choice.GoldCost == 0 ? "" : $"{choice.GoldCost} 铜币 · ") +
                    (choice.HpCost == 0 ? "" : $"{choice.HpCost} 体力 · ");
                AddShanheChoice(choice.Id, cost + choice.Name,
                    ShanheStoryChoiceDescription(choice, reward),
                    reward, false,
                    state.Gold >= choice.GoldCost && state.Hp > choice.HpCost);
            }
        RefreshShanheModules();
        RefreshShanheGuideTasks();
        RefreshShanheJourney();
        RefreshShanheRewards();
        RefreshShanheSettlement();
        ValidateShanheSkillReplacement();
        foreach (var property in new[] { nameof(ShanheCampaignTitle), nameof(ShanheRouteSummary), nameof(ShanheVitalText),
            nameof(ShanheGoldText), nameof(ShanheTacticsText), nameof(ShanheSkillsText), nameof(ShanheRecruitedCompanionCards), nameof(IsShanheStoryEventOpen), nameof(ShanheRankStatusText), nameof(SelectedShanheRankDescription), nameof(ShanheContextTitle),
            nameof(ShanheContextDescription), nameof(ShanheScenarioDetails), nameof(IsShanheBattleActive), nameof(IsShanheBattleSession),
            nameof(ShanheBuildTabTitle), nameof(ShanheTacticEmptyText), nameof(ShanheStartingLoadoutText), nameof(ShanheCarryoverNotice), nameof(ShanheEditionNotice), nameof(SelectedShanheGeneralMeta), nameof(HasShanheSession), nameof(CanSaveShanheProgress), nameof(CanContinueShanheExpedition),
            nameof(ShanheStartingCompanionCards), nameof(ShanheStartingCompanionName), nameof(ShanheStartingCompanionPortrait), nameof(ShanheStartingCompanionDescription), nameof(HasShanheStartingCompanion),
            nameof(IsShanheBattleRetryReady), nameof(ShanheBattleActionText), nameof(ShanheBattleRetryTitle), nameof(ShanheBattleRetryDescription) }) RaisePropertyChanged(property);
        foreach (var command in new[] { StartShanheCommand, ContinueShanheCommand, ResumeShanheBattleCommand, EndShanheCommand, SaveGameCommand, ToggleShanheGeneralRosterCommand })
            (command as RelayCommand)?.NotifyCanExecuteChanged();
    }

    private void RefreshShanheJourneyMap(ShanheCampaignDefinition definition, IReadOnlyDictionary<string, int> depths,
        string? currentNodeId, IReadOnlySet<string> completed) =>
        BuildShanheChapterRouteMap(definition, depths, currentNodeId, completed);

    private static string ShanheMapShortName(ShanheNodeDefinition node) =>
        node.Id.StartsWith("source-region-", StringComparison.Ordinal) && node.Kind == ShanheNodeKind.EliteBattle
            ? "精锐军营" : node.Id switch
    {
        "hulao-start" => node.Kind == ShanheNodeKind.Event ? "北路出征" : node.Name,
        "guangcheng-start" => node.Kind == ShanheNodeKind.Event ? "南路出征" : node.Name, "hulao" => "虎牢关",
        "guangcheng" => "广成关", "huaxiong" => "汜水关", "dagu" => "大谷关", "huanyuan" => "轘辕关",
        "lijue" => "西凉铁骑", "guosi" => "乱军追击", "finalcamp" => "最后营火", "caravan" => "失散商旅",
        "luoyang" => "洛阳", "suanzao" => "酸枣", "henei" => "河内", _ => node.Name.Split('·').Last().Trim()
    };

    private void AddShanheChoice(string id, string name, string description, ShanheRewardDefinition? reward, bool isReward, bool enabled)
    {
        var state = _shanheSession!.Snapshot;
        if (reward?.SkillId is { } learned && state.ExtraSkillIds.Contains(learned, StringComparer.Ordinal))
        {
            ShanheChoices.Add(new(id, name + " · 已习得", description, isReward, null, false, state.Revision));
            return;
        }
        if (reward?.SkillId is not null && state.ExtraSkillIds.Count >= state.ExtraSkillSlots + reward.AddSkillSlots)
        {
            foreach (var replaced in state.ExtraSkillIds)
                ShanheChoices.Add(new(id, $"{name} · 替换{ShanheSkillName(replaced)}", description, isReward, replaced, enabled, state.Revision));
            return;
        }
        ShanheChoices.Add(new(id, name, description, isReward, null, enabled, state.Revision));
    }

    private bool IsCurrentShanheNode(ShanheCampaignNode node) => node.IsAvailable &&
        _shanheSession?.Revision == node.Revision && ShanheMapRows.Any(row => row.Nodes.Any(current => ReferenceEquals(current, node)));

    private bool IsCurrentShanheChoice(ShanheCampaignChoice choice) => choice.IsEnabled &&
        _shanheSession?.Revision == choice.Revision && ShanheChoices.Any(current => ReferenceEquals(current, choice));

    private string ShanheGeneralName(string id) => StandardShanheCampaignCatalog.PlayerRosterRegistry.Generals.TryGetValue(id, out var general)
        ? general.Name : _contentRegistry.Generals.TryGetValue(id, out general) ? general.Name : id;
    private string ShanheSkillName(string id) => CurrentPresentationRegistry.Skills.TryGetValue(id, out var skill) ? skill.Name : id;
    private static string ShanheKindLabel(ShanheNodeKind kind) => kind switch
    { ShanheNodeKind.Battle => "交锋", ShanheNodeKind.EliteBattle => "精英", ShanheNodeKind.BossBattle => "决战", ShanheNodeKind.Event => "奇遇", ShanheNodeKind.Shop => "行商", ShanheNodeKind.Camp => "营地", _ => "征程" };
}

public sealed record ShanheScenarioOption(string Id, string Name);
public sealed record ShanheDifficultyOption(int Value, string Name);
public sealed record ShanheGeneralOption(string Id, string Name)
{
    public Brush Portrait => GeneralArt.GetPortrait(Id, decodeWidth: 300);
    public string SkillSummary { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Kingdom { get; init; } = string.Empty;
    public string EditionText => (Id.StartsWith("ol:", StringComparison.Ordinal) ? "OL · " :
        Id.StartsWith("mobile:", StringComparison.Ordinal) ? "移动版 · " :
        Id.StartsWith("anniversary:", StringComparison.Ordinal) ? "十周年 · " : string.Empty) + Presentation.GeneralGalleryCatalog.Classify(Id).Title;
    public string SearchText => $"{Name} {Id} {Kingdom} {EditionText} {SkillSummary}";
}
public sealed record ShanheMapRow(string Title, IReadOnlyList<ShanheCampaignNode> Nodes);
public sealed record ShanheJourneyNode(ShanheCampaignNode Node, double CenterX, double CenterY,
    ShanheNodeKind Kind, string ShortName, bool IsCurrent, bool IsCompleted)
{
    public double X => NativeLandmark?.MapX ?? CenterX - 68;
    public double Y => NativeLandmark?.MapY ?? CenterY - 44;
    public ShanheNativeMapLandmark? NativeLandmark { get; init; }
    public bool HasNativeArtwork => NativeLandmark is not null;
    public ImageSource? NativeArtwork => IsAvailable || IsCurrent && !IsCompleted ? NativeLandmark?.Artwork : NativeLandmark?.DormantArtwork;
    public double HitWidth => NativeLandmark is { } art ? Math.Max(90, art.MapWidth) : 136;
    public double HitHeight => NativeLandmark is { } art ? Math.Max(74, art.MapHeight) : 132;
    public Thickness BattleMarkerMargin => MarkerMargin(58, 58, new(28, -23, 0, 0));
    public Thickness EventMarkerMargin => MarkerMargin(43, 52, new(35, -23, 0, 0));
    public Thickness CampMarkerMargin => MarkerMargin(72, 50, new(21, -21, 0, 0));
    public Thickness BossMarkerMargin => MarkerMargin(88, 63, new(12, -24, 0, 0));
    public Thickness PlayerPinMargin => NativeLandmark is { } art ? new(art.MapWidth / 2 - 22.5, -59, 0, 0) : new(34, -72, 0, 0);
    public Thickness ExtraFlagMargin => NativeLandmark is { } art ? new(3, Math.Max(0, art.MapHeight - 24), 0, 0) : new(5, 100, 0, 0);
    public Thickness CompletedMarkMargin => NativeLandmark is { } art ? new(Math.Max(0, art.MapWidth - 22), Math.Max(0, art.MapHeight - 22), 0, 0) : new(79, 72, 0, 0);
    private Thickness MarkerMargin(double width, double height, Thickness fallback) => NativeLandmark is { } art
        ? new(art.MapWidth / 2 + 5 * ShanheMapArt.Scale - width / 2, art.MapHeight / 2 - 10 * ShanheMapArt.Scale - height / 2, 0, 0) : fallback;
    public bool IsPlayerLocation { get; init; }
    public bool IsBattle => Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle;
    public bool IsBoss => Kind == ShanheNodeKind.BossBattle;
    public bool IsExtraBattle { get; init; }
    public bool IsAlternativeBattle { get; init; }
    public bool IsMainline { get; init; }
    public string SceneLandmark { get; init; } = "City";
    public string StageLabel { get; init; } = string.Empty;
    public string RouteFlagText { get; init; } = string.Empty;
    public bool HasRouteFlag => IsAvailable && !string.IsNullOrWhiteSpace(RouteFlagText);
    public string RewardText { get; init; } = string.Empty;
    public string RouteSourceText { get; init; } = string.Empty;
    public string EnemyText { get; init; } = string.Empty;
    public string VerticalName => string.Join("\n", ShortName.Take(6));
    public bool IsAvailable => Node.IsAvailable;
    public string KindLabel => Node.KindLabel;
    public string StateLabel => IsCompleted ? IsBattle ? "已通关" : "已完成"
        : IsCurrent ? "进行中" : IsAvailable ? IsBattle ? "可挑战" : "可前往" : "未解锁";
    public string LocationLabel => Kind == ShanheNodeKind.Shop ? "集市" : $"第{StageLabel}站";
    public string AccessibleName => $"{Node.Name}，{KindLabel}，{StateLabel}" +
        (RouteSourceText.Length > 0 ? "，" + RouteSourceText : string.Empty);
    public string DetailText => IsAvailable || IsCurrent || IsCompleted
        ? $"{Node.Name}\n{StateLabel}" + (RouteSourceText.Length > 0 ? " · " + RouteSourceText : string.Empty) + "\n点击地点查看介绍。"
        : $"{Node.Name}\n尚未开放";
}
public sealed record ShanheJourneyLink(Geometry Route, bool IsCompleted, bool IsAvailable);
public sealed record ShanheBuildCard(string Id, string Name, string Description, string RuleDescription, string SourceText)
{
    public bool HasRuleDescription => !string.IsNullOrWhiteSpace(RuleDescription) && RuleDescription != Description;
    public string AccessibleText => $"{Name}。{Summary}";
    public string Summary => ShanheDisplayText.Effect(ShanheDisplayText.Rule(Description, RuleDescription));
    public string RuleTooltip => ShanheDisplayText.Rule(Description, RuleDescription);
}
public sealed record ShanheCampaignNode(string Id, string Name, string KindLabel, string Description,
    string StatusLabel, bool IsAvailable, long Revision)
{
    public string AccessibleName => $"{Name}，{KindLabel}，{StatusLabel}";
}
public sealed record ShanheCampaignChoice(string Id, string Name, string Description, bool IsReward,
    string? ReplaceSkillId, bool IsEnabled, long Revision)
{
    public string AccessibleName => $"{Name}，{Description}";
}

