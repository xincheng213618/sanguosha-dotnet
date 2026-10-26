# 庞德·移动版

核查日期：2026-10-26。普通移动身份，群、男、4体力、神话再临火包，完整引用 `classic:mashu` 与 `classic:mengjin`。没有新增技能JSON、运行时或重复人物快照。

来源：[移动官网39](https://www.sanguosha.cn/pc/hero-detail-39.html)、[移动BWIKI人物35418](https://wiki.biligame.com/msgs/index.php?title=庞德&oldid=35418)、[马术/庞德10199](https://wiki.biligame.com/msgs/index.php?title=马术/庞德&oldid=10199)、[猛进39003](https://wiki.biligame.com/msgs/index.php?title=猛进&oldid=39003)。原始MediaWiki revisions API核实时间分别为2025-12-30、2022-09-14、2026-06-11；当前经典人物为火包、80将魂或招募获得。`猛进/庞德` 不是当前技能页，不能拿不存在的页面引用；实际页面名就是 `猛进`。官网39列出两个技能名但漏猛进正文，使用当前移动Wiki明确经典栏补足。

马术是本人计算到其他角色的距离减一；现有 `classic:mashu` 仅 `outgoingDistance/add:-1`，没有把其他角色到本人的距离减一，也没有冒充攻击范围加一。

猛进是在本人使用的杀被该目标的闪完整抵消后，可弃置该目标一张手牌或装备牌。现有 `classic:mengjin` 使用原生 `slashFullyDodged`、ownerRelation actor，取得原杀的实际目标；不是只打出第一张闪就发动，也不是任意取消杀效果、决斗打出杀或伤害后触发。普通杀、火杀、雷杀沿完整旧杀族兼容覆盖冰杀。无出牌阶段或每回合一次的额外限制。牌区沿现有HE弃置协议，手牌只发布不透明位置，装备为公开原实体；原生弃牌及其触发子窗返回原猛进帧，不制造攻击者先获得再弃牌的中间过程。

`MengjinChecks.HiddenHandPublicEquipmentChoiceAndReplay` 已有真实杀被闪抵消、目标手牌位置与公开装备选择、实体弃置以及冷恢复检查；本次纯完整引用无需另复制夹具。该检查本次尚未由本组执行，统一验证由主代理完成。未混用界庞德先判定装备/非装备后获得原杀的技能，也未采用SP庞德的决死。

普通经典图：[3900.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/3900.jpg)，官网39第二张；首图43400为界版，不采用。`portraitKey` 为 `mobile-pang-de`，图鉴分组 `myth-fire`，人物/将池86099。
