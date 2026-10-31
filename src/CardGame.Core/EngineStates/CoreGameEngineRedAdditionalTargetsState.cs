#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineRedAdditionalTargetsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess
{
    private global::System.Collections.Generic.Dictionary<int, (global::CardGame.Core.CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> _redAdditionalTargetGrants = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, (global::CardGame.Core.CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> ReferenceToRedAdditionalTargetGrants => ref _redAdditionalTargetGrants;

    private global::CardGame.Core.CardConversionSource? _selectedRedAdditionalTargetSource = default !;
    internal ref global::CardGame.Core.CardConversionSource? ReferenceToSelectedRedAdditionalTargetSource => ref _selectedRedAdditionalTargetSource;

    ref readonly global::System.Collections.Generic.Dictionary<int, (global::CardGame.Core.CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsAccess._redAdditionalTargetGrants => ref _redAdditionalTargetGrants;

    ref global::CardGame.Core.CardConversionSource? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess._selectedRedAdditionalTargetSource => ref _selectedRedAdditionalTargetSource;

    ref readonly global::System.Collections.Generic.Dictionary<int, (global::CardGame.Core.CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess._redAdditionalTargetGrants => ref _redAdditionalTargetGrants;
}
