# 祝融·移动版

> 此版本与 `classic:zhu-rong` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核查日期：2026-10-26。普通移动身份，蜀、女、4体力、神话再临林包。完整引用 `classic:juxiang` 与 `classic:lieren`，不复制技能或增加运行时。

来源：[移动官网46](https://www.sanguosha.cn/pc/hero-detail-46.html)、[移动BWIKI人物39023](https://wiki.biligame.com/msgs/index.php?title=祝融&oldid=39023)、[巨象20612](https://wiki.biligame.com/msgs/index.php?title=巨象/祝融&oldid=20612)、[烈刃38993](https://wiki.biligame.com/msgs/index.php?title=烈刃/祝融&oldid=38993)。原始 revisions API 核实人物/烈刃为2026-06-11、巨象为2023-01-03；经典栏为林包、开服上线、160将魂/招募获取。官网巨象正文与Wiki一致，但缺烈刃正文，后者以当前普通技能修订补足。

巨象是南蛮对本人无效，其他角色使用的南蛮全部结算、实际置入弃牌堆后获得该牌。原生 `classic:juxiang` 的 `claimResolvedGlobalCard` 在Group完毕逐个检查完整 `PhysicalCards`，只领取仍真实处于Discard的原实体；不是只取代表牌ID，也不是提前获得Processing中的材料。资格用原Group的 `SourceSeat` 判断其他角色使用，不用祸首改变后的 `DamageSourceSeat`。本人使用的南蛮不会被自己再次领取；已被奸雄等效果领走而不在弃牌堆的实体不会被重新生成或从他人手中取回。原始使用、目标及伤害来源仍由同一个原生CardUse/Group帧保存。

烈刃与现有 `classic:lieren` 相同：本人使用杀对原目标实际造成直接伤害后，每次伤害事件可选择拼点，不是指定目标时发动，也不是每一点伤害重复发动。规则 `afterDamageApplied/damageSource/perDamage` 搭配 `directCardUseDamage` 和原另一伤害参与者存活条件，排除无来源、传导伤害、技能伤害等替代入口；普通、火、雷的完整旧杀族兼容冰杀。真实双方都必须有合法手牌才能拼点，平点不算赢；胜利后只从原目标HE取一张真实牌，无牌则正常跳过，不领取判定区牌，也不交换双方拼点牌。

当前人物修订的FAQ明确本人南蛮不回收、奸雄已获得则不再取得、烈刃胜后目标无牌无收益、可取装备不可取判定区。以上均与实码的实体位置、原参与者及HE选择一致。没有采用界祝融指定目标时拼点/未赢交换拼点牌的另一规则。

本次纯内容引用沿用已有南蛮实体领取和共享拼点/牌移动检查，不添加重复人物快照；本组未自行运行构建，统一验证由主代理执行。

普通经典图：[4600.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/4600.jpg)，官网46第二张，首图44500为界版。`portraitKey` 为 `mobile-zhu-rong`，图鉴分组 `myth-forest`，人物/将池86599。
