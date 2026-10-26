using System.Collections.ObjectModel;
using System.Windows.Input;
using CardGame.Core;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private GameEngine? _cardExchangeGame;
    private PromptId? _cardExchangePromptId;
    private CardSnapshot[] _cardExchangeTopSource = [];
    private CardSnapshot[] _cardExchangeHandSource = [];
    private int? _cardExchangeTopId;
    private int? _cardExchangeHandId;
    private readonly HashSet<int> _cardExchangeDiscards = [];
    public ObservableCollection<CardViewModel> ExchangeTopCards { get; } = [];
    public ObservableCollection<CardViewModel> ExchangeHandCards { get; } = [];
    public ICommand SelectCardExchangeCardCommand { get; private set; } = null!;

    public bool IsCardExchangePending => _snapshot?.PendingDecision is { IsPrivate: true, Choices.Count: > 0 } prompt &&
        prompt.PlayerSeat == _snapshot.HumanSeat && prompt.Choices.All(choice =>
            choice.Parameters.GetValueOrDefault("program-action") == "wushuang-original" &&
            choice.Parameters.GetValueOrDefault("option") is "guanxu-exchange" or "guanxu-discard" or "finish") &&
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option") is "guanxu-exchange" or "guanxu-discard");
    public bool IsCardExchangeDiscardPending => IsCardExchangePending &&
        _snapshot.PendingDecision!.Choices[0].Parameters.GetValueOrDefault("option") == "guanxu-discard";
    public bool IsCardExchangeViewPending => IsCardExchangePending && !IsCardExchangeDiscardPending;
    public string ExchangeTopLabel => "牌堆顶五张 · 从左到右依次先摸 · 仅你可见";
    public string ExchangeHandLabel => IsCardExchangePending && _snapshot.PendingDecision!.Choices.FirstOrDefault(choice => choice.Targets.Count == 1) is { } target
        ? $"{_snapshot.Players.Single(player => player.Seat == target.Targets[0]).GeneralName}的手牌 · 仅你可见" : "目标的手牌";
    private bool HasCardExchangeDraft => _cardExchangeTopId is not null || _cardExchangeHandId is not null || _cardExchangeDiscards.Count > 0;
    private string CardExchangeHint => IsCardExchangeDiscardPending
        ? $"第二步：选择三张同花色手牌弃置 · 已选 {_cardExchangeDiscards.Count}/3 张。"
        : _cardExchangeTopId is not null && _cardExchangeHandId is not null
            ? "第一步：已预览原位交换，其余牌顺序不变；确认后检查三张同花色弃牌。"
            : "第一步：在上下两排各选一张牌，原位交换；也可直接结束观看。";
    private PromptChoice? SelectedCardExchangeChoice => !IsCardExchangePending ? null :
        _snapshot.PendingDecision!.Choices.FirstOrDefault(choice => IsCardExchangeDiscardPending
            ? _cardExchangeDiscards.SetEquals(choice.Cards)
            : _cardExchangeTopId is { } top && _cardExchangeHandId is { } hand
                ? choice.Cards.SequenceEqual([hand, top])
                : !HasCardExchangeDraft && choice.Parameters.GetValueOrDefault("option") == "finish");
    private string CardExchangeConfirmText => IsCardExchangeDiscardPending ? $"弃置 {_cardExchangeDiscards.Count}/3 张"
        : HasCardExchangeDraft ? "确认交换" : "结束观看";

    private void InitializeCardExchange() => SelectCardExchangeCardCommand = new RelayCommand<CardViewModel>(
        SelectCardExchangeCard, CanSelectCardExchangeCard);

    private void SyncCardExchange()
    {
        var prompt = IsCardExchangePending ? _snapshot.PendingDecision : null;
        if (!ReferenceEquals(_cardExchangeGame, _game) || _cardExchangePromptId != prompt?.PromptId)
        {
            _cardExchangeTopId = null; _cardExchangeHandId = null; _cardExchangeDiscards.Clear();
            var faces = (_snapshot.PrivateRevealedCards ?? []).ToDictionary(card => card.Id);
            _cardExchangeTopSource = prompt is not null && !IsCardExchangeDiscardPending
                ? prompt.Choices.Where(choice => choice.Cards.Count == 2).Select(choice => choice.Cards[1]).Distinct()
                    .Where(faces.ContainsKey).Select(id => faces[id]).ToArray() : [];
            _cardExchangeHandSource = prompt is null ? [] : IsCardExchangeDiscardPending
                ? (_snapshot.PrivateRevealedCards ?? []).ToArray()
                : prompt.Choices.Where(choice => choice.Cards.Count == 2).Select(choice => choice.Cards[0]).Distinct()
                    .Where(faces.ContainsKey).Select(id => faces[id]).ToArray();
            RebuildCardExchangeRows();
        }
        _cardExchangeGame = _game; _cardExchangePromptId = prompt?.PromptId;
        RaisePropertyChanged(nameof(IsCardExchangePending)); RaisePropertyChanged(nameof(IsCardExchangeDiscardPending));
        RaisePropertyChanged(nameof(IsCardExchangeViewPending)); RaisePropertyChanged(nameof(ExchangeHandLabel));
        (SelectCardExchangeCardCommand as RelayCommand<CardViewModel>)?.NotifyCanExecuteChanged();
    }

    private bool CanSelectCardExchangeCard(CardViewModel card) => IsCardExchangePending &&
        ExchangeTopCards.Concat(ExchangeHandCards).Any(current => ReferenceEquals(current, card)) &&
        (!IsCardExchangeDiscardPending || _cardExchangeDiscards.Contains(card.Id) ||
            _snapshot.PendingDecision!.Choices.Any(choice => choice.Cards.Contains(card.Id) && _cardExchangeDiscards.IsSubsetOf(choice.Cards)));

    private void SelectCardExchangeCard(CardViewModel card)
    {
        if (!CanSelectCardExchangeCard(card)) return;
        if (IsCardExchangeDiscardPending)
        {
            if (!_cardExchangeDiscards.Remove(card.Id)) _cardExchangeDiscards.Add(card.Id);
        }
        else if (_cardExchangeTopSource.Any(face => face.Id == card.Id))
            _cardExchangeTopId = _cardExchangeTopId == card.Id ? null : card.Id;
        else if (_cardExchangeHandSource.Any(face => face.Id == card.Id))
            _cardExchangeHandId = _cardExchangeHandId == card.Id ? null : card.Id;
        RebuildCardExchangeRows(); RefreshSelectionHint();
    }

    private void RebuildCardExchangeRows()
    {
        ExchangeTopCards.Clear(); ExchangeHandCards.Clear();
        var swapped = _cardExchangeTopId is not null && _cardExchangeHandId is not null;
        foreach (var face in _cardExchangeTopSource)
            ExchangeTopCards.Add(View(swapped && face.Id == _cardExchangeTopId
                ? _cardExchangeHandSource.Single(card => card.Id == _cardExchangeHandId) : face));
        foreach (var face in _cardExchangeHandSource)
            ExchangeHandCards.Add(View(swapped && face.Id == _cardExchangeHandId
                ? _cardExchangeTopSource.Single(card => card.Id == _cardExchangeTopId) : face));
        (SelectCardExchangeCardCommand as RelayCommand<CardViewModel>)?.NotifyCanExecuteChanged();
        CardViewModel View(CardSnapshot face)
        {
            var card = CreateViewedCard(face, true);
            card.IsSelected = _cardExchangeDiscards.Contains(face.Id) || face.Id == _cardExchangeTopId || face.Id == _cardExchangeHandId;
            return card;
        }
    }

    private void ClearCardExchangeSelection()
    {
        _cardExchangeTopId = null; _cardExchangeHandId = null; _cardExchangeDiscards.Clear();
        RebuildCardExchangeRows(); RefreshSelectionHint();
    }
    private void ConfirmCardExchange()
    {
        if (SelectedCardExchangeChoice is { } choice)
            SubmitPublishedPromptChoice(choice, IsCardExchangePending, "观虚选择未执行");
    }
}
