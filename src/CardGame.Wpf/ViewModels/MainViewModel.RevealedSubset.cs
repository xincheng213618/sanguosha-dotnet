using CardGame.Core;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private readonly HashSet<int> _revealedSubsetCardIds = [];
    private GameEngine? _revealedSubsetGame;
    private PromptId? _revealedSubsetPromptId;
    private GameSnapshot? _revealedSubsetSnapshot;
    private bool _hasRevealedSubsetPrompt;

    // The engine's published sets remain the authority for every combined constraint.
    public bool IsRevealedSubsetSelectionPending
    {
        get
        {
            if (ReferenceEquals(_revealedSubsetSnapshot, _snapshot)) return _hasRevealedSubsetPrompt;
            _revealedSubsetSnapshot = _snapshot;
            _hasRevealedSubsetPrompt = false;
            if (_snapshot?.PendingDecision is not { SkillPrompt: not null, Choices.Count: > 0 } prompt ||
                prompt.PlayerSeat != _snapshot.HumanSeat) return false;
            var visible = RevealedSubsetCandidates.Select(card => card.Id).ToHashSet();
            return _hasRevealedSubsetPrompt = prompt.Choices.All(choice =>
                choice.Parameters.GetValueOrDefault("program-action") == "select-subset" &&
                choice.Targets.Count == 0 && choice.ContentIds.Count == 0 && choice.Cards.All(visible.Contains));
        }
    }

    private IEnumerable<CardSnapshot> RevealedSubsetCandidates =>
        _snapshot.PublicRevealedCards.Concat(_snapshot.PrivateRevealedCards ?? []).DistinctBy(card => card.Id);

    private PromptChoice? SelectedRevealedSubsetChoice => IsRevealedSubsetSelectionPending
        ? _snapshot.PendingDecision!.Choices.FirstOrDefault(choice => _revealedSubsetCardIds.SetEquals(choice.Cards)) : null;
    private PromptChoice? EmptyRevealedSubsetChoice => IsRevealedSubsetSelectionPending
        ? _snapshot.PendingDecision!.Choices.FirstOrDefault(choice => choice.Cards.Count == 0) : null;
    private string RevealedSubsetSelectionHint =>
        $"已选 {_revealedSubsetCardIds.Count} 张 · 点数和 {RevealedSubsetCandidates.Where(card => _revealedSubsetCardIds.Contains(card.Id)).Sum(card => card.Rank)} · {_snapshot.PendingDecision!.SkillPrompt!.Instructions}" +
        (EmptyRevealedSubsetChoice is not null ? "确认获得；取消不选。" : "选满后确认。");

    private void SyncRevealedSubsetSelection()
    {
        var prompt = IsRevealedSubsetSelectionPending ? _snapshot.PendingDecision : null;
        if (!ReferenceEquals(_revealedSubsetGame, _game) || _revealedSubsetPromptId != prompt?.PromptId ||
            prompt is null || !prompt.Choices.Any(choice => _revealedSubsetCardIds.IsSubsetOf(choice.Cards)))
            _revealedSubsetCardIds.Clear();
        _revealedSubsetGame = _game;
        _revealedSubsetPromptId = prompt?.PromptId;
        RaisePropertyChanged(nameof(IsRevealedSubsetSelectionPending));
    }

    private bool CanToggleRevealedSubsetCard(CardViewModel card) => IsRevealedSubsetSelectionPending &&
        PublicRevealedCards.Concat(PrivatelyViewedCards).Any(current => ReferenceEquals(current, card)) &&
        (_revealedSubsetCardIds.Contains(card.Id) || _snapshot.PendingDecision!.Choices.Any(choice =>
            choice.Cards.Contains(card.Id) && _revealedSubsetCardIds.IsSubsetOf(choice.Cards)));

    private void ToggleRevealedSubsetCard(CardViewModel card)
    {
        if (!CanToggleRevealedSubsetCard(card)) return;
        if (!_revealedSubsetCardIds.Remove(card.Id)) _revealedSubsetCardIds.Add(card.Id);
        RefreshRevealedSubsetDraft();
    }

    private void RefreshRevealedSubsetDraft()
    {
        foreach (var card in PublicRevealedCards.Concat(PrivatelyViewedCards))
            card.IsSelected = _revealedSubsetCardIds.Contains(card.Id);
        ((RelayCommand<CardViewModel>)SelectRevealedCardCommand).NotifyCanExecuteChanged();
        RefreshSelectionHint();
    }

    private void ConfirmRevealedSubset()
    {
        if (SelectedRevealedSubsetChoice is { } choice)
            SubmitPublishedPromptChoice(choice, IsRevealedSubsetSelectionPending, "展示牌选择未执行");
    }
}
