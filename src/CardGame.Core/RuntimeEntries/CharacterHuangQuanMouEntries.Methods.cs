#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangQuanMouEntries
{
    internal void IssueMouHqOriginalEffect(ProgramSkillFrame f) => _resolve().IssueMouHqOriginalEffect(f);
    internal bool MouHqFinalizedWindow(ProgramTargetCardWindowQuery q) => _resolve().MouHqFinalizedWindow(q);
    internal bool MouHqParent(ProgramSkillFrame f) => _resolve().MouHqParent(f);
    internal bool TryMouHqOriginalUse(int owner, ProgramSkillWindowContext? context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryMouHqOriginalUse(owner, context, out use, out window);
    internal bool ValidMouHq(ProgramSkillFrame f) => _resolve().ValidMouHq(f);
    internal bool HasMouHuangQuan { get => _resolve().HasMouHuangQuan; }
    internal MouHuangQuanRuntime MouHuangQuan { get => _resolve().MouHuangQuan; }
}
