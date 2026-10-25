# 移动版界司马懿

核对日期：2026-10-25。独立身份 `boundary:sima-yi-mobile`，魏、男性、三体力，画像键 `boundary-sima-yi-mobile`，规则集 `sanguosha-mobile`。

- [移动版官网武将详情165](https://www.sanguosha.cn/index.php/pc/hero-detail-165.html)：完整身份HTML的反馈为每受到一点伤害后，可获得来源的一张牌；隐藏的第二技能正文明确鬼才可打出一张牌替代任一角色生效前的判定牌。官网玩法也明确可以预留装备改判。
- [移动版Wiki界司马懿](https://wiki.biligame.com/msgs/%E7%95%8C%E5%8F%B8%E9%A9%AC%E6%87%BF)：交叉核对三体力、逐点反馈及鬼才手牌与装备的范围。页面历史普通版的整次伤害反馈、仅手牌鬼才不用于本人物。

与已有 `boundary:feedback`、`boundary:guicai` 的规则完整相同，直接复用，不创建重复技能或运行时。反馈使用已有逐伤害点原生事件，来源一张牌仍指手牌或装备，不能取得判定区牌。鬼才沿现有原生判定替换窗口，以本人真实手牌或装备牌作为替换实体，旧判定牌进入弃牌堆；其装备离区、获得牌、子结算及冷恢复均保持既有原母帧、真实移动和付费不重复合同。

本批同时修复被复用的反馈程序对自源伤害的既有遗漏：其选来源牌节点原来默认拒绝伤害来源与本人相同，现在显式允许该来源。规则文字未排除本人；[移动台服官方司马懿说明与FAQ](https://www.sanguosha.com.tw/newsinfo_921.html)（2021-07-02）同时列出相同界限突破反馈、鬼才，并明确连环传导时反馈的对象为伤害来源，包含自己。这里仅补原有来源资格，不改反馈逐点次数、HE范围或经典版反馈程序。

新增一条针对该实际缺口的行为回归：以本移动身份的真实原生自源一点伤害触发反馈，取得本人装备至手牌，并在已经完成移动的真实获得牌子窗冷恢复，核对原帧返回、一次移动和一次完成。其余纯配置复用不增加重复夹具。整批沿用 `damage programs claim select gift draw and replay through generic choices` 及 `skill program judgment replacement commits both old-card destinations and replays`，统一图鉴检查核准新增身份、独立立绘与完整技能正文。
