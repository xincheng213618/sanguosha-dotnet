#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreLeastHandMarkerDrawProgramsMarkerExtraDrawPoliciesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<(int Target, PlayerMarkerKind Marker), ProgramMarkerExtraDrawPolicy> _markerExtraDrawPolicies { get; }
}
