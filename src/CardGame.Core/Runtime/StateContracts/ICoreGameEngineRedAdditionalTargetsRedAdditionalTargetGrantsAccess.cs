#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, (CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> _redAdditionalTargetGrants { get; }
}
