#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5YuanshuEntries
{
    internal bool ShanheS5TongjiAllowsSlash(ProgramSlashTargetQuery query) => _resolve().ShanheS5TongjiAllowsSlash(query);
    internal bool ShanheS5TongjiProhibitsSlashTarget(ProgramDirectedTargetQuery query) => _resolve().ShanheS5TongjiProhibitsSlashTarget(query);
}
