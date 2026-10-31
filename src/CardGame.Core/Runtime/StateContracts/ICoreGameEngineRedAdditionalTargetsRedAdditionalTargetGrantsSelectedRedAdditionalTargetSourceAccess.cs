#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.CardConversionSource? _selectedRedAdditionalTargetSource { get; }

    ref readonly global::System.Collections.Generic.Dictionary<int, (global::CardGame.Core.CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> _redAdditionalTargetGrants { get; }
}
