using System.Windows.Input;
using CardGame.Core;

namespace CardGame.Wpf.ViewModels;

public sealed class CardEnhancementOptionViewModel(PromptChoice choice, CurrentCardEnhancement enhancement) : ObservableObject
{
    private bool _isSelected;
    public PromptChoice Choice { get; } = choice;
    public CurrentCardEnhancement Enhancement { get; } = enhancement;
    public string Description => Choice.Description;
    public bool IsSelected { get => _isSelected; internal set => SetProperty(ref _isSelected, value); }
}

public sealed partial class MainViewModel
{
    private PromptId? _cardEnhancementPromptId;
    private GameEngine? _cardEnhancementGame;
    private readonly HashSet<CurrentCardEnhancement> _selectedCardEnhancements = [];
    private ICommand? _toggleCardEnhancementCommand;
    public IReadOnlyList<CardEnhancementOptionViewModel> CardEnhancementOptions { get; private set; } = [];
    public bool IsCardEnhancementSelectionPending => _snapshot?.PendingDecision is { PlayerSeat: var seat } prompt &&
        seat == _snapshot.HumanSeat && prompt.Choices.Any(choice =>
            choice.Parameters.GetValueOrDefault("program-action") == "current-card-enhancement" &&
            choice.Parameters.GetValueOrDefault("enhancement-option") == "confirm");
    private int CardEnhancementMaximum => IsCardEnhancementSelectionPending ? int.Parse(_snapshot.PendingDecision!.Choices
        .First(choice => choice.Parameters.ContainsKey("maximum-options")).Parameters["maximum-options"], System.Globalization.CultureInfo.InvariantCulture) : 0;
    private PromptChoice? SelectedCardEnhancementChoice
    {
        get
        {
            if (!IsCardEnhancementSelectionPending) return null;
            var flags = _selectedCardEnhancements.Aggregate(CurrentCardEnhancement.None, (value, flag) => value | flag);
            return _snapshot.PendingDecision!.Choices.SingleOrDefault(choice => flags == CurrentCardEnhancement.None
                ? choice.Parameters.GetValueOrDefault("enhancement-option") == "finish"
                : choice.Parameters.GetValueOrDefault("enhancement-selection") == ((int)flags).ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
    }

    public ICommand ToggleCardEnhancementCommand => _toggleCardEnhancementCommand ??=
        new RelayCommand<CardEnhancementOptionViewModel>(option =>
        {
            if (!ToggleCardEnhancementCommand.CanExecute(option)) return;
            if (!_selectedCardEnhancements.Remove(option.Enhancement)) _selectedCardEnhancements.Add(option.Enhancement);
            foreach (var current in CardEnhancementOptions) current.IsSelected = _selectedCardEnhancements.Contains(current.Enhancement);
            RefreshSelectionHint();
            ((RelayCommand<CardEnhancementOptionViewModel>)ToggleCardEnhancementCommand).NotifyCanExecuteChanged();
        }, option => IsCardEnhancementSelectionPending && CardEnhancementOptions.Any(current => ReferenceEquals(current, option)) &&
            IsCurrentPromptChoice(option.Choice) && (option.IsSelected || _selectedCardEnhancements.Count < CardEnhancementMaximum));

    private void SyncCardEnhancementSelection()
    {
        var prompt = _snapshot.PendingDecision;
        if (!IsCardEnhancementSelectionPending || _cardEnhancementPromptId != prompt!.PromptId ||
            !ReferenceEquals(_cardEnhancementGame, _game)) _selectedCardEnhancements.Clear();
        _cardEnhancementGame = IsCardEnhancementSelectionPending ? _game : null;
        _cardEnhancementPromptId = IsCardEnhancementSelectionPending ? prompt!.PromptId : null;
        CardEnhancementOptions = IsCardEnhancementSelectionPending ? prompt!.Choices
            .Where(choice => Enum.TryParse<CurrentCardEnhancement>(choice.Parameters.GetValueOrDefault("enhancement-option"), out _))
            .Select(choice => new CardEnhancementOptionViewModel(choice, Enum.Parse<CurrentCardEnhancement>(choice.Parameters["enhancement-option"]))
                { IsSelected = _selectedCardEnhancements.Contains(Enum.Parse<CurrentCardEnhancement>(choice.Parameters["enhancement-option"])) }).ToArray() : [];
        RaisePropertyChanged(nameof(IsCardEnhancementSelectionPending));
        RaisePropertyChanged(nameof(CardEnhancementOptions));
        ((RelayCommand<CardEnhancementOptionViewModel>)ToggleCardEnhancementCommand).NotifyCanExecuteChanged();
    }

    private void ClearCardEnhancementSelection()
    {
        _selectedCardEnhancements.Clear();
        foreach (var option in CardEnhancementOptions) option.IsSelected = false;
        RefreshSelectionHint();
        ((RelayCommand<CardEnhancementOptionViewModel>)ToggleCardEnhancementCommand).NotifyCanExecuteChanged();
    }

    private void ConfirmCardEnhancements()
    {
        if (SelectedCardEnhancementChoice is { } choice)
            SubmitPublishedPromptChoice(choice, IsCardEnhancementSelectionPending, "技能效果选择未执行");
    }
}
