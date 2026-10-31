#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreLeastHandMarkerDrawProgramsState : Contracts.ICoreLeastHandMarkerDrawProgramsMarkerExtraDrawPoliciesAccess
{
    // Issued permanent obligations are independent of later source availability;
    // resolution progress itself remains only on the exact draw owning frame.
    private Dictionary<(int Target, PlayerMarkerKind Marker), ProgramMarkerExtraDrawPolicy> _markerExtraDrawPolicies = [];
    internal ref readonly Dictionary<(int Target, PlayerMarkerKind Marker), ProgramMarkerExtraDrawPolicy> ReferenceToMarkerExtraDrawPolicies => ref _markerExtraDrawPolicies;

    ref readonly Dictionary<(int Target, PlayerMarkerKind Marker), ProgramMarkerExtraDrawPolicy> Contracts.ICoreLeastHandMarkerDrawProgramsMarkerExtraDrawPoliciesAccess._markerExtraDrawPolicies => ref _markerExtraDrawPolicies;
}
