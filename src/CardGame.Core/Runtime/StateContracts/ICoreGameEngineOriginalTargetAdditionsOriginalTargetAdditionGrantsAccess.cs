#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineOriginalTargetAdditionsOriginalTargetAdditionGrantsAccess : IProgramRuntimeOperations
{
    ref readonly List<OriginalTargetAdditionGrant> _originalTargetAdditionGrants { get; }
}
