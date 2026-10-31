#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineCardConversionsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceSelectedResponseConversionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedResponseConversionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedUseConversionAccess
{
    private bool _hasSelectedResponseConversionChoice = default !;
    internal ref bool ReferenceToHasSelectedResponseConversionChoice => ref _hasSelectedResponseConversionChoice;

    private bool _hasSelectedUseConversionChoice = default !;
    internal ref bool ReferenceToHasSelectedUseConversionChoice => ref _hasSelectedUseConversionChoice;

    private global::CardGame.Core.CardConversionSource? _selectedResponseConversion = default !;
    internal ref global::CardGame.Core.CardConversionSource? ReferenceToSelectedResponseConversion => ref _selectedResponseConversion;

    private global::CardGame.Core.CardConversionSource? _selectedUseConversion = default !;
    internal ref global::CardGame.Core.CardConversionSource? ReferenceToSelectedUseConversion => ref _selectedUseConversion;

    ref global::CardGame.Core.CardConversionSource? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess._selectedUseConversion => ref _selectedUseConversion;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess._hasSelectedUseConversionChoice => ref _hasSelectedUseConversionChoice;

    ref global::CardGame.Core.CardConversionSource? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion => ref _selectedUseConversion;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedUseConversionChoice => ref _hasSelectedUseConversionChoice;

    ref global::CardGame.Core.CardConversionSource? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion => ref _selectedResponseConversion;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedResponseConversionChoice => ref _hasSelectedResponseConversionChoice;

    ref global::CardGame.Core.CardConversionSource? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion => ref _selectedUseConversion;

    ref global::CardGame.Core.CardConversionSource? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion => ref _selectedResponseConversion;

    ref global::CardGame.Core.CardConversionSource? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceSelectedResponseConversionAccess._selectedResponseConversion => ref _selectedResponseConversion;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceSelectedResponseConversionAccess._hasSelectedResponseConversionChoice => ref _hasSelectedResponseConversionChoice;

    ref global::CardGame.Core.CardConversionSource? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedResponseConversionAccess._selectedResponseConversion => ref _selectedResponseConversion;

    ref global::CardGame.Core.CardConversionSource? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedUseConversionAccess._selectedUseConversion => ref _selectedUseConversion;
}
