using System.Windows.Input;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Persistence;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private ShanheCampaignSession? _dismissedShanheSettlement;
    private string? _shanheSettlementError;
    public bool IsShanheSettlementOpen => _shanheSession is { } session &&
        session.Snapshot.Phase is ShanheCampaignPhase.Won or ShanheCampaignPhase.Lost && !ReferenceEquals(session, _dismissedShanheSettlement);
    public bool HasShanheBookReward => _shanheSession is { } session && ShanheCodexSettlement.ConfirmedBookAmount(session, _shanheRunDifficulty) > 0;
    public bool IsShanheSettlementPending => _shanheSession is { } session &&
        ShanheGuideTaskSettlement.HasPending(_shanheCodexProfile, session, _shanheRunDifficulty,
            StandardShanheSourceDifficultyCatalog.Season5.BySourceId.Keys);
    public string ShanheSettlementTitle => _shanheSession?.Snapshot.Phase == ShanheCampaignPhase.Won ? "胜 利" : "征 程 结 束";
    public string ShanheSettlementSubtitle => _shanheSession?.Snapshot.EndingText ?? "";
    public string ShanheSettlementBookText => _shanheSession is { } session ? $"× {ShanheCodexSettlement.ConfirmedBookAmount(session, _shanheRunDifficulty)}" : "";
    public string ShanheSettlementRewardRule => "通关获得10册图册";
    public IReadOnlyList<ShanheRunRecordView> ShanheRunRecords => _shanheCodexProfile.RunRecords
        .Select(record => new ShanheRunRecordView(record)).ToArray();
    public bool HasShanheRunRecords => _shanheCodexProfile.RunRecords.Length > 0;
    public string ShanheRunRecordSummary => HasShanheRunRecords
        ? $"最近 {_shanheCodexProfile.RunRecords.Length} 次征程 · {_shanheCodexProfile.RunRecords.Count(r => r.Won)} 次通关 · 最多保留100次"
        : "尚无征程记录，完成一段征程后会在这里留存。";
    public string ShanheSettlementReceipt => _shanheSettlementError ?? (IsShanheSettlementPending ? "结算未保存，请重试。"
        : _shanheSession is { } session && ShanheCodexSettlement.HasClaimed(_shanheCodexProfile, session)
        ? HasShanheBookReward ? "图册已存入山河册 · 征程记录已保存"
            : "征程记录已保存"
        : "可重新整备出征");
    private RelayCommand? _closeShanheSettlementCommand;
    public ICommand CloseShanheSettlementCommand => _closeShanheSettlementCommand ??= new RelayCommand(() =>
    {
        _dismissedShanheSettlement = _shanheSession;
        RefreshShanheSettlement();
    }, () => IsShanheSettlementOpen);
    private RelayCommand? _retryShanheSettlementCommand;
    public ICommand RetryShanheSettlementCommand => _retryShanheSettlementCommand ??= new RelayCommand(SettleShanheCodexReward,
        () => IsShanheSettlementPending);

    // The account receipt commits independently of the expedition journal and prevents duplicate credit.
    private void SettleShanheCodexReward()
    {
        if (_shanheSession is not { } session) return;
        _shanheSettlementError = null;
        if (_shanheCodexReadFailed && ShanheCodexSettlement.CanSettle(session, _shanheRunDifficulty))
            _shanheSettlementError = "山河册读取失败，图册与征程记录尚未入账；原文件已保留。";
        else if (!_shanheCodexReadFailed)
        {
            // Task receipts, their books and the ordinary completion receipt share
            // the existing atomic account write. A failed write credits none of them.
            if (ShanheGuideTaskSettlement.TryPrepareCombined(_shanheCodexProfile, session, _shanheRunDifficulty,
                StandardShanheSourceDifficultyCatalog.Season5.BySourceId.Keys, out var next,
                ShanheGeneralName(session.Snapshot.PlayerGeneralId)) &&
                !SaveCodexProfile(next, "征程进度已保存，当前共有 " + next.Books + " 册。"))
                _shanheSettlementError = "图册与征程进度尚未保存，本次未入账；可重试结算。";
        }
        RefreshShanheSettlement();
    }
    private void RefreshShanheSettlement()
    {
        foreach (var property in new[] { nameof(IsShanheSettlementOpen), nameof(IsShanheSettlementPending), nameof(HasShanheBookReward), nameof(ShanheSettlementTitle),
            nameof(ShanheSettlementSubtitle), nameof(ShanheSettlementBookText), nameof(ShanheSettlementReceipt),
            nameof(ShanheRunRecords), nameof(HasShanheRunRecords), nameof(ShanheRunRecordSummary) }) RaisePropertyChanged(property);
        _closeShanheSettlementCommand?.NotifyCanExecuteChanged();
        _retryShanheSettlementCommand?.NotifyCanExecuteChanged();
    }
}

public sealed record ShanheRunRecordView(ShanheRunRecord Record)
{
    public string Title => Record.CampaignName + " · " + (Record.Won ? "通关" : "未通关");
    public string Detail => $"{Record.PlayerGeneralName} · {(Record.SourceDifficultyId is { } sourceDifficulty ? ShanheGuideTaskCatalog.SourceDifficultyName(sourceDifficulty) : $"难度 {Record.Difficulty}")} · {Record.Battles}场战斗 · 已过 {Record.CompletedNodes} 处";
    public string Reward => Record.BooksEarned > 0 ? $"图册 +{Record.BooksEarned}" : "图册 +0";
    public string Timestamp => Record.CompletedAtUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
    public string Ending => Record.EndingText;
}
