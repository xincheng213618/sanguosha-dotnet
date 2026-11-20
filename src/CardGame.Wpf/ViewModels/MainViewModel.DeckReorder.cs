using System.Collections.ObjectModel;
using CardGame.Core;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private GameEngine? _deckReorderGame;
    private DeckReorderView? _deckReorderView;
    public ObservableCollection<CardViewModel> DeckTopCards { get; } = [];
    public ObservableCollection<CardViewModel> DeckBottomCards { get; } = [];
    public bool IsDeckReorderPending => _snapshot?.PendingDecision is { DeckReorder: not null } prompt &&
        prompt.PlayerSeat == _snapshot.HumanSeat;
    public bool HasDeckTopRow => _deckReorderView is { BottomOnly: false };
    public string DeckTopLabel => _deckReorderView?.RequiredTopCount is { } count
        ? $"牌堆顶 · {DeckTopCards.Count}/{count} 张 · 从左到右依次先摸" : $"牌堆顶 · {DeckTopCards.Count} 张 · 从左到右依次先摸";
    public string DeckBottomLabel => $"牌堆底 · {DeckBottomCards.Count} 张 · 最右侧的牌在最底部";
    private bool CanConfirmDeckReorder => IsDeckReorderPending && (_deckReorderView!.RequiredTopCount is not { } count || DeckTopCards.Count == count);
    private string DeckReorderHint => _deckReorderView is { BottomOnly: true } ? "拖动调整底牌顺序，完成后确认。" : "左右拖动排序，上下拖动换排。" +
        (_deckReorderView?.RequiredTopCount is { } count ? $"牌堆顶须保留 {count} 张。" : string.Empty) + "完成后确认。";

    private void SyncDeckReorder()
    {
        var order = IsDeckReorderPending ? _snapshot.PendingDecision!.DeckReorder : null;
        var unchanged = ReferenceEquals(_deckReorderGame, _game) && order is not null && _deckReorderView is { } previous &&
            order.FrameId == previous.FrameId && order.ChoosingBottom == previous.ChoosingBottom &&
            order.TopCardIds.SequenceEqual(previous.TopCardIds) && order.BottomCardIds.SequenceEqual(previous.BottomCardIds) &&
            order.Cards.SequenceEqual(previous.Cards);
        _deckReorderGame = _game;
        _deckReorderView = order;
        if (!unchanged)
        {
            DeckTopCards.Clear(); DeckBottomCards.Clear();
            if (order is not null)
            {
                var used = order.TopCardIds.Concat(order.BottomCardIds).ToHashSet();
                var remaining = order.Cards.Where(card => !used.Contains(card.Id)).Select(card => card.Id).ToArray();
                foreach (var id in order.TopCardIds.Concat(order.ChoosingBottom ? [] : remaining))
                    DeckTopCards.Add(CreateViewedCard(order.Cards.Single(card => card.Id == id), true));
                foreach (var id in (order.ChoosingBottom ? remaining : []).Concat(order.BottomCardIds.Reverse()))
                    DeckBottomCards.Add(CreateViewedCard(order.Cards.Single(card => card.Id == id), true));
            }
        }
        RaisePropertyChanged(nameof(IsDeckReorderPending));
        NotifyDeckReorderChanged();
    }

    private ObservableCollection<CardViewModel>? DeckOrderRow(CardViewModel card) =>
        DeckTopCards.Contains(card) ? DeckTopCards : DeckBottomCards.Contains(card) ? DeckBottomCards : null;
    private bool IsLockedDeckOrderCard(CardViewModel card) => _deckReorderView is not { } order ||
        order.TopCardIds.Contains(card.Id) || order.BottomCardIds.Contains(card.Id);
    internal bool CanDragDeckOrderCard(CardViewModel card) =>
        IsDeckReorderPending && DeckOrderRow(card) is not null && !IsLockedDeckOrderCard(card);

    internal bool CanDropDeckOrderCard(CardViewModel card, bool bottom, int targetIndex) =>
        ResolveDeckOrderMove(card, bottom, targetIndex) is not null;

    private (ObservableCollection<CardViewModel> Source, ObservableCollection<CardViewModel> Destination, int OldIndex, int Index)?
        ResolveDeckOrderMove(CardViewModel card, bool bottom, int targetIndex)
    {
        if (!IsDeckReorderPending || _deckReorderView is not { } order ||
            DeckOrderRow(card) is not { } source || IsLockedDeckOrderCard(card)) return null;
        var destination = bottom ? DeckBottomCards : DeckTopCards;
        var sameRow = ReferenceEquals(source, destination);
        if (!sameRow && order.ChoosingBottom) return null;
        var index = Math.Clamp(targetIndex, 0, destination.Count);
        var oldIndex = source.IndexOf(card);
        if (sameRow && oldIndex < index) index--;
        // A recovered native prefix has already been accepted and stays fixed.
        var movableEnd = destination.Count - (sameRow ? 1 : 0) - order.BottomCardIds.Count;
        if (bottom ? index > movableEnd : index < order.TopCardIds.Count) return null;
        return (source, destination, oldIndex, index);
    }

    internal bool MoveDeckOrderCard(CardViewModel card, bool bottom, int targetIndex)
    {
        if (ResolveDeckOrderMove(card, bottom, targetIndex) is not { } move) return false;
        if (ReferenceEquals(move.Source, move.Destination))
        {
            if (move.OldIndex == move.Index) return true;
            move.Source.Move(move.OldIndex, move.Index);
        }
        else
        {
            move.Source.RemoveAt(move.OldIndex);
            move.Destination.Insert(move.Index, card);
        }
        NotifyDeckReorderChanged(); RefreshSelectionHint();
        return true;
    }

    private void NotifyDeckReorderChanged()
    {
        RaisePropertyChanged(nameof(DeckTopLabel)); RaisePropertyChanged(nameof(DeckBottomLabel)); RaisePropertyChanged(nameof(HasDeckTopRow));
    }

    private void ConfirmDeckReorder()
    {
        if (!CanConfirmDeckReorder) return;
        var order = _deckReorderView!;
        if (order.BottomOnly)
        {
            var ids = DeckBottomCards.Reverse().Select(card => card.Id).ToArray();
            if (_snapshot.PendingDecision!.Choices.SingleOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "order-bound-cards" &&
                choice.Cards.SequenceEqual(ids)) is { } choice)
                SubmitPublishedPromptChoice(choice, IsDeckReorderPending, "牌堆底排序未执行");
            return;
        }
        var top = DeckTopCards.Select(card => card.Id).Skip(order.TopCardIds.Count).ToArray();
        var bottom = DeckBottomCards.Reverse().Select(card => card.Id).Skip(order.BottomCardIds.Count).ToArray();
        ExecuteSafely(() =>
        {
            foreach (var id in top) if (!Submit("top", id)) return;
            if (bottom.Length > 0 && _snapshot.PendingDecision?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "finish-top") == true)
                if (!Submit("finish-top", null)) return;
            foreach (var id in bottom) if (!Submit("bottom", id)) return;
        });

        bool Submit(string action, int? cardId)
        {
            if (_snapshot.PendingDecision is not { DeckReorder: { } live } prompt || live.FrameId != order.FrameId) return false;
            var choice = prompt.Choices.SingleOrDefault(candidate => candidate.Parameters.GetValueOrDefault("action") == action &&
                (cardId is { } id ? candidate.Cards.SequenceEqual([id]) : candidate.Cards.Count == 0));
            if (choice is null) return false;
            var result = SubmitCommand(new AnswerPromptCommand(_snapshot.HumanSeat, prompt.PromptId, choice.Id, _snapshot.Revision));
            if (!result.Accepted) { PromptText = result.Error?.Message ?? "排序未执行"; return false; }
            RefreshCommandResult(result.State);
            return true;
        }
    }
}
