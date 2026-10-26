namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    public SelectionAreaViewModel SelectionArea { get; private set; } = new();

    private void RefreshSelectionArea()
    {
        if (IsZhuqueFanSelectionPending)
        {
            SelectionArea = new()
            {
                Title = "朱雀羽扇", Hint = ZhuqueFanHint,
                Choices = _zhuqueFanSelection!.Choices, ChoiceCommand = ChooseZhuqueFanCommand
            };
            RaisePropertyChanged(nameof(SelectionArea));
            return;
        }
        SelectionArea = new()
        {
            Title = IsActiveSkillSelectionPending ? $"发动{HumanActiveSkillName}"
                : CurrentDecisionContext?.Title ?? PlayButtonText,
            Hint = ActionHint,
            ConfirmText = PlayButtonText,
            CancelText = ActionDockCancelText,
            CanConfirm = CanConfirmSelected,
            CanCancel = CanCancelAction,
            ShowConfirm = CanActFromHand,
            ShowEndTurn = CanEndTurn,
            ShowPrimaryButtons = ShowPrimaryActionButtons,
            ConfirmCommand = ConfirmSelectedCommand,
            CancelCommand = CancelActionCommand,
            ShowSelectAllHand = ShowSelectAllZhihengHand,
            CanSelectAllHand = CanSelectAllZhihengHand,
            SelectAllHandCommand = SelectAllZhihengHandCommand,
            Choices = DockSkillChoices,
            ChoiceCommand = DockSkillChoiceCommand,
            EnhancementOptions = CardEnhancementOptions,
            EnhancementCommand = ToggleCardEnhancementCommand,
            CatalogChoices = ExpandedSelectionChoices,
            CatalogCommand = CardNameSelectionCommand,
            CatalogTitle = CardNameSelectionTitle
        };
        RaisePropertyChanged(nameof(SelectionArea));
        ((RelayCommand)SelectAllZhihengHandCommand).NotifyCanExecuteChanged();
    }
}
