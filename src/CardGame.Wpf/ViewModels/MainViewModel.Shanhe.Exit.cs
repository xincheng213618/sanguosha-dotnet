using System.Windows.Input;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Persistence;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private ShanheCampaignSession? _shanheEndSession;
    private long _shanheEndRevision;
    private bool _isShanheEndConfirmationOpen;
    private string _shanheEndError = string.Empty;

    public bool IsShanheEndConfirmationOpen
    {
        get => _isShanheEndConfirmationOpen;
        private set => SetProperty(ref _isShanheEndConfirmationOpen, value);
    }
    public string ShanheEndError { get => _shanheEndError; private set => SetProperty(ref _shanheEndError, value); }
    public ICommand EndShanheCommand { get; private set; } = null!;
    public ICommand ConfirmEndShanheCommand { get; private set; } = null!;
    public ICommand CancelEndShanheCommand { get; private set; } = null!;

    private void InitializeShanheExit()
    {
        EndShanheCommand = new RelayCommand(() =>
        {
            if (!IsShanheRunInProgress || IsTutorialActive) return;
            _shanheEndSession = _shanheSession;
            _shanheEndRevision = _shanheSession!.Revision;
            ShanheEndError = string.Empty;
            IsShanheOpen = true;
            IsShanheEndConfirmationOpen = true;
        }, () => IsShanheRunInProgress && !IsTutorialActive);
        ConfirmEndShanheCommand = new RelayCommand(EndShanheExpedition);
        CancelEndShanheCommand = new RelayCommand(CancelEndShanhe);
    }

    private void CancelEndShanhe()
    {
        IsShanheEndConfirmationOpen = false;
        _shanheEndSession = null;
        ShanheEndError = string.Empty;
    }

    private void AttachShanheBattle(GameEngine battle, ShanheBattleSetup setup)
    {
        if (!IsShanheBattleSession) _shanheReturnGame = _game;
        _shanheTableGame = battle;
        _shanheTableSetup = setup;
        ReplaceEngine(battle);
    }

    private void EndShanheExpedition()
    {
        if (!IsShanheEndConfirmationOpen || !IsShanheRunInProgress || IsTutorialActive ||
            _shanheSession is not { } session || !ReferenceEquals(session, _shanheEndSession) ||
            session.Revision != _shanheEndRevision || _shanheSaveStore is null) return;
        ShanheCampaignSession ended;
        try
        {
            // Persist a detached terminal journal first. A failed write leaves the live
            // expedition and battle intact, so Cancel and Retry remain safe.
            ended = ShanheCampaignSession.Restore(session.CreateCheckpoint(), session.Definition,
                StandardShanheCampaignCatalog.CreateBattleRegistry);
            var result = ended.Submit(new EndShanheExpeditionCommand(ended.Revision));
            if (!result.Accepted) { ShanheEndError = result.Error ?? "征程暂时无法结束。"; return; }
            _shanheSaveStore.Write(new(ShanheCampaignSaveFile.CurrentFormatVersion,
                DateTimeOffset.UtcNow, _shanheRunDifficulty, ended.CreateCheckpoint()));
        }
        catch (Exception error) when (IsSaveException(error))
        {
            ShanheEndError = $"结束征程未保存：{ReadableSaveError(error)} 当前征程已保留，可重试或取消。";
            return;
        }
        _shanheSession = ended;
        CancelEndShanhe();
        CancelShanheSkillReplacement();
        CloseShanheEncounterPreview();
        IsShanheMarketOpen = false;
        if (IsShanheBattleSession && _shanheReturnGame is { } previous) ReplaceEngine(previous);
        _shanheBattleGame = _shanheTableGame = _shanheReturnGame = null;
        ResetShanheBattleProjection();
        _shanheTableSetup = null;
        _autoSavePending = false;
        _saveTimer?.Stop();
        HasSaveError = false;
        // Return directly to the outer preparation module; keep its normal
        // settlement receipt and record without reopening the completed battle.
        _dismissedShanheSettlement = ended;
        SettleShanheCodexReward();
        if (IsShanheSettlementPending) _dismissedShanheSettlement = null;
        RefreshShanhePresentation();
        ShanheStatusText = "本次征程已结束，可以重新选将、配置山河册。";
    }
}
