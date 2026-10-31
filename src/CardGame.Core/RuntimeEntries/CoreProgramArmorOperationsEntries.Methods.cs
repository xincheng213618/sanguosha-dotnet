#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramArmorOperationsEntries
{
    internal void GrantProgramArmor(ProgramSkillFrame frame) => _resolve().GrantProgramArmor(frame);
}
