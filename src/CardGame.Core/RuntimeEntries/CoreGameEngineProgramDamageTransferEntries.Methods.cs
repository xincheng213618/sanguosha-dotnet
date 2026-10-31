#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDamageTransferEntries
{
    internal void RedirectProgramCurrentDamage(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind, bool drawLostHpAfterDamage) => _resolve().RedirectProgramCurrentDamage(frame, sourceBind, drawLostHpAfterDamage);
}
