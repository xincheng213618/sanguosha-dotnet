#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineRedAdditionalTargetsState : Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsAccess, Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess
{
    private Dictionary<int, (CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> _redAdditionalTargetGrants = [];
    internal ref readonly Dictionary<int, (CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> ReferenceToRedAdditionalTargetGrants => ref _redAdditionalTargetGrants;

    private CardConversionSource? _selectedRedAdditionalTargetSource = default !;
    internal ref CardConversionSource? ReferenceToSelectedRedAdditionalTargetSource => ref _selectedRedAdditionalTargetSource;

    ref readonly Dictionary<int, (CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsAccess._redAdditionalTargetGrants => ref _redAdditionalTargetGrants;

    ref CardConversionSource? Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess._selectedRedAdditionalTargetSource => ref _selectedRedAdditionalTargetSource;

    ref readonly Dictionary<int, (CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess._redAdditionalTargetGrants => ref _redAdditionalTargetGrants;
}
