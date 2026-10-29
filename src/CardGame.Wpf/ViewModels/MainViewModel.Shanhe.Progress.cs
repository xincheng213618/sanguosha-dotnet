using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Persistence;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private bool HasLiveShanheBattle => CurrentShanheBattleState is { Status: not EngineStatus.Completed };
    public bool IsShanheBattleRetryReady => IsShanheBattleActive && !HasLiveShanheBattle;
    public string ShanheBattleActionText => IsShanheBattleRetryReady ? "重新挑战本关" : "返回战斗";
    public string ShanheBattleRetryTitle => CurrentShanheBattleState?.Status == EngineStatus.Completed
        ? "本关未通过，可重新挑战" : "关卡进度已保留";
    public string ShanheBattleRetryDescription =>
        "从进入本关时的体力、铜币和构筑重新挑战，之前的通关与领奖进度保留。每次挑战重新洗牌，开战前可随机换起手牌并确认。本关内的回合、消耗和交易重置。";

    private static ShanheCampaignSession RestoreShanheProgress(ShanheCampaignSaveFile save)
    {
        // Only the expedition journal is a resume boundary. Legacy in-flight Battle
        // checkpoints remain readable as data but are never resumed or executed.
        return ShanheCampaignSession.Restore(save.Campaign,
            StandardShanheCampaignCatalog.CreateDefinitionForCheckpoint(save.Campaign, save.Difficulty),
            StandardShanheCampaignCatalog.CreateBattleRegistry);
    }

    private void DiscardShanheBattleAttempt()
    {
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
    }

    private void LeaveShanheExpedition()
    {
        if (!SaveShanheProgress()) return;
        DiscardShanheBattleAttempt();
        _shanheWasAutoAdvance = false;
        IsShanheOpen = false;
        IsAutoAdvance = false;
        IsNewGameSetupOpen = true;
        RefreshShanhePresentation();
    }
}
