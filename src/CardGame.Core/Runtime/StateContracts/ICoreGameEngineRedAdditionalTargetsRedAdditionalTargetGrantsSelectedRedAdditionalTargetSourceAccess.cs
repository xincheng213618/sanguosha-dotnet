#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess : IProgramRuntimeOperations
{
    ref CardConversionSource? _selectedRedAdditionalTargetSource { get; }

    ref readonly Dictionary<int, (CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> _redAdditionalTargetGrants { get; }
}
