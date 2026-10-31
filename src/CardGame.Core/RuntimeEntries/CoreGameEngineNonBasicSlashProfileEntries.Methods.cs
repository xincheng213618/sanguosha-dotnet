#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNonBasicSlashProfileEntries
{
    internal bool DistancePhysicalSlash80HasIssuedNonBasicSlashDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash80HasIssuedNonBasicSlashDistance(q);
    internal bool HasIssuedNonBasicSlashDistance(long? useId) => _resolve().HasIssuedNonBasicSlashDistance(useId);
    internal bool IgnoresNonBasicSlashDistance(CharacterState owner, CardConversionSource source) => _resolve().IgnoresNonBasicSlashDistance(owner, source);
    internal bool IgnoresNonBasicSlashDistanceQuery(ProgramSlashConversionDistanceQuery q) => _resolve().IgnoresNonBasicSlashDistanceQuery(q);
    internal void IssueNonBasicSlashPolicy(long useId, CardActionContext? action) => _resolve().IssueNonBasicSlashPolicy(useId, action);
}
