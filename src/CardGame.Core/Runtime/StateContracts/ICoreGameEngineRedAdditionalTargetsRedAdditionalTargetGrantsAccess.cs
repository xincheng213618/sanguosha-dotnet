#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, (global::CardGame.Core.CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> _redAdditionalTargetGrants { get; }
}
