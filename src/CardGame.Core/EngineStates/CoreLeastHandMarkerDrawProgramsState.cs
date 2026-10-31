#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreLeastHandMarkerDrawProgramsState : global::CardGame.Core.EngineStates.Contracts.ICoreLeastHandMarkerDrawProgramsMarkerExtraDrawPoliciesAccess
{
    // Issued permanent obligations are independent of later source availability;
    // resolution progress itself remains only on the exact draw owning frame.
    private global::System.Collections.Generic.Dictionary<(int Target, global::CardGame.Core.PlayerMarkerKind Marker), global::CardGame.Core.ProgramMarkerExtraDrawPolicy> _markerExtraDrawPolicies = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<(int Target, global::CardGame.Core.PlayerMarkerKind Marker), global::CardGame.Core.ProgramMarkerExtraDrawPolicy> ReferenceToMarkerExtraDrawPolicies => ref _markerExtraDrawPolicies;

    ref readonly global::System.Collections.Generic.Dictionary<(int Target, global::CardGame.Core.PlayerMarkerKind Marker), global::CardGame.Core.ProgramMarkerExtraDrawPolicy> global::CardGame.Core.EngineStates.Contracts.ICoreLeastHandMarkerDrawProgramsMarkerExtraDrawPoliciesAccess._markerExtraDrawPolicies => ref _markerExtraDrawPolicies;
}
