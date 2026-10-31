#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenheChaosEntries
{
    internal void BeginWenheCardChaos(global::CardGame.Core.CardUseFrame use) => _resolve().BeginWenheCardChaos(use);
    internal void BeginWenheChaos(global::CardGame.Core.ProgramSkillFrame? roundParent, global::CardGame.Core.CardUseFrame? cardParent) => _resolve().BeginWenheChaos(roundParent, cardParent);
    internal bool IsExactWenheChaos(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsExactWenheChaos(frame);
    internal bool TryCompleteWenheChaosProgram(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => _resolve().TryCompleteWenheChaosProgram(frame, completed);
    internal bool WenheChaosFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().WenheChaosFrameRidesOn(child, parent);
}
