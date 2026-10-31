#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNonBasicSlashProfileEntries
{
    internal bool DistancePhysicalSlash80HasIssuedNonBasicSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash80HasIssuedNonBasicSlashDistance(q);
    internal bool HasIssuedNonBasicSlashDistance(long? useId) => _resolve().HasIssuedNonBasicSlashDistance(useId);
    internal bool IgnoresNonBasicSlashDistance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource source) => _resolve().IgnoresNonBasicSlashDistance(owner, source);
    internal bool IgnoresNonBasicSlashDistanceQuery(global::CardGame.Core.ProgramSlashConversionDistanceQuery q) => _resolve().IgnoresNonBasicSlashDistanceQuery(q);
    internal void IssueNonBasicSlashPolicy(long useId, global::CardGame.Core.CardActionContext? action) => _resolve().IssueNonBasicSlashPolicy(useId, action);
}
