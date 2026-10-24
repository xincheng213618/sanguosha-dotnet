namespace CardGame.Core;

internal sealed partial class MatchModeRuntime
{
    internal IWushuangModeFeature? Wushuang => Find<IWushuangModeFeature>();
}
