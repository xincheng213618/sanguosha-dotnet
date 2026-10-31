#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreLeastHandMarkerDrawProgramsMarkerExtraDrawPoliciesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<(int Target, global::CardGame.Core.PlayerMarkerKind Marker), global::CardGame.Core.ProgramMarkerExtraDrawPolicy> _markerExtraDrawPolicies { get; }
}
