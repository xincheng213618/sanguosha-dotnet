#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDamageReductionEntries
{
    internal void ReduceProgramCurrentDamage(global::CardGame.Core.ProgramSkillFrame program, int amount) => _resolve().ReduceProgramCurrentDamage(program, amount);
}
