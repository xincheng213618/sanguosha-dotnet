using System.IO;
using System.Text.Json;
using System.Windows.Input;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.Presentation;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private IGameSaveStore _saveStore = null!;
    private DispatcherTimer? _saveTimer;
    private bool _initializing = true;
    private bool _autoSavePending;
    private bool _sessionActivated;
    private bool _disposed;
    private bool _hasAutomaticSave;
    private bool _hasManualSave;
    private string _saveStatus = "本模式每次独立开局；结束后保留战绩，不保存续玩进度。";
    private string _automaticSaveText = string.Empty;
    private string _manualSaveText = string.Empty;
    private bool _hasSaveError;

    public bool HasAutomaticSave { get => _hasAutomaticSave; private set => SetProperty(ref _hasAutomaticSave, value); }
    public bool HasManualSave { get => _hasManualSave; private set => SetProperty(ref _hasManualSave, value); }
    public bool HasAnySave => HasAutomaticSave || HasManualSave;
    public bool CanSaveShanheProgress => HasShanheSession && (IsShanheOpen || IsShanheBattleSession) && !IsTutorialActive;
    public string SaveStatus
    {
        get => _saveStatus;
        private set { if (SetProperty(ref _saveStatus, value)) RaisePropertyChanged(nameof(SessionSaveStatus)); }
    }
    public string AutomaticSaveText { get => _automaticSaveText; private set => SetProperty(ref _automaticSaveText, value); }
    public string ManualSaveText { get => _manualSaveText; private set => SetProperty(ref _manualSaveText, value); }
    public bool HasSaveError { get => _hasSaveError; private set => SetProperty(ref _hasSaveError, value); }
    public ICommand SaveGameCommand { get; private set; } = null!;
    public ICommand LoadManualGameCommand { get; private set; } = null!;
    public ICommand ContinueGameCommand { get; private set; } = null!;

    private void InitializePersistence(IGameSaveStore? saveStore)
    {
        _saveStore = saveStore ?? new FileGameSaveStore();
        SaveGameCommand = new RelayCommand(() => { if (CanSaveShanheProgress) SaveShanheProgress(); }, () => CanSaveShanheProgress);
        // Ordinary matches retain history, but no longer expose resumable saves.
        // Keep the checkpoint readers below for legacy validation and diagnostics.
        LoadManualGameCommand = new RelayCommand(() => { }, () => false);
        ContinueGameCommand = new RelayCommand(() => { }, () => false);
        _saveTimer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1) };
        _saveTimer.Tick += OnSaveTick;
        RefreshSaveSlots();
    }

    private CommandResult SubmitCommand(GameCommand command)
    {
        command = PrepareWushuangClockCommand(command);
        command = PrepareNianClockCommand(command);
        if (!AllowsTutorialCommand(command))
        {
            var current = _game.State;
            return new CommandResult(false,
                new CommandError(CommandErrorCode.IllegalAction, "请完成当前教学目标，或先退出新手演练。"),
                _game.Revision,
                new EngineRunResult(current.Status, current.Winner, current, current.PendingDecision, _game.Revision));
        }
        var firstEvent = _game.Events.Count;
        var previousRevision = _game.Revision;
        CommandResult result;
        try { result = _game.Submit(command); }
        catch (Exception exception)
        {
            var committed = _game.Revision > previousRevision;
            RecordExceptionReport(exception, committed ? "SubmitCommand committed delivery" : "SubmitCommand",
                committed ? null : command);
            throw;
        }
        if (result.Accepted)
        {
            try
            {
                CaptureBattleFeedback(result.State);
                ObserveTutorialCommand(command, firstEvent);
            }
            finally
            {
                // The journal is committed once Submit returns. A presentation
                // observer failure must not prevent saving that accepted action.
                if (!_initializing && !IsTutorialActive)
                {
                    _sessionActivated = true;
                    CaptureShanheBattleResult();
                    QueueAutoSave();
                    if (!IsShanheBattleSession) CaptureCompletedHistory();
                }
                SyncNianClockAfterCommand();
                SyncWushuangClockAfterCommand();
                if (IsWushuangTable) RefreshWushuangPresentation();
            }
        }
        if (!result.Accepted)
        {
            ActionHint = $"操作未执行：{result.Error?.Message}";
        }
        return result;
    }

    private void QueueAutoSave()
    {
        if (_initializing || _disposed || !_sessionActivated || IsTutorialActive || !IsShanheBattleSession) return;
        _autoSavePending = true;
        // Coalesce at most one second of commands without starving saves during continuous AI play.
        if (_saveTimer is { IsEnabled: false }) _saveTimer.Start();
    }

    private void OnSaveTick(object? sender, EventArgs e)
    {
        _saveTimer?.Stop();
        FlushPendingSave();
    }

    /// <summary>Flush the current committed boundary before closing; failure leaves it pending for retry.</summary>
    public bool FlushPendingSave()
    {
        FlushWushuangClock();
        if (IsShanheBattleSession) return SaveShanheProgress();
        _saveTimer?.Stop();
        if (IsTutorialActive) return true;
        return !_autoSavePending || SaveCurrentGame(GameSaveSlot.Automatic);
    }

    private bool SaveCurrentGame(GameSaveSlot slot)
    {
        if (IsShanheBattleSession) return SaveShanheProgress();
        _autoSavePending = false;
        _saveTimer?.Stop();
        return true;
    }

    private void ContinueAutomaticGame(bool wushuang = false)
    {
        if (IsTutorialActive) return;
        if (!_game.IsFaulted && !FlushPendingSave()) return;
        LoadGame(GameSaveSlot.Automatic, wushuang);
    }

    private void LoadGame(GameSaveSlot slot, bool wushuang = false)
    {
        if (!SaveShanheBeforeOrdinaryGame()) return;
        try
        {
            var save = (wushuang ? _wushuangSaveStore! : _saveStore).Read(slot);
            if (wushuang && save.Checkpoint?.ModeId != WushuangMode.ModeId)
                throw new InvalidDataException("这个存档不是单骑无双对局。");
            if (save.FormatVersion != GameSaveFile.CurrentFormatVersion || save.Checkpoint?.Options is not { } options)
                throw new InvalidDataException("存档数据不完整或版本不受支持。");
            if (options.HumanSeat != 0)
                throw new InvalidDataException("这个存档不属于当前本地玩家座位。");
            var playbackSpeed = save.PlaybackSpeedId is null ? PlaybackSpeed.Normal
                : PlaybackSpeed.Find(save.PlaybackSpeedId) ?? throw new InvalidDataException("存档中的对局速度设置无效。");
            var registry = RegistryForSave(save);
            var registeredMode = registry.Modes.TryGetValue(save.Checkpoint.ModeId, out var checkpointMode)
                ? checkpointMode
                : throw new InvalidDataException("存档的模式不在内容注册表中。");
            if (options.PlayerCount < registeredMode.MinPlayers || options.PlayerCount > registeredMode.MaxPlayers)
                throw new InvalidDataException("存档的对局人数与模式定义不一致。");
            var mode = TableModes.SingleOrDefault(mode => mode.ModeId == save.Checkpoint.ModeId)
                ?? new TableModeOption(
                    options.PlayerCount,
                    registeredMode.Name,
                    "已从存档恢复的内容模式",
                    save.Checkpoint.ModeId);
            if (options.HumanRole is { } savedRole && !Enum.IsDefined(savedRole))
                throw new InvalidDataException("存档的玩家身份不受支持。");
            // Fully restore and validate a separate engine before replacing the active match.
            var restored = GameReplay.Restore(save.Checkpoint, registry);
            var team = registeredMode.ModeKind == ContentModeKind.Team
                ? StartingTeamsFor(registeredMode).SingleOrDefault(team => team.TeamId ==
                    restored.CreateSnapshot(0).Players.Single(player => player.IsHuman).TeamId)
                    ?? throw new InvalidDataException("存档的玩家队伍不受支持。")
                : null;
            _sessionActivated = true;
            _saveTimer?.Stop();
            _autoSavePending = false;
            ReplaceEngine(restored);
            _activeCustomGenerals = save.CustomGenerals is { } custom ? CustomGeneralContent.Snapshot(custom) : CustomGeneralLibrary.Empty;
            SelectedTableMode = mode;
            SelectedStartingRole = StartingRoles[0];
            if (registeredMode.WenheActivity is not null)
                SelectedWenheAspiration = WenheAspirations.Single(choice => choice.GoalRank ==
                    (restored.CreateSnapshot(0).WenheAspiration?.GoalRank ?? options.WenheAspirationRank ?? 1));
            if (registeredMode.GuanduActivity is not null)
                SelectedGuanduRole = GuanduRoles.Single(choice => choice.Role == (restored.CreateSnapshot(0).GuanduActivity?.HumanRole ?? options.HumanGuanduRole ?? GuanduSeatRole.General));
            if (team is not null) SelectedStartingTeam = team;
            ManualDiscardEnabled = options.UseInteractiveDiscard;
            IsAutoAdvance = save.AutoAdvance;
            SelectedPlaybackSpeed = playbackSpeed;
            IsNewGameSetupOpen = false;
            IsHelpOpen = false;
            IsLogOpen = false;
            HasSaveError = false;
            SaveStatus = $"已恢复{(slot == GameSaveSlot.Automatic ? "自动" : "手动")}存档 · {save.SavedAtUtc.ToLocalTime():MM-dd HH:mm}";
            _autoSavePending = true;
            _saveTimer?.Start();
            CaptureCompletedHistory();
            SyncNianClockAfterCommand();
            SyncWushuangClockAfterCommand();
        }
        catch (Exception exception)
        {
            // Start clean without autosaving over the file that failed to load.
            _saveTimer?.Stop();
            _autoSavePending = false;
            _sessionActivated = false;
            IsAutoAdvance = false;
            var wasInitializing = _initializing;
            try
            {
                _initializing = true;
                if (!_contentRegistry.Modes.ContainsKey(SelectedTableMode.ModeId))
                    SelectedTableMode = TableModes.First(mode => _contentRegistry.Modes.ContainsKey(mode.ModeId));
                NewGame();
            }
            finally { _initializing = wasInitializing; }
            IsNewGameSetupOpen = false;
            IsHelpOpen = false;
            IsLogOpen = false;
            HasSaveError = true;
            SaveStatus = $"读取失败：{ReadableSaveError(exception)} 已初始化新牌局，原存档未被覆盖。";
        }
    }

    private ContentRegistry RegistryForCheckpoint(GameCheckpoint checkpoint)
    {
        var packages = checkpoint.ContentPackages ?? [];
        var configuredPackages = _contentRegistry.Packages
            .Select(package => $"{package.Id}@{package.Version}")
            .ToArray();
        if (packages.SequenceEqual(configuredPackages, StringComparer.Ordinal) &&
            string.Equals(checkpoint.ContentHash, _contentRegistry.ContentHash, StringComparison.Ordinal))
        {
            return _contentRegistry;
        }

        var hasTeamModes = packages.Contains("standard-team-modes@1.0.0", StringComparer.Ordinal);
        var composedPackages = packages
            .Where(package => package.StartsWith("standard-composed-skills@", StringComparison.Ordinal))
            .ToArray();
        if (composedPackages.Length > 1 ||
            composedPackages.Length == 1 && composedPackages[0] != "standard-composed-skills@1.0.0")
        {
            throw new InvalidDataException("存档使用了当前版本不支持的技能组合内容包版本。");
        }
        var nationalZhangJiaoPackages = packages
            .Where(package => package.StartsWith("standard-national-zhang-jiao@", StringComparison.Ordinal))
            .ToArray();
        if (nationalZhangJiaoPackages.Length > 1 ||
            nationalZhangJiaoPackages.Length == 1 &&
            nationalZhangJiaoPackages[0] != "standard-national-zhang-jiao@1.0.0")
        {
            throw new InvalidDataException("存档使用了当前版本不支持的国战张角内容包版本。");
        }
        if (composedPackages.Length == 1)
        {
            return ComposedSkillContentRegistry.CreateShowcaseForCheckpoint(checkpoint);
        }
        if (nationalZhangJiaoPackages.Length == 1)
            throw new InvalidDataException("存档中的国战张角内容包组合不是当前桌面版支持的组合。");
        var hasRescueSkills = packages.Contains("standard-rescue-skills@1.0.0", StringComparer.Ordinal);
        var hasActiveSkills = packages.Contains("standard-active-skills@1.1.0", StringComparer.Ordinal);
        var classicPackageVersion = packages
            .Where(package => package.StartsWith("standard-classic-generals@", StringComparison.Ordinal))
            .Select(package => Version.Parse(package["standard-classic-generals@".Length..]))
            .SingleOrDefault();
        var hasClassicGenerals = classicPackageVersion is not null;
        var hasAmbitiousNational = packages.Contains("standard-national-war-ambitious@1.0.0", StringComparer.Ordinal);
        if (hasClassicGenerals)
        {
            if (classicPackageVersion != StandardClassicGeneralPackage.CurrentVersion)
                throw new InvalidDataException("存档使用了当前版本不支持的经典武将内容包版本。");
            return hasTeamModes && hasAmbitiousNational
                ? StandardContentRegistry.CreateWithClassicGeneralsAndTeamModesAndNationalWarAmbitious()
                : StandardContentRegistry.CreateWithClassicGenerals();
        }
        if (hasAmbitiousNational || packages.Contains("standard-national-war-lite@1.1.0", StringComparer.Ordinal))
        {
            if (hasAmbitiousNational)
            {
                if (hasRescueSkills && hasTeamModes) return StandardContentRegistry.CreateWithRescueSkillsAndTeamModesAndNationalWarAmbitious();
                if (hasRescueSkills) return StandardContentRegistry.CreateWithRescueSkillsAndNationalWarAmbitious();
                if (hasActiveSkills && hasTeamModes) return StandardContentRegistry.CreateWithActiveSkillsAndTeamModesAndNationalWarAmbitious();
                if (hasActiveSkills) return StandardContentRegistry.CreateWithActiveSkillsAndNationalWarAmbitious();
                if (hasTeamModes) return StandardContentRegistry.CreateWithTeamModesAndNationalWarAmbitious();
                return StandardContentRegistry.CreateWithNationalWarAmbitious();
            }
            if (hasRescueSkills && hasTeamModes) return StandardContentRegistry.CreateWithRescueSkillsAndTeamModesAndNationalWarLite();
            if (hasActiveSkills) return StandardContentRegistry.CreateWithActiveSkillsAndNationalWarLite();
            return StandardContentRegistry.CreateWithNationalWarLite();
        }
        if (hasRescueSkills && hasTeamModes)
        {
            return StandardContentRegistry.CreateWithRescueSkillsAndTeamModes();
        }

        if (hasRescueSkills)
        {
            return StandardContentRegistry.CreateWithRescueSkills();
        }

        if (hasActiveSkills && hasTeamModes)
        {
            return StandardContentRegistry.CreateWithActiveSkillsAndTeamModes();
        }

        if (hasActiveSkills)
        {
            return StandardContentRegistry.CreateWithActiveSkills();
        }

        return hasTeamModes
            ? StandardContentRegistry.CreateWithTeamModes()
            : StandardContentRegistry.Create();
    }

    private void ResetPersistenceMessage()
    {
        HasSaveError = false;
        SaveStatus = "本模式每次独立开局；结束后保留战绩，不保存续玩进度。";
    }

    private void RefreshSaveSlots()
    {
        // Do not probe or modify legacy match saves. Only expeditions can resume.
        HasAutomaticSave = HasManualSave = false;
        AutomaticSaveText = ManualSaveText = "本模式不提供续玩存档。";
        RaisePropertyChanged(nameof(HasAnySave));
        (ContinueGameCommand as RelayCommand)?.NotifyCanExecuteChanged();
        (LoadManualGameCommand as RelayCommand)?.NotifyCanExecuteChanged();
        RaisePropertyChanged(nameof(CanSaveShanheProgress));
        (SaveGameCommand as RelayCommand)?.NotifyCanExecuteChanged();
    }

    private static bool IsSaveException(Exception exception) => exception is IOException or InvalidDataException or UnauthorizedAccessException
        or JsonException or InvalidOperationException or ArgumentException or NotSupportedException or KeyNotFoundException;

    private static string ReadableSaveError(Exception exception) => exception switch
    {
        UnauthorizedAccessException => "没有访问存档目录的权限。",
        FileNotFoundException or DirectoryNotFoundException => "没有找到存档文件。",
        InvalidDataException => exception.Message,
        JsonException or NotSupportedException => "文件格式损坏或不受支持。",
        IOException => "文件暂时无法读写。",
        _ => "存档与当前规则或内容不兼容。"
    };
}
